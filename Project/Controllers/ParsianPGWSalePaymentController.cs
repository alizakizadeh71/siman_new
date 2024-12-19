//using PecPGW.Samples.WebApp.MVC.ParsianPGWSalePaymentServices;
//using PecPGW.Samples.WebApp.MVC.Models;
using Infrastructure;
using log4net;
using OPS.ir.shaparak.sadad;
//using PecPGW.Samples.WebApp.MVC.Models.SalePayment;
//using PecPGW.Samples.Common;
using OPS.Parsian;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public partial class ParsianPGWSalePaymentController : Controller
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();

        private readonly static ILog logger = log4net.LogManager.GetLogger(typeof(ParsianPGWSalePaymentController));
        private const string SaleLoginAccountSessionKey = "SaleLoginAccount";
        private const string ConfirmAfterPaymentSessionKey = "ConfirmAfterPayment";
        private const string ConfirmServiceUrlSessionKey = "ConfirmServiceUrl";
        private const string ReversalServiceUrlSessionKey = "ReversalServiceUrl";

        // GET: ParsianPGWSalePayment
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult SalePayment(string saleType)
        //{
        //    SalePaymentRequestModel model;
        //    if (saleType == "sale")
        //    {
        //        model = new SalePaymentRequestModel();
        //    }
        //    else
        //    {
        //        model = new GovermentSalePaymentRequestModel();
        //    }

        //    if (ViewData.ContainsKey("saleType"))
        //        ViewData.Remove("saleType");
        //    ModelState.Remove("saleType");
        //    return PartialView("_SalePayment", model);
        //}

        private string GetLoginAccount()
        {
            return Session[SaleLoginAccountSessionKey].ToString();
        }

        ////این اکشن برای پرداخت در عملیات خرید استفاده می شود
        ////به نمونه کد فراخوانی سرویس پرداخت قبض درگاه پرداخت اینترنتی پارسیان در کنترلر قبلی مراجعه شود
        //[HttpPost]
        //public ActionResult SalePayment(SalePaymentRequestModel model)
        //{
        //    if (false == ModelState.IsValid)
        //        return View(model);

        //    long token = 0;
        //    short paymentStatus = Int16.MinValue;
        //    ClientPaymentResponseDataBase responseData = null;
        //    using (var service = new ParsianPGWSalePaymentServices.SaleService())
        //    {
        //        //it is not recommended to bypass Server Certificate Validation, due to vaiolating security concerns.
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((o, xc, xch, sslP) => true);


        //        //set Service Url from your application configuration
        //        service.Url = ConfigHelper.ParsianPGWSaleServiceUrl;

        //        //instantiate a new instance of the class containing request parameters
        //        var saleRequest = new ParsianPGWSalePaymentServices.ClientSaleRequestData();

        //        string loginAccount = model.LoginAccount;
        //        if (string.IsNullOrWhiteSpace(loginAccount))
        //        {
        //            loginAccount = model.LoginAccount;
        //        }

        //        if (string.IsNullOrWhiteSpace(loginAccount))
        //        {
        //            ModelState.AddModelError("LoginAccount", "please fill either Login Account or customLoginAccount");
        //            return View();
        //        }

        //        Session[ConfirmAfterPaymentSessionKey] = model.ConfirmAfterPayment;
        //        Session[ConfirmServiceUrlSessionKey] = ConfigHelper.ParsianPGWConfirmServiceUrl;
        //        Session[ReversalServiceUrlSessionKey] = ConfigHelper.ReversalServiceUrl;

        //        saleRequest.LoginAccount = loginAccount;

        //        //save LoginAccount into Session, to use in Confirm or Reversal.
        //        //Please don't do this in your application, this is only for this Sample!
        //        Session[SaleLoginAccountSessionKey] = loginAccount;


        //        //make sure you set the CallBackUrl property. because after user has completed Payment on IPG page, it will be redirected to the callback url you provided
        //        //to get you back result of the user Payment on IPG.
        //        saleRequest.CallBackUrl = ConfigHelper.SalePaymentCallback;

        //        //Amount is not used. you should not assign a value to this property.
        //        saleRequest.Amount = model.Amount;

        //        //AdditionalData will be saved in Parsian PGW
        //        saleRequest.AdditionalData = model.AdditionalData;

        //        //Mobile Number, Telephone or any property of your user to indicate he/she is ordered this payment request.
        //        saleRequest.Originator = model.Originator;

        //        //Order Id MUST be UNIQUE at all times. if a duplicated Order Id is received from your request, you will get Status= -112
        //        // DateTime.Now.Ticks does not ensures uniqueness of OrderId. consider generate OrderId by a Sequence or Identity field in your database.
        //        saleRequest.OrderId = DateTime.Now.Ticks;

        //        //it is recommended to save Request before calling service in your application's Data Store.
        //        //e.g. DataStore.DoSaveRequest(loginAccount, OrderId, amount, addData, originator)


        //        //call appropriate api based on the documentation provided to you.
        //        responseData = service.SalePaymentRequest(saleRequest);

        //        //update response of api call for the request you've saved before.
        //        //e.g. DataStore.UpdatePaymentRequestResponse(orderId, pgwtoken, pgwStatus).

        //        paymentStatus = responseData.Status;

        //        //check Status property of the response object to see if the operation was successful.
        //        if (responseData.Status == Constants.ParsianPaymentGateway.Successful)
        //        {
        //            //if everything is OK (LoginAccount and your IP address is valid in Parsian PGW), save the token in a data store
        //            // to use it for redirectgion of your web site's user to the Parsian IPG (Internet Payment Gateway) page to complete payment.
        //            token = responseData.Token;

        //            //you must save the token in a data store for subsequent support and api calls.
        //            Session["Token"] = token;
        //        }
        //        else
        //        {
        //            logger.Error($"Parsian PGW service call status code : {responseData.Status}");
        //        }
        //    }

        //    //if pgwStatus == 0 and pgwToken > 0, redirect user to the Parsian IPG to continue payment flow.
        //    if (paymentStatus == Constants.ParsianPaymentGateway.Successful && token > 0L)
        //    {
        //        //first, save token to your database to be able to track payment process with your business.
        //        //after successfully retrieved a token from Parsian PGW, redirect user to Parsian IPG to complete the payment operation.

        //        var redirectUrl = string.Format(ConfigHelper.ParsianIPGPageUrl, responseData.Token);
        //        return Json(new
        //        {
        //            status = 0,
        //            location = redirectUrl
        //        });
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Error connecting to pay service";
        //        var mdl = new PaymentRequestResponseModel()
        //        {
        //            Message = responseData?.Message,
        //            Status = responseData?.Status,
        //            Token = responseData?.Token
        //        };
        //        return View("_PaymentRequestResults", mdl);
        //    }
        //}

        //[HttpPost]
        //public ActionResult GovermentSalePayment(GovermentSalePaymentRequestModel model)
        //{
        //    if (false == ModelState.IsValid)
        //        return View(model);

        //    long token = 0;
        //    short paymentStatus = Int16.MinValue;
        //    GovermentSaleServices.ClientPaymentResponseDataBase responseData = null;
        //    using (var service = new GovermentSaleServices.GovermentIdSaleServiceSw2())
        //    {
        //        //it is not recommended to bypass Server Certificate Validation, due to vaiolating security concerns.
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((o, xc, xch, sslP) => true);

        //        service.Url = ConfigHelper.ParsianPGWGovermentIdSaleServiceUrl;
        //        var saleRequest = new GovermentSaleServices.ClientSaleRequestData();

        //        string loginAccount = model.LoginAccount;
        //        if (string.IsNullOrWhiteSpace(loginAccount))
        //        {
        //            loginAccount = model.LoginAccount;
        //        }

        //        if (string.IsNullOrWhiteSpace(loginAccount))
        //        {
        //            ModelState.AddModelError("LoginAccount", "please fill either Login Account or customLoginAccount");
        //            return View();
        //        }

        //        saleRequest.LoginAccount = loginAccount;
        //        Session[SaleLoginAccountSessionKey] = loginAccount;

        //        //make sure you set the CallBackUrl property. because after user has completed Payment on IPG page, it will be redirected to the callback url you provided
        //        //to get you back result of the user Payment on IPG.
        //        saleRequest.CallBackUrl = ConfigHelper.SalePaymentCallback;

        //        //Amount is not used. you should not assign a value to this property.
        //        saleRequest.Amount = model.Amount;

        //        saleRequest.AdditionalData = $"GOVId={model.AdditionalData}";

        //        saleRequest.Originator = model.Originator;

        //        //Order Id MUST be UNIQUE at all times. if a duplicated Order Id is received from your request, you will get Status=-112
        //        saleRequest.OrderId = DateTime.Now.Ticks;

        //        Session[ConfirmAfterPaymentSessionKey] = model.ConfirmAfterPayment;
        //        Session[ConfirmServiceUrlSessionKey] = ConfigHelper.ParsianPGWConfirmServiceSW2Url;
        //        Session[ReversalServiceUrlSessionKey] = ConfigHelper.ReversalServiceSW2Url;

        //        //save request into your application data store
        //        responseData = service.SalePaymentRequest(saleRequest);
        //        //update response for previously saved request


        //        paymentStatus = responseData.Status;

        //        //check Status property of the response object to see if the operation was successful.
        //        if (responseData.Status == Constants.ParsianPaymentGateway.Successful)
        //        {
        //            //if everything is OK (LoginAccount and your IP address is valid in the Parsian PGW), save the token in a data store
        //            // to use it for redirectgion of your web site's user to the Parsian IPG (Internet Payment Gateway) page to complete payment.
        //            token = responseData.Token;

        //            //you must save the token in a data store for further support and resolving 
        //            Session["Token"] = token;
        //        }
        //        else
        //        {
        //            logger.Error($"Parsian PGW service call status code : {responseData.Status}");
        //        }
        //    }

        //    if (paymentStatus == Constants.ParsianPaymentGateway.Successful && token > 0L)
        //    {
        //        //first, save token to your database to be able to track payment process with your business.
        //        //after successfully retrieved a token from Parsian PGW, redirect user to Parsian IPG to complete the payment operation.
        //        //var redirectUrl = string.Format(ConfigHelper.ParsianIPGPageUrl, token);
        //        var redirectUrl = string.Format(ConfigHelper.ParsianIPGPageSW2Url, responseData.Token);
        //        return Json(new
        //        {
        //            status = 0,
        //            location = redirectUrl
        //        });
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Error conecting to pay service";
        //        var mdl = new PaymentRequestResponseModel()
        //        {
        //            Message = responseData?.Message,
        //            Status = responseData?.Status,
        //            Token = responseData?.Token
        //        };
        //        return View("_PaymentRequestResults", mdl);
        //    }
        //}
        /// <summary>
        /// After user in Parsian IPG has completed payment process, it will be redirected to your CallBackUrl specified in initial Payment Request method call.
        /// You MUST insure of user's payment process status. if you havn't received a callback for payment, you should not deliver requested business product or service to your user.
        /// </summary>
        /// <param name="model">This is the model that Parsian IPg will fill for you after user has completed Payment process on Parsian IPG.
        /// to inform you of results of payment.</param>
        /// <returns></returns>
        /// پس از فراخوانی این اکشن توسط درگاه پرداخت اینترنتی پارسیان، 
        /// //متوجه وضعیت انجام فرآیند پرداخت توسط کاربر در صفحه درگاه پرداخت اینترنتی پارسیان خواهید شد
        /// و در صورت موفق بودن انجام پرداخت، باید تایید پرداخت را به درگاه پرداخت اینترنتی پارسیان اعلام نمایید
        /// //تا تراکنش موفق تلقی شود
        /// درصورتی که سرویس تایید پرداخت فراخوانی نشود، پولی که کاربر در صفحه درگاه پرداخت از کارت خود پرداخت نموده، 
        /// به حساب او برگشت داده می شود و خدماتی از سوی شما به او نباید ارائه شود
        /// بنابراین حتماً به خاطر داشته باشد که در کال بک، باید در صورتی که کد وضعیت تراکنش پرداخت موفق اعلام شد
        /// سرویس تایید تراکنش را با فراهم نمودن پارامتر مطلوب
        /// فراخوانی نمایید
        /// و سپس در صورت موفق بودن تایید تراکنش، نسبت به  ارائه خدمات به کاربر وب سایت خود اقدام نمایید
        /// بنابراین اگر پرداخت در صفحه پرداخت توسطک اربر موفق باشد
        /// اما تایید پرداخت را ارسال ننمایید
        /// خدماتی به کاربر نیز نباید بدهید
        /// اما درصورتی که تایید پرداخت را انجام دادید
        /// و تایید پرداخت موفق بود
        /// حتما باید خدمات را به کاربر ارائه دهید
        [HttpPost]
        public virtual ActionResult PaymentCallback(PaymentCallbackModel model)
        {
            Nullable<int> ErrorCount = null;
            try
            {
                //if (model != null)
                //{
                Calander calander = new Infrastructure.Calander();
                var nowshamsiDate = calander.Miladi_To_Shamsi(DateTime.Now).Split('/');
                var nowshamsiDate2 = nowshamsiDate[0] + nowshamsiDate[1] + nowshamsiDate[2];
                logger.Info(new
                {
                    Message = "Callbacked!",
                    Model =
                    model
                });

                ErrorCount = 1;
                //save callback info to your database and relate those info to the user's requested service or product.
                //if needed, deliver product or service to the user after ensuring payment.
                //in the case of Bill Payment, no delivery is commonly required.
                //but if a Sale payment is performed, may be a product or service delivery will be needed.
                //if transaction by user is successful, call Confirm service from Parsian PGW to complete the transaction
                //and indicate that you will deliver service or product to your end user.
                //it is better to save callback results into a database.

                var callBackViewModel = new PaymentCallBackViewModel()
                {
                    Amount = model.Amount,
                    HashCardNumber = model.HashCardNumber,
                    OrderId = model.OrderId,
                    RRN = model.RRN,
                    status = model.status,
                    TerminalNo = model.TerminalNo,
                    Token = model.Token,
                    TspToken = model.TspToken
                };

                // برای جلو گیری از ایجاد OrderId تکراری
                //var OrderId2 = Convert.ToString(model.OrderId);
                //var OrderId = Convert.ToInt64(OrderId2.Substring(0, 6));
                //var NewOrderId = Convert.ToInt64(OrderId2.Substring(0, 7));
                var OrderId = model.OrderId.ToString();
                if (OrderId.Length == 11)
                    OrderId = OrderId.Substring(0, 5);
                if (OrderId.Length == 12)
                    OrderId = OrderId.Substring(0, 6);
                if (OrderId.Length == 13)
                    OrderId = OrderId.Substring(0, 7);
                if (OrderId.Length == 14)
                    OrderId = OrderId.Substring(0, 8);
                if (OrderId.Length == 15)
                    OrderId = OrderId.Substring(0, 9);
                var NewOrderId = Convert.ToInt64(OrderId);


                Models.Request oRequestNew = null;
                Models.Request oRequest = null;
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                ErrorCount = 2;

                if (model.status == Constants.ParsianPaymentGateway.Successful && (model.Token ?? 0L) > 0L && model.RRN > 0)
                {
                    ErrorCount = 25;

                    //      var shouldConfirmAfterPay = (bool)Session[ConfirmAfterPaymentSessionKey];
                    ErrorCount = 26;

                    //if (shouldConfirmAfterPay)
                    //{
                    ErrorCount = 27;

                    //ایجاد یک نمونه از سرویس تایید پرداخت
                    using (var confirmSvc = new PasrianPGWConfirmServices.ConfirmServiceSoapClient())
                    {
                        ErrorCount = 28;

                        //confirmSvc.Url = (string)Session[ConfirmServiceUrlSessionKey];
                        //ایجاد یک نمونه از نوع پارامتر سرویس تایید پرداخت
                        var confirmRequestData = new PasrianPGWConfirmServices.ClientConfirmRequestData();
                        ErrorCount = 3;

                        //شناسه پذیرندگی باید در فراخوانی سرویس تایید تراکنش پرداخت ارائه شود
                        confirmRequestData.LoginAccount = "Twl2kPsw8mesE6h1SfJE";

                        //توکن باید ارائه شود
                        confirmRequestData.Token = model.Token ?? -1;

                        //فراخوانی سرویس و دریافت نتیجه فراخوانی
                        var confirmResponse = confirmSvc.ConfirmPayment(confirmRequestData);
                        callBackViewModel.ConfirmResponseStatus = confirmResponse.Status;


                        //oRequest =
                        //    UnitOfWork.RequestRepository.Get()
                        //   .Where(current => current.InvoiceNumber == OrderId)
                        //   .FirstOrDefault()
                        //   ;
                        //if (oRequest == null)
                        //{
                        oRequest =
                            UnitOfWork.RequestRepository.Get()
                           .Where(current => current.InvoiceNumber == NewOrderId)
                           .FirstOrDefault()
                           ;
                        ErrorCount = 4;

                        //}
                        var oAccountNumber =
                             UnitOfWork.AccountNumberManageRepository.Get()
                            .Where(current => current.ProvinceId == oRequest.ProvinceId)
                            .Where(current => current.SubSystemId == oRequest.SubSystemId)
                             .FirstOrDefault()
                             .AccountNumber
                              ;

                        oRequest.AmountPaidDate = DateTime.Now;
                        oRequest.Bank_AppStatus = confirmResponse.Status;

                        oRequest.Bank_BankReciptNumber = nowshamsiDate2 + model.RRN;
                        oRequest.Bank_CardHolderAccNumber = confirmResponse.CardNumberMasked;
                        oRequest.Bank_CardHolderName = "";
                        oRequest.Bank_CustomerCardNumber = confirmResponse.CardNumberMasked;

                        oRequest.Bank_NewlyCommitted = false;
                        oRequest.Bank_RealTransactionDateTime = DateTime.Now;
                        oRequest.Bank_RefrenceNumber = model.RRN.ToString();
                        ErrorCount = 5;

                        oRequest.Bank_ShamsiDate = nowshamsiDate2;
                        oRequest.Bank_TraceNo = model.OrderId;
                        oRequest.Bank_Terminal = model.TerminalNo.ToString();
                        oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                        oRequest.UpdateDateTime = DateTime.Now;
                        oRequest.Bank_RequestKey = model.Token.ToString();


                        #region Insert PaymentMessage
                        Models.Message oMessageP = new Models.Message();
                        oMessageP.UserId = oRequest.UserId;
                        oMessageP.LastState = oRequest.RequestState;
                        oMessageP.MessageText = Resources.Message.Request.Message_Paymented;
                        oMessageP.NewState = (int)Enums.RequestStates.Payment;
                        oMessageP.RequestId = oRequest.Id;
                        ErrorCount = 6;

                        UnitOfWork.MessageRepository.Insert(oMessageP);
                        #endregion

                        //if (oRequest.RequestState > (int)Enums.RequestStates.Payment)
                        //{
                        //    #region Insert Payment Confirm Message
                        //    Models.Message oMessage = new Models.Message();
                        //    oMessage.UserId = oRequest.UserId;
                        //    oMessage.LastState = (int)Enums.RequestStates.Payment;
                        //    oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation;
                        //    oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                        //    oMessage.RequestId = oRequest.Id;
                        //    UnitOfWork.MessageRepository.Insert(oMessage);
                        //    #endregion
                        //}
                        //else
                        //{
                        //    #region Insert Not Payment Message
                        //    Models.Message oMessage = new Models.Message();
                        //    oMessage.UserId = oRequest.UserId;
                        //    oMessage.LastState = oRequest.RequestState;
                        //    oMessage.MessageText = Resources.Message.Request.Message_NoPaymented;
                        //    oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                        //    oMessage.RequestId = oRequest.Id;
                        //    UnitOfWork.MessageRepository.Insert(oMessage);
                        //    #endregion

                        //    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                        //}

                        string LogValue = string.Empty;
                        LogValue += Environment.NewLine;
                        LogValue += "AmountPaidDate" + oRequest.AmountPaidDate;
                        LogValue += "*********************************";
                        LogValue += Environment.NewLine;
                        LogValue += "Bank_RealTransactionDateTime" + oRequest.Bank_RealTransactionDateTime;
                        LogValue += "*********************************";
                        LogValue += Environment.NewLine;
                        LogValue += "InsertDateTime" + oRequest.InsertDateTime;
                        LogValue += "*********************************";
                        LogValue += Environment.NewLine;
                        LogValue += "InvoiceDate" + oRequest.InvoiceDate;
                        LogValue += "*********************************";
                        LogValue += Environment.NewLine;
                        LogValue += "UpdateDateTime" + oRequest.UpdateDateTime;
                        LogValue += "*********************************";
                        Utilities.Net.LogHandler.LogToFile(LogValue);

                        //UnitOfWork.RequestRepository.Update(oRequest);
                        //UnitOfWork.Save();
                        //oRequestNew =
                        //    UnitOfWork.RequestRepository.Get()
                        //    .Where(current => current.InvoiceNumber == OrderId)
                        //    .FirstOrDefault();

                        //کنترل کد وضعیت نتیجه فراخوانی
                        //درصورتی که موفق باشد، باید خدمات یا کالا به کاربر پرداخت کننده ارائه شود
                        if (confirmResponse.Status == Constants.ParsianPaymentGateway.Successful)
                        {
                            ErrorCount = 7;

                            oRequest.Bank_AppStatusCode = 0;
                            oRequest.Bank_AppStatusDescription = "SUCCESS";

                            if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode == (int)Enums.CurrencyUnits.Rails)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                                oRequest.RequestState = (int)Enums.RequestStates.Payment;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Fob && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                                oRequest.RequestState = (int)Enums.RequestStates.Payment;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                                oRequest.RequestState = (int)Enums.RequestStates.Payment;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Import)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Clearance)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Export)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Internal)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Quarantine_Transit)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.InvoiceKhadamatDampezeshki)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.InvoiceSharei)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23)
                                oRequest.RequestState = (int)Enums.RequestStates.Payment;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Certificate)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Registration)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Lims)
                                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                            oRequest.Bank_FailCode = 0;
                            oRequest.Bank_ResponseCode = "0";
                            //UnitOfWork.RequestRepository.Update(oRequest);
                            UnitOfWork.Save();

                            //oRequestNew =
                            //    UnitOfWork.RequestRepository.Get()
                            //    .Where(current => current.InvoiceNumber == OrderId)
                            //    .FirstOrDefault();
                            //if (oRequestNew == null)
                            //{
                            ErrorCount = 8;

                            oRequestNew =
                                UnitOfWork.RequestRepository.Get()
                                .Where(current => current.InvoiceNumber == NewOrderId)
                                .FirstOrDefault();
                            //}

                            return View("~/Views/Payment/MerchantCommitByInvoiceNumber.cshtml", oRequestNew);
                        }
                        //در صورتی که موفق نباشد
                        else if (confirmResponse.Status == -138)
                        {
                            ErrorCount = 9;

                            oRequest.Bank_AppStatusCode = -138;
                            oRequest.Bank_AppStatusDescription = "FAIL";
                            oRequest.Bank_FailCode = 1;
                            oRequest.Bank_ResponseCode = "-138";
                            UnitOfWork.RequestRepository.Update(oRequest);
                            UnitOfWork.Save();
                            return View("~/Views/Payment/MerchantCommitByInvoiceNumber.cshtml", oRequestNew);
                            ////ایجاد یک نمونه از نوع پارامتر سرویس برگشت
                            //var reversalRequestData = new ParsianPGWReversalServices.ClientReversalRequestData();

                            ////شناسه پذیرندگی باید در فراخوانی سرویس تایید تراکنش پرداخت ارائه شود
                            //confirmRequestData.LoginAccount = "Twl2kPsw8mesE6h1SfJE";

                            ////توکن باید ارائه شود
                            //confirmRequestData.Token = model.Token ?? -1;

                            ////فراخوانی سرویس و دریافت نتیجه فراخوانی
                            //var confirmResponse2 = confirmSvc.ConfirmPayment(confirmRequestData);
                            //callBackViewModel.ConfirmResponseStatus = confirmResponse2.Status;
                        }
                    }
                    //}
                }
                ErrorCount = 10;

                //oRequest.Bank_AppStatusCode = -139;
                //oRequest.Bank_AppStatusDescription = "FAIL2";
                //oRequest.Bank_FailCode = 2;
                //oRequest.Bank_ResponseCode = "-139";
                //UnitOfWork.RequestRepository.Update(oRequest);
                //UnitOfWork.Save();
                //oRequestNew =
                //            UnitOfWork.RequestRepository.Get()
                //            .Where(current => current.InvoiceNumber == OrderId)
                //            .FirstOrDefault();
                //ErrorCount = 11;

                //if (oRequestNew == null)
                //{
                oRequestNew =
            UnitOfWork.RequestRepository.Get()
            .Where(current => current.InvoiceNumber == NewOrderId)
            .FirstOrDefault();
                //}


                return Redirect("https://ops.ivo.ir/");
            }
            catch (Exception ex)
            {
                Session["NotFoundMessage1"] = ErrorCount.ToString();
                return RedirectToAction("Index", "HomeMain");
            }

        }

    }

    //در صورتی که لازم باشد پرداختی که کاربر در صفحه درگاه پرداخت اینترنتی پارسیان انجام داده است
    //به شکلی لغو شود، باید از وب سرویس برگشت وجه استفاده شود
    //این وب سرویس با پاس نمودن توکن درخواستی که پرداخت آن انجام شده است قابل فراخوانی است
    //لطفاً دقت فرمایید که تنها در صورتی باید این سرویس فراخوانی شود که 
    //به عنوان مثال، کاربر درخواست لغو دریافت کالا یا خدمات از وب سایت شما را پس از پرداخت موفق داده باشد
    //فقط بیست دقیقه پس از درخواست اولیه پرداخت که توسط اپلیکیشن داده شده است، فرصت درخواست برگشت وجه پرداخت شده وجود دارد
    //[HttpPost]
    //public ActionResult ReversePayment(ReversalModel model)
    //{
    //    try
    //    {
    //        //ایجاد یک نمونه از سرویس برگشت وجه پرداخت شده
    //        using (var reversalSvc = new ParsianPGWReversalServices.ReversalService())
    //        {
    //            reversalSvc.Url = (string)Session[ReversalServiceUrlSessionKey];

    //            //ایجاد یک نمونه از نوع پارامتر ورودی متد برگشت وجه
    //            var requestData = new ParsianPGWReversalServices.ClientReversalRequestData()
    //            {
    //                LoginAccount = GetLoginAccount(),
    //                Token = model.Token
    //            };

    //            //فراخوانی متد برگشت وجه از نمونه وب سرویس برگشت وجه
    //            var response = reversalSvc.ReversalRequest(requestData);

    //            //کنترل کد وضعیت عملیات درخواست برگشت وجه
    //            //درصورت موفق بودن، می توانید از ارائه کالا و یا خدمات درخواستی کاربر به او انصراف دهید
    //            if (response.Status == Constants.ParsianPaymentGateway.Successful)
    //            {
    //                //reversal was successful
    //            }

    //            var viewModel = new ReversalResponseViewModel()
    //            {
    //                Status = response.Status,
    //                Message = response.Status == 0 ? "Successful" : "Failed!!!"
    //            };

    //            return PartialView("_ReverseResponse", viewModel);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(new { Method = "Reversal", Exception = ex });
    //        return PartialView("_ReverseResponse", new ReversalResponseViewModel()
    //        {
    //            Message = "Unsuccessful" + Environment.NewLine + ex.ToString()
    //        });
    //    }
    //}
}
