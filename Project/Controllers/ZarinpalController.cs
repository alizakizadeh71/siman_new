//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json.Linq;
using OPS.CBINasimService;
using OPS.ir.shaparak.sadad;
using RestSharp;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Cement;
using ViewModels.Areas.Administrator.ZarinPal;

namespace OPS.Controllers
{
    public partial class ZarinpalController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();
        PGServiceClient oIPGServices = new PGServiceClient();
        //string testamount = "10017";

        public virtual ActionResult Payment(int invoiceNumber, string MahalTahvil, Guid? CarrierId, string DriverName, string DriverMobile)
        {
            try
            {
                string carrierName = "نامشخص";
                string carrierMobile = "";

                var factor = UnitOfWork.FactorCementRepository
                    .GetByinvoicenumber(invoiceNumber)
                    .FirstOrDefault();

                if (factor == null)
                    return HttpNotFound();

                if (factor.FinalApprove || factor.ref_id > 0 || factor.Authority != null)
                    return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });

                var lastInvoice = Session["LastInvoiceNumber"];
                if (lastInvoice == null || (int)lastInvoice != invoiceNumber)
                    return RedirectToAction("Index", "HomeMain");

                Session.Remove("LastInvoiceNumber");

                var user = UnitOfWork.UserRepository.GetById(factor.UserId);
                factor.MahalTahvil = MahalTahvil;

                // ── باربری ──
                if (CarrierId.HasValue && CarrierId.Value != Guid.Empty)
                {
                    var carrierInventory = UnitOfWork.CarrierInventoryRepository
                        .GetByProduct(factor.ProductNameId, factor.ProductTypeId, factor.PackageTypeId, factor.FactoryNameId)
                        .FirstOrDefault(x => x.CarrierId == CarrierId.Value);

                    if (carrierInventory == null || carrierInventory.InventoryTonnage < factor.Tonnagedouble)
                    {
                        TempData["Error"] = "موجودی باربری انتخاب شده کافی نیست. لطفاً باربری دیگری انتخاب کنید.";
                        return RedirectToAction("Index", "HomeMain", new
                        {
                            productId = factor.ProductNameId,
                            typeId = factor.ProductTypeId,
                            packageId = factor.PackageTypeId,
                            factoryId = factor.FactoryNameId
                        });
                    }

                    factor.CarrierId = CarrierId;

                    var carrierUser = UnitOfWork.UserRepository.GetById(CarrierId.Value);
                    if (carrierUser != null && carrierUser.Role.Code == (int)Enums.Roles.Carrier)
                    {
                        carrierName = carrierUser.FullName;
                        carrierMobile = carrierUser.BuyerMobile;
                    }
                }

                if (!string.IsNullOrEmpty(DriverName)) factor.DriverName = DriverName;
                if (!string.IsNullOrEmpty(DriverMobile)) factor.DriverMobile = DriverMobile;

                // ── موجودی انبار ──
                var inventory = UnitOfWork.InventoryamountRepository.Get()
                    .FirstOrDefault(x =>
                        !x.IsDeleted &&
                        x.IsActived != false &&
                        x.ProductNameId == factor.ProductNameId &&
                        x.ProductTypeId == factor.ProductTypeId &&
                        x.PackageTypeId == factor.PackageTypeId &&
                        x.FactoryNameId == factor.FactoryNameId);

                // ── محاسبه مبلغ ──
                long walletUsed = Math.Min(user.creditAmount, Convert.ToInt64(factor.AmountPaid));
                long gatewayAmount = Convert.ToInt64(factor.AmountPaid) - walletUsed;

                user.creditAmount -= walletUsed;
                factor.WalletPaidAmount = walletUsed;
                factor.OnlinePaidAmount = gatewayAmount;
                factor.FinalApprove = (gatewayAmount == 0);

                UnitOfWork.UserRepository.Update(user);
                UnitOfWork.FactorCementRepository.Update(factor);
                UnitOfWork.Save();

                // ── اطلاعات مشترک پیامک ──
                string fullProductName = BuildProductName(factor, includesTonnage: true);
                string buyerMobile = !string.IsNullOrEmpty(user.BuyerMobile) ? user.BuyerMobile : factor.BuyerMobile;
                string buyerName = (!string.IsNullOrEmpty(user.FullName) && user.FullName != "کاربر مهمان")
                                     ? user.FullName : factor.BuyerFullName;

