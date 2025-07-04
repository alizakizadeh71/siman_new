//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OPS.CBINasimService;
using OPS.ir.shaparak.sadad;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Utilities.PersianDate;
using ViewModels.Areas.Administrator.ZarinPal;

namespace OPS.Controllers
{
    public partial class ZarinpalController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();
        private Infrastructure.cbinasimService oCBINasim = new Infrastructure.cbinasimService();
        PGServiceClient oIPGServices = new PGServiceClient();
        //string testamount = "10017";

        public virtual ActionResult Payment(int invoiceNumber, string MahalTahvil)
        {
            try
            {
                var oFactorCement = UnitOfWork.FactorCementRepository.GetByinvoicenumber(invoiceNumber).FirstOrDefault();
                var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                oFactorCement.MahalTahvil = MahalTahvil;
                var Tonnage = oFactorCement.Tonnagedouble;
                var InventoryTonnage = UnitOfWork.InventoryamountRepository.Get()
                    .Where(x => x.IsDeleted == false && x.IsActived != false)
                    .Where(x => x.ProductNameId == oFactorCement.ProductNameId)
                    .Where(x => x.ProductTypeId == oFactorCement.ProductTypeId)
                    .Where(x => x.PackageTypeId == oFactorCement.PackageTypeId)
                    .Where(x => x.FactoryNameId == oFactorCement.FactoryNameId)
                    .FirstOrDefault();
                UnitOfWork.FactorCementRepository.Update(oFactorCement);

                string merchant = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";
                string amount = string.Empty;
                if (oFactorCement.MahalTahvil == "Karkhane")
                {
                    if (oFactorCement.AmountPaid > user.creditAmount && user.UserName != "Guest")
                    {
                        amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                    }
                    else if (user.UserName == "Guest")
                    {
                        amount = oFactorCement.AmountPaid.ToString();
                    }
                    else if (oFactorCement.AmountPaid <= user.creditAmount && user.UserName != "Guest")
                    {
                        user.creditAmount = Convert.ToInt64(user.creditAmount - oFactorCement.AmountPaid);
                        UnitOfWork.UserRepository.Update(user);
                        oFactorCement.FinalApprove = true;
                        InventoryTonnage.Inventorytonnage = InventoryTonnage.Inventorytonnage - Tonnage;
                        UnitOfWork.InventoryamountRepository.Update(InventoryTonnage);
                        UnitOfWork.FactorCementRepository.Update(oFactorCement);
                        UnitOfWork.Save();
                        PaymentSMS(user.BuyerMobile, oFactorCement);
                        // Redirect به صفحه مقصد
                        return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = oFactorCement.InvoiceNumber });
                    }
                    //amount = oFactorCement.AmountPaid.ToString();
                }
                else if (oFactorCement.MahalTahvil == "Mahal")
                {
                    if (oFactorCement.AmountPaid > user.creditAmount || user.UserName != "Guest")
                    {
                        amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                        user.creditAmount = 0;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                    }
                    else if (user.UserName != "Guest")
                    {
                        user.creditAmount = Convert.ToInt32(user.creditAmount - oFactorCement.AmountPaid);
                        UnitOfWork.UserRepository.Update(user);
                        oFactorCement.FinalApprove = true;
                        UnitOfWork.FactorCementRepository.Update(oFactorCement);
                        UnitOfWork.Save();
                        // Redirect به صفحه مقصد
                        return RedirectToAction("ShowFactor", "HomeMain", new { invoicenumber = oFactorCement.InvoiceNumber });
                    }
                }
                string authority;
                string description = oFactorCement.ProductName.Name + " - " + oFactorCement.PackageType.Name + " - " + oFactorCement.FactoryName.Name + " - " + oFactorCement.Tonnagedouble;
                string callbackurl = "https://masalehpakhsh.com/Zarinpal/VerifyPayment";
                string mobile = oFactorCement.BuyerMobile;
                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    callbackurl = "http://localhost:6066/Zarinpal/VerifyPayment";
                }

                ViewModels.Areas.Administrator.ZarinPal.RequestParameters Parameters = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(merchant, amount, description, callbackurl, mobile, "ali_animax@yahoo.com");

                var client = new RestClient(URLs.requestUrl);

                Method method = Method.Post;

                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");

                request.AddJsonBody(Parameters);

                var requestresponse = client.ExecuteAsync(request);

                JObject jo = JObject.Parse(requestresponse.Result.Content); /// "authority": "A00000000000000000000000000433573885"

                string errorscode = jo["errors"].ToString();

                JObject jodata = JObject.Parse(requestresponse.Result.Content);

                string dataauth = jodata["data"].ToString();


                if (dataauth != "[]")
                {


                    authority = jodata["data"]["authority"].ToString();

                    oFactorCement.Authority = authority;
                    UnitOfWork.FactorCementRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    string gatewayUrl = URLs.gateWayUrl + authority;

                    return Redirect(gatewayUrl);

                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                //    throw new Exception(ex.Message);
            }
            return null;
        }

        public virtual ActionResult VerifyPayment()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            try
            {
                VerifyParameters parameters = new VerifyParameters();
                string authority = string.Empty;
                string Status = string.Empty;
                if (HttpContext.Request.QueryString["Authority"] != "")
                {
                    authority = HttpContext.Request.QueryString["Authority"];
                    Status = HttpContext.Request.QueryString["Status"];
                }
                var oFactorCement = UnitOfWork.FactorCementRepository.GetByAuthority(authority).FirstOrDefault();

                if (oFactorCement.FinalApprove) /// اگر دفعه دوم وارد این صفحه شد تایید نهایی بوده و وارد این بخش شود
                {
                    cementViewModel = ConvertCementViewModel(oFactorCement);
                    return View(cementViewModel);
                }

                parameters.authority = oFactorCement.Authority;
                if (oFactorCement.MahalTahvil == "Karkhane")
                {
                    var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                    parameters.amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                }
                else if (oFactorCement.MahalTahvil == "Mahal")
                {
                    parameters.amount = oFactorCement.DestinationAmountPaid.ToString();
                }
                parameters.merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";


                //if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                //{
                //    parameters.amount = testamount;
                //}

                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();
                    string card_pan = jodata["data"]["card_pan"].ToString();
                    string code = jodata["data"]["code"].ToString();

                    oFactorCement.ref_id = Convert.ToInt64(refid);
                    oFactorCement.card_pan = card_pan;
                    oFactorCement.Bankcode = Convert.ToInt32(code);
                    oFactorCement.AmountPaidDate = DateTime.Now;
                    oFactorCement.Address = cementViewModel.Address;
                    UnitOfWork.FactorCementRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    ViewBag.code = refid;
                    if (Status == "OK") /// اگر با موفقییت پرداخت شده بود
                    {
                        oFactorCement.FinalApprove = true;
                        var InventoryTonnage = UnitOfWork.InventoryamountRepository.Get()
                        .Where(x => x.IsDeleted == false && x.IsActived != false)
                        .Where(x => x.ProductNameId == oFactorCement.ProductNameId)
                        .Where(x => x.ProductTypeId == oFactorCement.ProductTypeId)
                        .Where(x => x.PackageTypeId == oFactorCement.PackageTypeId)
                        .Where(x => x.FactoryNameId == oFactorCement.FactoryNameId)
                        .FirstOrDefault();

                        var Tonnage = oFactorCement.Tonnagedouble;

                        InventoryTonnage.Inventorytonnage = InventoryTonnage.Inventorytonnage - Tonnage;
                        UnitOfWork.InventoryamountRepository.Update(InventoryTonnage);
                        UnitOfWork.Save();

                        var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                        if (user.IsMarketer != true && user.ReferredByCode != null)
                        {
                            var Marketer = UnitOfWork.UserRepository.GetUserByMarketingCode(user.ReferredByCode);
                            if (Marketer != null)
                            {
                                Marketer.creditAmount = (long)((oFactorCement.AmountPaid/10) * 0.005);
                            }
                        }
                        //string amount = Convert.ToString(oFactorCement.AmountPaid - user.creditAmount);
                        //user.AmountOfTonnagePurchased = int.Parse(oFactorCement.Tonnage.Code);
                        //user.creditAmount += (user.AmountOfTonnagePurchased / 50) * 10000000;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                        string mobile = oFactorCement.BuyerMobile;
                        PaymentSMS(mobile, oFactorCement);
                        cementViewModel = ConvertCementViewModel(oFactorCement);
                    }
                }
                else if (errors != "[]")
                {
                    string errorscode = jo["errors"]["code"].ToString();
                    TempData["NotFoundMessage"] = "خطا در پرداخت، کد خطا: " + errorscode;
                    //return BadRequest($"error code {errorscode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cementViewModel);
        }

        public static ViewModels.Areas.Administrator.Cement.CementViewModel ConvertCementViewModel(Models.FactorCement oFactorCement)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.Id = oFactorCement.Id;
            cementViewModel.InvoiceNumber = oFactorCement.InvoiceNumber;
            cementViewModel.StringInsertDateTime = new Infrastructure.Calander(oFactorCement.InsertDateTime).Persion();
            cementViewModel.BuyerMobile = oFactorCement.BuyerMobile;
            cementViewModel.BuyerName = oFactorCement.User.FullName;
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
            var oFactorCement = UnitOfWork.walletFactorRepository.GetByinvoicenumber(invoiceNumber).FirstOrDefault();
            var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
            string authority;
            string description = "شارژ کیف پول" + "-" + user.UserName;
            string callbackurl = "https://masalehpakhsh.com/Zarinpal/VerifyPaymentWallet";
            string merchant = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";
            string mobile = oFactorCement.BuyerMobile;
        //    PaymentSMSWallet(mobile, oFactorCement);
            if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
            {
                callbackurl = "http://localhost:6066/Zarinpal/VerifyPaymentWallet";
            }

            ViewModels.Areas.Administrator.ZarinPal.RequestParameters Parameters = new ViewModels.Areas.Administrator.ZarinPal.RequestParameters(merchant, Chargeamount.ToString(), description, callbackurl, user.BuyerMobile, "ali_animax@yahoo.com");

            var client = new RestClient(URLs.requestUrl);

            Method method = Method.Post;

            var request = new RestRequest("", method);

            request.AddHeader("accept", "application/json");

            request.AddHeader("content-type", "application/json");

            request.AddJsonBody(Parameters);

            var requestresponse = client.ExecuteAsync(request);

            JObject jo = JObject.Parse(requestresponse.Result.Content); /// "authority": "A00000000000000000000000000433573885"

            string errorscode = jo["errors"].ToString();

            JObject jodata = JObject.Parse(requestresponse.Result.Content);

            string dataauth = jodata["data"].ToString();


            if (dataauth != "[]")
            {


                authority = jodata["data"]["authority"].ToString();

                oFactorCement.Authority = authority;
                UnitOfWork.walletFactorRepository.Update(oFactorCement);
                UnitOfWork.Save();

                string gatewayUrl = URLs.gateWayUrl + authority;

                return Redirect(gatewayUrl);

            }
            else
            {
                return null;
            }

        }

        public virtual ActionResult VerifyPaymentWallet()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            try
            {
                VerifyParameters parameters = new VerifyParameters();
                string authority = string.Empty;
                string Status = string.Empty;
                if (HttpContext.Request.QueryString["Authority"] != "")
                {
                    authority = HttpContext.Request.QueryString["Authority"];
                    Status = HttpContext.Request.QueryString["Status"];
                }
                var oFactorCement = UnitOfWork.walletFactorRepository.GetByAuthority(authority).FirstOrDefault();

                if (oFactorCement.FinalApprove) /// اگر دفعه دوم وارد این صفحه شد تایید نهایی بوده و وارد این بخش شود
                {
                    cementViewModel = ConvertCementViewModel(oFactorCement);
                    return View(cementViewModel);
                }

                parameters.authority = oFactorCement.Authority;
                parameters.merchant_id = "d9c07ec3-6934-41f3-b6d4-a7eecedf3114";

                if (System.Diagnostics.Debugger.IsAttached) //برای اینکه در لوکال اجرا شود
                {
                    parameters.amount = oFactorCement.Chargeamount.ToString();
                }
                else
                {
                    parameters.amount = oFactorCement.Chargeamount.ToString();
                }

                var client = new RestClient(URLs.verifyUrl);
                Method method = Method.Post;
                var request = new RestRequest("", method);

                request.AddHeader("accept", "application/json");

                request.AddHeader("content-type", "application/json");
                request.AddJsonBody(parameters);

                var response = client.ExecuteAsync(request);


                JObject jodata = JObject.Parse(response.Result.Content);

                string data = jodata["data"].ToString();

                JObject jo = JObject.Parse(response.Result.Content);

                string errors = jo["errors"].ToString();

                if (data != "[]")
                {
                    string refid = jodata["data"]["ref_id"].ToString();
                    string card_pan = jodata["data"]["card_pan"].ToString();
                    string code = jodata["data"]["code"].ToString();

                    oFactorCement.ref_id = Convert.ToInt64(refid);
                    oFactorCement.card_pan = card_pan;
                    oFactorCement.Bankcode = Convert.ToInt32(code);
                    oFactorCement.AmountPaidDate = DateTime.Now;
                    UnitOfWork.walletFactorRepository.Update(oFactorCement);
                    UnitOfWork.Save();

                    ViewBag.code = refid;
                    if (Status == "OK") /// اگر با موفقییت پرداخت شده بود
                    {
                        oFactorCement.FinalApprove = true;
                        UnitOfWork.walletFactorRepository.Update(oFactorCement);
                        var user = UnitOfWork.UserRepository.GetById(oFactorCement.UserId);
                        user.creditAmount += oFactorCement.Chargeamount;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                        string mobile = oFactorCement.BuyerMobile;
                        PaymentSMSWallet(mobile, oFactorCement);
                        cementViewModel = ConvertCementViewModel(oFactorCement);
                    }
                }
                else if (errors != "[]")
                {
                    string errorscode = jo["errors"]["code"].ToString();
                    TempData["NotFoundMessage"] = "خطا در پرداخت، کد خطا: " + errorscode;
                    //return BadRequest($"error code {errorscode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cementViewModel);
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
                const string username = "989926932699";
                const string password = "#57PD";
                const int bodyId = 284290;
                string to = user?.BuyerMobile?.ToString() ?? phoneNumber;

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
                        accountStatus = "طلبکار";
                        balanceDetails = $"{Math.Abs(balanceAmount).ToString("N0")}";
                    }
                    else
                    {
                        accountStatus = "";
                    }
                }



                string[] text = user != null && user.isSendSms
                    ? new string[] {
                user.UserName,
                factor.Tonnagedouble != null 
                    ? factor.Tonnagedouble.ToString().Replace('.', '/') + " تن"
                    : "N/A",
                factor.PackageType?.Name ?? "N/A",
                factor.FactoryName?.Name ?? "N/A",
                factor.ProductType?.Name ?? "N/A",
                factor.AmountPaid.ToString("N0"),
                balanceDetails,
                accountStatus
                    }
                    : new string[] {
                "ثبت نام نشده",
                factor.Tonnagedouble != null 
                    ? factor.Tonnagedouble.ToString().Replace('.' , '/') + " تن"
                    : "N/A",
                factor.PackageType?.Name ?? "N/A",
                factor.FactoryName?.Name ?? "N/A",
                factor.ProductType?.Name ?? "N/A",
                factor.AmountPaid.ToString("N0"),
                "0",
                ""
                    };

                var binding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = BasicHttpSecurityMode.Transport
                    }
                };

                var endpoint = new EndpointAddress("https://api.payamak-panel.com/post/Send.asmx");
                var soapClient = new MelipayamakService.SendSoapClient(binding, endpoint);
                var result = soapClient.SendByBaseNumber(username, password, text, to, bodyId);

                // بازگرداندن نتیجه به صورت جاوا اسکریپت
                return Content($"<script>console.log('پیامک با موفقیت ارسال شد. نتیجه: {result}');</script>", "text/html");
            }
            catch (Exception ex)
            {
                // بازگرداندن خطا به صورت جاوا اسکریپت
                return Content($"<script>console.error('خطا در ارسال پیامک: {ex.Message}');</script>", "text/html");
            }
        }


        public void PaymentSMSWallet(string phoneNumber, Models.walletFactor factor)
        {
            try
            {
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
                var result = soapClient.SendSimpleSMS(username, password,phoneNumbers,from,text,isFlash);

                // بازگرداندن نتیجه به صورت جاوا اسکریپت
                return true;
            }
            catch (Exception ex)
            {
                // بازگرداندن خطا به صورت جاوا اسکریپت
                return false;
            }
        }
    }
}