                bool isGuest = user.FullName == "کاربر مهمان" || user.FullName == "میهمان";
                long balance = isGuest ? 0 : user.InitialCredit - user.creditAmount;
                string remainAmount = isGuest ? null : Math.Abs(balance).ToString("N0");
                string remainStatus = isGuest ? null : (balance > 0 ? "بدهکار" : balance < 0 ? "طلبکار" : "تسویه");

                // ── پرداخت کامل با کیف پول ──
                if (gatewayAmount == 0)
                {
                    if (inventory != null) inventory.Inventorytonnage -= factor.Tonnagedouble;
                    UnitOfWork.InventoryamountRepository.Update(inventory);

                    // ✅ کاهش موجودی باربری
                    DeductCarrierInventory(factor);

                    CreateMarketerTransaction(factor, user);
                    UnitOfWork.Save();

                    SendOrderSMS(buyerMobile, buyerName, factor, fullProductName, remainAmount, remainStatus, carrierName, carrierMobile);

                    return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });
                }

                // ── حالت دیباگ ──
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    factor.ref_id = 123456789;
                    factor.card_pan = "1234-****-****-5678";
                    factor.Bankcode = 100;
                    factor.AmountPaidDate = DateTime.Now;
                    factor.FinalApprove = true;

                    if (inventory != null) inventory.Inventorytonnage -= factor.Tonnagedouble;
                    UnitOfWork.InventoryamountRepository.Update(inventory);

                    // ✅ کاهش موجودی باربری
                    DeductCarrierInventory(factor);

                    UnitOfWork.FactorCementRepository.Update(factor);
                    CreateMarketerTransaction(factor, user);
                    UnitOfWork.Save();

                    SendOrderSMS(buyerMobile, buyerName, factor, fullProductName, remainAmount, remainStatus, carrierName, carrierMobile);

                    return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });
                }

                // ── درگاه پرداخت ──
                string description = $"{factor.ProductName.Name} - {factor.PackageType.Name} - {factor.FactoryName.Name} - {factor.Tonnagedouble}";
                string callback = "https://masalehpakhsh.com/Zarinpal/VerifyPayment";

                var requestObj = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(
                    "d9c07ec3-6934-41f3-b6d4-a7eecedf3114",
                    gatewayAmount.ToString(),
                    description,
                    callback,
                    factor.BuyerMobile,
                    "ali_animax@yahoo.com"
                );

                var client = new RestClient(URLs.requestUrl);
                var request = new RestRequest("", Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(requestObj);

                var response = client.Execute(request);
                var jo = JObject.Parse(response.Content);

                if (jo["data"] != null && jo["data"].HasValues)
                {
                    factor.Authority = jo["data"]["authority"].ToString();
                    UnitOfWork.FactorCementRepository.Update(factor);
                    UnitOfWork.Save();
                    return Redirect(URLs.gateWayUrl + factor.Authority);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual ActionResult VerifyPayment()
        {
            try
            {
                string authority = Request.QueryString["Authority"];
                string status = Request.QueryString["Status"];

                var factor = UnitOfWork.FactorCementRepository.GetByAuthority(authority).FirstOrDefault();
                if (factor == null) return View(new CementViewModel());

                if (factor.FinalApprove)
                    return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });

                var user = UnitOfWork.UserRepository.GetById(factor.UserId);

                // ── تایید درگاه ──
                var parameters = new VerifyParameters
                {
                    authority = factor.Authority,
                    merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114",
                    amount = factor.OnlinePaidAmount.ToString()
                };

                var client = new RestClient(URLs.verifyUrl);
                var request = new RestRequest("", Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var jo = JObject.Parse(client.Execute(request).Content);

                if (jo["data"] == null || status != "OK")
                    return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });

                // ── اطلاعات باربری ──
                string carrierName = "ثبت نشده";
                string carrierMobile = "";

                if (factor.CarrierId.HasValue && factor.CarrierId.Value != Guid.Empty)
                {
                    var carrierUser = UnitOfWork.UserRepository.GetById(factor.CarrierId.Value);
                    if (carrierUser != null && carrierUser.Role.Code == (int)Enums.Roles.Carrier)
                    {
                        carrierName = carrierUser.FullName;
                        carrierMobile = carrierUser.BuyerMobile;
                    }

                    // ✅ کاهش موجودی باربری
                    DeductCarrierInventory(factor);
                }

                // ── ثبت اطلاعات پرداخت ──
                factor.ref_id = Convert.ToInt64(jo["data"]["ref_id"]);
                factor.card_pan = jo["data"]["card_pan"].ToString();
                factor.Bankcode = Convert.ToInt32(jo["data"]["code"]);
                factor.AmountPaidDate = DateTime.Now;
                factor.FinalApprove = true;

                // ── کاهش موجودی انبار اصلی ──
                var inventory = UnitOfWork.InventoryamountRepository.Get()
                    .FirstOrDefault(x =>
                        x.ProductNameId == factor.ProductNameId &&
                        x.ProductTypeId == factor.ProductTypeId &&
                        x.PackageTypeId == factor.PackageTypeId &&
                        x.FactoryNameId == factor.FactoryNameId);

                if (inventory != null)
                {
                    inventory.Inventorytonnage -= factor.Tonnagedouble;
                    UnitOfWork.InventoryamountRepository.Update(inventory);
                }

                CreateMarketerTransaction(factor, user);
                UnitOfWork.FactorCementRepository.Update(factor);
                UnitOfWork.Save();

                // ── ارسال پیامک ──
                string fullProductName = BuildProductName(factor, includesTonnage: true);
                string buyerMobile = !string.IsNullOrEmpty(user.BuyerMobile) ? user.BuyerMobile : factor.BuyerMobile;
                string buyerName = (!string.IsNullOrEmpty(user.FullName) && user.FullName != "میهمان")
                                     ? user.FullName : factor.BuyerFullName;

                bool isGuest = user.FullName == "کاربر مهمان" || user.FullName == "میهمان";

                if (isGuest)
                {
                    Utilities.SMS.SmsUtility.SendGuestLoadedNotification(
                        buyerMobile,
                        buyerName,
                        factor.InvoiceNumber.ToString(),
                        fullProductName,
                        factor.AmountPaid.ToString("N0"),
                        carrierName,
                        carrierMobile);
                }
                else
                {
                    long balance = user.InitialCredit - user.creditAmount;
                    string remainAmount = Math.Abs(balance).ToString("N0");
                    string remainStatus = balance > 0 ? "بدهکار" : balance < 0 ? "طلبکار" : "تسویه";

                    Utilities.SMS.SmsUtility.SendLoadedNotification(
                        buyerMobile,
                        buyerName,
                        factor.InvoiceNumber.ToString(),
                        fullProductName,
                        factor.AmountPaid.ToString("N0"),
                        remainAmount,
                        remainStatus,
                        carrierName,
                        carrierMobile);
                }

                // ── پیامک به باربری ──
                if (!string.IsNullOrEmpty(carrierMobile))
                {
                    bool hasDriver = !string.IsNullOrEmpty(factor.DriverName) || !string.IsNullOrEmpty(factor.DriverMobile);

                    if (hasDriver)
                    {
                        Utilities.SMS.SmsUtility.SendCarrierNotificationWithDriver(
                            carrierMobile,
                            carrierName,
                            factor.InvoiceNumber.ToString(),
                            fullProductName,
                            buyerName,
                            buyerMobile,
                            factor.DriverName ?? "",
                            factor.DriverMobile ?? "");
                    }
                    else
                    {
                        Utilities.SMS.SmsUtility.SendCarrierNotification(
                            carrierMobile,
                            carrierName,
                            factor.InvoiceNumber.ToString(),
                            fullProductName,
                            buyerName,
                            buyerMobile);
                    }
                }

                return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = factor.InvoiceNumber });
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در تایید پرداخت: " + ex.Message);
            }
        }

        public static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.FactorCement oFactorCement)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.Id = oFactorCement.Id;
            cementViewModel.InvoiceNumber = oFactorCement.InvoiceNumber;
            cementViewModel.StringInsertDateTime = new Infrastructure.Calander(oFactorCement.InsertDateTime).Persion();
            cementViewModel.BuyerMobile = oFactorCement.BuyerMobile;
            cementViewModel.BuyerName = oFactorCement.User.FullName;
            cementViewModel.RequestState = oFactorCement.RequestState;
            cementViewModel.BuyerNationalCode = oFactorCement.User.NationalCode;
            cementViewModel.StringProductName = oFactorCement.ProductName.Name;
            cementViewModel.StringProductType = oFactorCement.ProductType.Name;
            cementViewModel.StringPackageType = oFactorCement.PackageType.Name;
            cementViewModel.StringFactoryName = oFactorCement.FactoryName.Name;
            cementViewModel.StringTonnage = oFactorCement.Tonnagedouble.ToString();
            cementViewModel.AmountPaid = oFactorCement.AmountPaid;
            cementViewModel.DestinationAmountPaid = oFactorCement.DestinationAmountPaid != null ? oFactorCement.DestinationAmountPaid.Value : 0;
            cementViewModel.MahalTahvil = oFactorCement.MahalTahvil == "Karkhane" ? "درب کارخانه" : oFactorCement.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ";
            cementViewModel.ref_id = oFactorCement.ref_id.ToString();
            cementViewModel.card_pan = oFactorCement.card_pan;
            cementViewModel.Address = oFactorCement.Address;
            cementViewModel.RemittanceNumber = oFactorCement.RemittanceNumber;
            return cementViewModel;
        }
        public virtual ActionResult Paymentwallet(int Chargeamount, int invoiceNumber)
        {
            try
            {
                var wallet = UnitOfWork.walletFactorRepository
                    .GetByinvoicenumber(invoiceNumber)
                    .FirstOrDefault();

                if (wallet == null) return HttpNotFound();

                var user = UnitOfWork.UserRepository.GetById(wallet.UserId);

                string callback = System.Diagnostics.Debugger.IsAttached
                    ? "http://localhost:6066/Zarinpal/VerifyPaymentWallet"
                    : "https://masalehpakhsh.com/Zarinpal/VerifyPaymentWallet";

                var req = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(
                    "d9c07ec3-6934-41f3-b6d4-a7eecedf3114",
                    Chargeamount.ToString(),
                    "شارژ کیف پول - " + user.UserName,
                    callback,
                    user.BuyerMobile,
                    "ali_animax@yahoo.com"
                );

                var client = new RestClient(URLs.requestUrl);
                var request = new RestRequest("", Method.Post);

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(req);

                var response = client.Execute(request);
                var jo = JObject.Parse(response.Content);

                if (jo["data"] == null) return null;

                wallet.Authority = jo["data"]["authority"].ToString();

                UnitOfWork.walletFactorRepository.Update(wallet);
                UnitOfWork.Save();

                return Redirect(URLs.gateWayUrl + wallet.Authority);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual ActionResult VerifyPaymentWallet()
        {
            var vm = new CementViewModel();

            try
            {
                string authority = Request.QueryString["Authority"];
                string status = Request.QueryString["Status"];

                var wallet = UnitOfWork.walletFactorRepository
                    .GetByAuthority(authority)
                    .FirstOrDefault();

                if (wallet == null) return View(vm);

                if (wallet.FinalApprove)
                    return View(ConvertCementViewModel(wallet));

                var user = UnitOfWork.UserRepository.GetById(wallet.UserId);

                var parameters = new VerifyParameters
                {
                    authority = wallet.Authority,
                    merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114",
                    amount = wallet.Chargeamount.ToString()
                };

                var client = new RestClient(URLs.verifyUrl);
                var request = new RestRequest("", Method.Post);

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.Execute(request);
                var jo = JObject.Parse(response.Content);

                if (jo["data"] != null && status == "OK")
                {
                    wallet.ref_id = Convert.ToInt64(jo["data"]["ref_id"]);
                    wallet.card_pan = jo["data"]["card_pan"].ToString();
                    wallet.Bankcode = Convert.ToInt32(jo["data"]["code"]);
                    wallet.AmountPaidDate = DateTime.Now;
                    wallet.FinalApprove = true;

                    user.creditAmount += wallet.Chargeamount;

                    UnitOfWork.UserRepository.Update(user);
                    UnitOfWork.walletFactorRepository.Update(wallet);
                    UnitOfWork.Save();

                    PaymentSMSWallet(user.BuyerMobile, wallet);

                    vm = ConvertCementViewModel(wallet);
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.walletFactor oFactorCement)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.Id = oFactorCement.Id;
            cementViewModel.BuyerMobile = oFactorCement.BuyerMobile;
            cementViewModel.AmountPaid = oFactorCement.Chargeamount;
            cementViewModel.ref_id = oFactorCement.ref_id.ToString();
            cementViewModel.InvoiceNumber = oFactorCement.InvoiceNumber;
            cementViewModel.BuyerName = oFactorCement.User.UserName;
            cementViewModel.PayEndDate = oFactorCement.AmountPaidDate;
            cementViewModel.card_pan = oFactorCement.card_pan;
            cementViewModel.BuyerNationalCode = oFactorCement.User.NationalCode;
            return cementViewModel;
        }

        public virtual ActionResult PaymentSMS(string phoneNumber, Models.FactorCement factor)
        {
            try
            {
                var user = UnitOfWork.UserRepository.GetByPhoneNumebr(phoneNumber);

                // اگر کاربر وجود دارد اما تنظیمات دریافت پیامک او غیرفعال است، خارج می‌شویم
                if (user != null && !user.isSendSms)
                {
                    return Content("<script>console.log('ارسال پیامک برای این کاربر غیرفعال است.');</script>", "text/html");
                }

                const string username = "989926932699";
                const string password = "#57PD";

                // کد پترن جدید که در پنل ثبت می‌کنید را اینجا قرار دهید
                const int bodyId = 448845;

                string to = user?.BuyerMobile?.ToString() ?? phoneNumber;

                // --- محاسبه وضعیت حساب ---
                string accountStatus = "تسویه";
                string balanceDetails = "0";

                if (user != null)
                {
                    long balanceAmount = user.InitialCredit - user.creditAmount;

                    if (balanceAmount > 0)
                    {
                        accountStatus = "بدهکار";
                        balanceDetails = Math.Abs(balanceAmount).ToString("N0");
                    }
                    else if (balanceAmount < 0)
                    {
                        accountStatus = "طلبکار";
                        balanceDetails = Math.Abs(balanceAmount).ToString("N0");
                    }
                }

                // --- آماده‌سازی پارامترهای پیامک به ترتیب دقیق الگوی ۶ متغیره ---
                string p0_Name = user?.UserName ?? "مشتری";
                string p1_FactorNum = factor.InvoiceNumber.ToString();

                // ترکیب تناژ، کارخانه، نوع و بسته‌بندی برای متغیر دوم (شرح کل)
                string tonnage = $"{factor.Tonnagedouble.ToString().Replace('.', '/')} تن";
                string factory = factor.FactoryName?.Name ?? "";
                string productType = factor.ProductType?.Name ?? "";
                string packageType = factor.PackageType?.Name ?? "";

                string p2_Description = $"{tonnage} {factory} {productType} {packageType}".Trim();
                if (string.IsNullOrWhiteSpace(p2_Description)) p2_Description = "نامشخص";

                string p3_Price = factor.AmountPaid.ToString("N0"); // مبلغ فی
                string p4_Balance = balanceDetails;                 // مبلغ مانده
                string p5_Status = accountStatus;                   // وضعیت (بدهکار/طلبکار/تسویه)

                // مپ کردن مقادیر به آرایه (دقیقا ۶ آیتم)
                string[] text = new string[]
                {
                    p0_Name,         // {0} 
                    p1_FactorNum,    // {1} 
                    p2_Description,  // {2} 
                    p3_Price,        // {3} 
                    p4_Balance,      // {4} 
                    p5_Status        // {5} 
                };

                // --- ارسال پیامک ---
                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport }
                };

                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                var result = soapClient.SendByBaseNumber(username, password, text, to, bodyId);

                return Content($"<script>console.log('پیامک با موفقیت ارسال شد. کد نتیجه: {result}');</script>", "text/html");
            }
            catch (Exception ex)
            {
                return Content($"<script>console.error('خطا در ارسال پیامک: {ex.Message}');</script>", "text/html");
            }
        }


        public void PaymentSMSWallet(string phoneNumber, Models.walletFactor factor)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString;

                using (var testConnection = new SqlConnection(connStr))
                {
                    try
                    {
                        testConnection.Open();
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"خطا در اتصال به دیتابیس: {dbEx.Message}");
                        return;
                    }
                }
                var user = UnitOfWork.UserRepository.GetByPhoneNumebr(phoneNumber);
                if (user != null && user.isSendSms == true)
                {
                    const string username = "989926932699";
                    const string password = "#57PD";
                    const int bodyId = 284335;
                    string to = user.BuyerMobile.ToString();

                    string accountStatus = "متعادل";
                    string balanceDetails = "0"; // جزئیات بدهکاری یا طلبکاری

                    if (user != null)
                    {
                        long balanceAmount = user.InitialCredit - user.creditAmount;

                        if (balanceAmount > 0)
                        {
                            accountStatus = "بدهکار";
                            balanceDetails = $"{Math.Abs(balanceAmount).ToString("N0")}";
                        }
                        else if (balanceAmount < 0)
                        {
                            accountStatus = "طلب کار";
                            balanceDetails = $"{Math.Abs(balanceAmount).ToString("N0")}";
                        }
                        else
                        {
                            accountStatus = "";
                        }
                    }

                    // string text = $"{user.UserName};{factor.Tonnage.Name};{factor.PackageType.Name};{factor.FactoryName.Name};{factor.ProductType.Name};{factor.AmountPaid};{user.InitialCredit}";
                    string[] text = {
                        user.UserName,
                        factor.Chargeamount.ToString("N0"),
                        balanceDetails,
                        accountStatus
                    };

                    const bool isFlash = false;

                    // تنظیمات BasicHttpBinding برای پشتیبانی از HTTPS
                    var binding = new BasicHttpBinding
                    {
                        Security = new BasicHttpSecurity
                        {
                            Mode = BasicHttpSecurityMode.Transport // برای پشتیبانی از HTTPS
                        }
                    };

                    // آدرس Endpoint با استفاده از HTTPS
                    var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");

                    // ایجاد کلاینت SOAP
                    var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                    // ارسال پیامک
                    var result = soapClient.SendByBaseNumber(username, password, text, to, bodyId);
                    Console.WriteLine($"پیامک با موفقیت ارسال شد. نتیجه: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا رخ داد: {ex.Message}");
            }
        }

        public bool SendSMSdebtor(string[] phoneNumbers)
        {

            try
            {
                const string username = "989926932699";
                const string from = "";
                const string text = "";
                const string password = "#57PD";
                const bool isFlash = false;
                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };
                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);
                var result = soapClient.SendSimpleSMS(username, password, phoneNumbers, from, text, isFlash);

                // بازگرداندن نتیجه به صورت جاوا اسکریپت
                return true;
            }
            catch (Exception ex)
            {
                // بازگرداندن خطا به صورت جاوا اسکریپت
                return false;
            }
        }

        public bool SendMarketingMessage(string phoneNumber, string messageText)
        {
            try
            {
                const string username = "989926932699";
                const string password = "#57PD";
                const string linksite = "https://masalehpakhsh.com/HomeMain/GetLivePrice";

                string[] texts = { messageText, linksite };

                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };

                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);

                int bodyId = 367213; // مقدار واقعی bodyId
                soapClient.SendByBaseNumber(username, password, texts, phoneNumber, bodyId);

                Console.WriteLine($"پیامک به {phoneNumber} ارسال شد.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا در ارسال پیامک: {ex.Message}");
                return false;
            }
        }

        private void CreateMarketerTransaction(FactorCement factor, User user)
        {
            if (user.ReferredByCode == null) return;

            var marketer = UnitOfWork.UserRepository
                .GetUserByMarketingCode(user.ReferredByCode);

            if (marketer == null) return;

            var commission = (int)(factor.AmountPaid * 0.007);

            var transaction = new MarketerTransactions
            {
                Id = Guid.NewGuid(),
                MarketingCode = int.Parse(marketer.MarketingCode),
                ReferredCode = int.Parse(user.ReferredByCode),
                ProductNameId = factor.ProductNameId,
                ProductTypeId = factor.ProductTypeId,
                PackageType = factor.PackageType,
                FactoryName = factor.FactoryName,
                Tonnagedouble = factor.Tonnagedouble,
                CommissionAmount = commission,
                InsertDateTime = DateTime.Now
            };

            UnitOfWork.MarketerTransactionsRepository.Insertdata(transaction);

            marketer.creditAmount += commission;
        }

        private string BuildProductName(Models.FactorCement factor, bool includesTonnage = false)
        {
            string productName = UnitOfWork.ProductNameRepository.GetById(factor.ProductNameId)?.Name ?? "";
            string productType = UnitOfWork.ProductTypeRepository.GetById(factor.ProductTypeId)?.Name ?? "";
            string packageType = UnitOfWork.PackageTypeRepository.GetById(factor.PackageTypeId)?.Name ?? "";
            string factoryName = UnitOfWork.FactoryNameRepository.GetById(factor.FactoryNameId)?.Name ?? "";

            string name = includesTonnage
                ? $"{factor.Tonnagedouble} تن {factoryName} {productType} {packageType}"
                : $"{productName} {productType} {packageType} {factoryName}";

            return string.IsNullOrWhiteSpace(name.Trim()) ? "محصول" : name.Trim();
        }
        private void SendOrderSMS(
            string buyerMobile, string buyerName,
            Models.FactorCement factor, string fullProductName,
            string remainAmount, string remainStatus,
            string carrierName, string carrierMobile)
        {
            bool isGuest = string.IsNullOrEmpty(remainAmount);

            // ── پیامک به مشتری ──
            if (isGuest)
            {
                Utilities.SMS.SmsUtility.SendGuestLoadedNotification(
                    buyerMobile,
                    buyerName,
                    factor.InvoiceNumber.ToString(),
                    fullProductName,
                    factor.AmountPaid.ToString("N0"),
                    carrierName,
                    carrierMobile);
            }
            else
            {
                Utilities.SMS.SmsUtility.SendLoadedNotification(
                    buyerMobile,
                    buyerName,
                    factor.InvoiceNumber.ToString(),
                    fullProductName,
                    factor.AmountPaid.ToString("N0"),
                    remainAmount,
                    remainStatus,
                    carrierName,
                    carrierMobile);
            }

            // ── پیامک به باربری ──
            if (string.IsNullOrEmpty(carrierMobile)) return;

            bool hasDriver = !string.IsNullOrEmpty(factor.DriverName) || !string.IsNullOrEmpty(factor.DriverMobile);

            if (hasDriver)
            {
                Utilities.SMS.SmsUtility.SendCarrierNotificationWithDriver(
                    carrierMobile,
                    carrierName,
                    factor.InvoiceNumber.ToString(),
                    fullProductName,
                    buyerName,
                    buyerMobile,
                    factor.DriverName ?? "",
                    factor.DriverMobile ?? "");
            }
            else
            {
                Utilities.SMS.SmsUtility.SendCarrierNotification(
                    carrierMobile,
                    carrierName,
                    factor.InvoiceNumber.ToString(),
                    fullProductName,
                    buyerName,
                    buyerMobile);
            }
        }

        /// <summary>
        /// کاهش موجودی باربری پس از تأیید پرداخت
        /// </summary>
        private void DeductCarrierInventory(Models.FactorCement factor)
        {
            if (!factor.CarrierId.HasValue || factor.CarrierId.Value == Guid.Empty)
                return;

            var carrierInventory = UnitOfWork.CarrierInventoryRepository
                .GetByProduct(factor.ProductNameId, factor.ProductTypeId, factor.PackageTypeId, factor.FactoryNameId)
                .FirstOrDefault(x => x.CarrierId == factor.CarrierId);

            if (carrierInventory != null)
            {
                carrierInventory.InventoryTonnage -= factor.Tonnagedouble;
                carrierInventory.UpdateDateTime = DateTime.Now;
                UnitOfWork.CarrierInventoryRepository.Update(carrierInventory);
            }
        }
    }
}