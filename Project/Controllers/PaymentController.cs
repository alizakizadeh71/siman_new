using OPS.ir.shaparak.sadad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using OPS.CBINasimService;
using Rotativa;
using OPS.Parsian;
using log4net;
using System.Text;
using System.Security.Cryptography;

namespace OPS.Controllers
{
    public partial class PaymentController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();
        private Infrastructure.cbinasimService oCBINasim = new Infrastructure.cbinasimService();
        PGServiceClient oIPGServices = new PGServiceClient();
        protected string FormBody = string.Empty;

        private readonly static ILog logger = log4net.LogManager.GetLogger(typeof(ParsianPGWSalePaymentController));
        private const string SaleLoginAccountSessionKey = "SaleLoginAccount";
        private const string ConfirmAfterPaymentSessionKey = "ConfirmAfterPayment";
        private const string ConfirmServiceUrlSessionKey = "ConfirmServiceUrl";
        private const string ReversalServiceUrlSessionKey = "ReversalServiceUrl";

        //private string UserName = "100000100";
        //private string Password = "Hh123456@";

        private string UserName = "100000178";
        private string Password = "123456@Aa";

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult Index()
        {
            return View();
        }
        //#region  GetRequest https://sadad.shaparak.ir
        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: true)]
        //public virtual ActionResult GetRequest(int id)
        //{
        //    try
        //    {
        //        DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

        //        var oRequest =
        //         UnitOfWork.RequestRepository.Get()
        //         .Where(current => current.InvoiceNumber == id)
        //         //.Where(current => current.RequestState <= (int)Enums.RequestStates.PaymentOrder)
        //         .FirstOrDefault()
        //         ;

        //        var oAccountNumber =
        //            UnitOfWork.AccountNumberManageRepository.Get()
        //            .Where(current => current.ProvinceId == oRequest.ProvinceId)
        //            .Where(current => current.SubSystemId == oRequest.SubSystemId)
        //            .FirstOrDefault()
        //            .AccountNumber
        //            ;

        //        if (oRequest == null)
        //        {
        //            return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
        //        }

        //        var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);

        //        #region Call Sadad Web Service

        //        long AmountPaid = 0;

        //        if (oRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
        //        {
        //            AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
        //            oRequest.CurrencyRation = oCurrency.Ratio;
        //            oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);

        //            UnitOfWork.RequestRepository.Update(oRequest);
        //            UnitOfWork.Save();
        //        }

        //        if (oRequest.RequestState == (int)Enums.RequestStates.PaymentOrder)
        //        {
        //            string requestKey;
        //            oMerchantUtility.Url = Infrastructure.WebServiceSetting_Sadad.ServiceURL;

        //            //if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
        //            //    AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio) + 33000;
        //            //else
        //            AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);

        //            //requestKey = "14362557679755f13aad96af853f9f1cd4e133e30a096";
        //            FormBody =
        //                oMerchantUtility.PaymentUtilityAdditionalData
        //                (oAccountNumber.MerchantId, AmountPaid, oRequest.InvoiceNumber
        //                , oAccountNumber.TranKey, oAccountNumber.Terminal
        //                , Infrastructure.WebServiceSetting_Sadad.ReturnURL_Last, string.Empty, out requestKey);

        //            FormBody += "<button type=\"submit\" class=\"col-md-offset-2 btn btn-success\""
        //            + " value='@Resources.OPS.Button.Payment'>" + Resources.OPS.Button.Payment + "</button> </form>"
        //            ;

        //            #region Save Request Key

        //            oRequest.Bank_RequestKey = requestKey;
        //            oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
        //            oRequest.Bank_Terminal = oAccountNumber.Terminal;

        //            #region Update CurrencyRation
        //            oRequest.CurrencyRation = oCurrency.Ratio;
        //            //if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
        //            //    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio) + 33000;
        //            //else
        //            oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
        //            #endregion

        //            UnitOfWork.RequestRepository.Update(oRequest);

        //            #endregion

        //            #region Insert Payment Confirm Message
        //            Models.Message oMessage = new Models.Message();
        //            oMessage.UserId = oRequest.UserId;
        //            oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
        //            oMessage.MessageText = Resources.Message.Request.Message_PaymentRequest;
        //            oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
        //            oMessage.RequestId = oRequest.Id;
        //            UnitOfWork.MessageRepository.Insert(oMessage);
        //            UnitOfWork.Save();
        //            #endregion
        //        }
        //        #endregion

        //        ViewBag.FormBody = FormBody;

        //        var retValue = UnitOfWork.RequestRepository.Get()
        //         .Where(current => current.InvoiceNumber == id)
        //         .ToList()
        //         .Select(current => new ViewModels.Areas.Administrator.Request.DisplayViewModel()
        //         {
        //             DepositNumber = current.DepositNumber,
        //             Id = current.Id,
        //             SubSystem = current.SubSystem.Name,
        //             CompanyName = current.CompanyName,
        //             CompanyNationalCode = current.CompanyNationalCode,
        //             RecordNumber = current.RecordNumber,
        //             RecordDate = current.RecordDate,
        //             InvoiceNumber = current.InvoiceNumber,
        //             InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
        //             CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
        //             CurrencyValue = current.CurrencyValue,
        //             AmountPaid = AmountPaid,
        //             RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
        //             Bank_TraceNo = current.Bank_TraceNo,
        //             Bank_ShamsiDate = current.Bank_ShamsiDate
        //         })
        //         .FirstOrDefault();

        //        return View(retValue);
        //        //}
        //    }

        //    catch (Exception ex)
        //    {
        //        Utilities.Net.LogHandler.Report(GetType(), null, ex);
        //        //return (RedirectToAction(MVC.Error.Display(ex.InnerException.Message.ToString(),0)));
        //        throw ex;
        //    }
        //}
        //#endregion

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult ShowPaymentInvoice(string NationalCode)
        {
            TempData["NotFoundMessage1"] = "";
            try
            {
                var oRequest =
                     UnitOfWork.RequestRepository.Get()
                     .Where(current => current.CompanyNationalCode == NationalCode)
                     .Where(current => current.RequestState == 3)
                     .OrderByDescending(x => x.InsertDateTime)
                     .Select(x=>x.InvoiceNumber)
                     .Take(10)
                     .ToList();
                if (oRequest.Count == 0)
                {
                    TempData["NotFoundMessage1"] = "هیچ فاکتور پرداخت شده ای برای این کدملی/شناسه ملی یافت نشد";
                    return RedirectToAction("Index", "HomeMain");
                }
                ViewBag.request = oRequest;
                return View();
            }

            catch (Exception ex)
            {
                TempData["NotFoundMessage1"] = "خطا - دقایقی بعد دوباره تلاش نمایید";
                return null;
            }
        }









        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetRequest(int id)
        {
            TempData["NotFoundMessage"] = "";
            try
            {


                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                var oRequest =
                 UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == id)
                 .Where(current => current.IsDeleted == false)
                 .Where(current => current.IsActived == true)
                 .FirstOrDefault()
                 ;

                if (oRequest == null)
                {
                    TempData["NotFoundMessage"] = "شماره فاکتور یافت نشد";
                    if (Session["NotFoundMessage1"]?.ToString() != null)
                    {
                        TempData["NotFoundMessage"] = Session["NotFoundMessage1"]?.ToString();
                    }
                    return RedirectToAction("Index", "HomeMain");
                }

                //           var oAccountNumber =
                //UnitOfWork.AccountNumberManageRepository.Get()
                //.Where(current => current.IsDeleted == false)
                //.Where(current => current.SubSystemId == oRequest.SubSystemId)
                //.FirstOrDefault()
                //.AccountNumber
                //;

                var acm = UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault();
                if (acm == null)
                {
                    return RedirectToAction("ErrorAccount", "HomeMain");
                }

                var oAccountNumber = acm.AccountNumber;
                var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);



                long AmountPaid = 0;

                if (oRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);

                    UnitOfWork.RequestRepository.Update(oRequest);
                    UnitOfWork.Save();
                }

                if (oRequest.RequestState == (int)Enums.RequestStates.PaymentOrder)
                {
                    //string requestKey;
                    //oMerchantUtility.Url = Infrastructure.WebServiceSetting_Sadad.ServiceURL;
                    oMerchantUtility.Url = Infrastructure.GovermentIdSaleServiceSW2.ServiceURL;


                    //oMerchantUtility.Url = Infrastructure.GovermentIdSaleServiceSW2.ServiceURL;
                    //if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                    //    AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio) + 33000;
                    //else

                    // مبلغ پرداخت به روز نشود - مشکل شناسه
                    //	AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    AmountPaid = oRequest.AmountPaid;




                    GovermentSaleServices.ClientPaymentResponseDataBase responseData = null;
                    short paymentStatus = Int16.MinValue;
                    long token = 0;
                    using (var service = new GovermentSaleServices.GovermentIdSaleServiceSoapClient())
                    {

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((o, xc, xch, sslP) => true);
                        //service.Url = ConfigHelper.ParsianPGWGovermentIdSaleServiceUrl;

                        var SaleRequest = new GovermentSaleServices.ClientSaleRequestData();
                        SaleRequest.LoginAccount = "Twl2kPsw8mesE6h1SfJE";
                        SaleRequest.CallBackUrl = ConfigHelper.SalePaymentCallback;
                        SaleRequest.Amount = AmountPaid;
                        SaleRequest.AdditionalData = oRequest.DepositNumber;
                        //SaleRequest.AdditionalData = $"GOVId={oRequest.DepositNumber}";
                        SaleRequest.Originator = "";
                        //	SaleRequest.OrderId = oRequest.InvoiceNumber;/* ( DateTime.Now.Ticks/10) % 1000000000;*/

                        //string InvoiceNumber = Convert.ToString(oRequest.InvoiceNumber);
                        //string Time = Convert.ToString((DateTime.Now.Ticks / 10) % 10000000);
                        //var OrderId = (String.Format("{0}{1}",InvoiceNumber,Time));
                        //SaleRequest.OrderId = Convert.ToInt64(OrderId);/* ( DateTime.Now.Ticks/10) % 1000000000;*/

                        //var OrderId = String.Format("{0}{1}", oRequest.InvoiceNumber, (DateTime.Now.Ticks / 10) % 10000000);
                        Random random = new Random();
                        int RandomNum = random.Next(100000, 999999); // 6 رقم
                        var OrderId = String.Format("{0}{1}", oRequest.InvoiceNumber, RandomNum);
                        SaleRequest.OrderId = Convert.ToInt64(OrderId);

                        Session[ConfirmAfterPaymentSessionKey] = true;
                        Session[ConfirmServiceUrlSessionKey] = ConfigHelper.ParsianPGWConfirmServiceSW2Url;
                        //Session[ReversalServiceUrlSessionKey] = ConfigHelper.ReversalServiceSW2Url;
                        Session[ReversalServiceUrlSessionKey] = "";
                        responseData = service.SalePaymentRequest(SaleRequest);
                        paymentStatus = responseData.Status;


                        //check Status property of the response object to see if the operation was successful.
                        if (responseData.Status == Constants.ParsianPaymentGateway.Successful)
                        {
                            //if everything is OK (LoginAccount and your IP address is valid in the Parsian PGW), save the token in a data store
                            // to use it for redirectgion of your web site's user to the Parsian IPG (Internet Payment Gateway) page to complete payment.
                            token = responseData.Token;

                            //you must save the token in a data store for further support and resolving 
                            Session["Token"] = token;
                        }

                        else
                        {
                            logger.Error($"Parsian PGW service call status code : {responseData.Status}");
                        }
                    }

                    if (paymentStatus == Constants.ParsianPaymentGateway.Successful && token > 0L)
                    {
                        //first, save token to your database to be able to track payment process with your business.
                        //after successfully retrieved a token from Parsian PGW, redirect user to Parsian IPG to complete the payment operation.
                        //var redirectUrl = string.Format(ConfigHelper.ParsianIPGPageUrl, token);
                        #region Save Request Key

                        oRequest.Bank_RequestKey = "1515476620903665adc610b06c8ceee8aa49fdf5d4c31";
                        oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                        oRequest.Bank_Terminal = oAccountNumber.Terminal;
                        #endregion
                        #region Update CurrencyRation
                        oRequest.CurrencyRation = oCurrency.Ratio;
                        //if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                        //    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio) + 33000;
                        //else
                        oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                        #endregion
                        var redirectUrl = string.Format(ConfigHelper.ParsianIPGPageUrl, responseData.Token);
                        //return Json(new
                        //{
                        //    status = 0,
                        //    location = redirectUrl
                        //});
                        ViewBag.redirectUrl = redirectUrl;
                        ViewBag.paymentstatus = paymentStatus;
                    }
                    else
                    {
                        ViewBag.paymentstatus = paymentStatus;
                        ViewBag.Error = "Error conecting to pay service";
                        var mdl = new PaymentRequestResponseModel()
                        {
                            Message = responseData?.Message,
                            Status = responseData?.Status,
                            Token = responseData?.Token
                        };
                        //return View("_PaymentRequestResults", mdl);
                    }

                    //var retValue1 = UnitOfWork.RequestRepository.Get()
                    //.Where(current => current.InvoiceNumber == id)
                    //.ToList()
                    //.Select(current => new ViewModels.Areas.Administrator.Request.DisplayViewModel()
                    //{
                    //    DepositNumber = current.DepositNumber,
                    //    Id = current.Id,
                    //    SubSystem = current.SubSystem.Name,
                    //    CompanyName = current.CompanyName,
                    //    CompanyNationalCode = current.CompanyNationalCode,
                    //    RecordNumber = current.RecordNumber,
                    //    RecordDate = current.RecordDate,
                    //    InvoiceNumber = current.InvoiceNumber,
                    //    InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                    //    CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                    //    CurrencyValue = current.CurrencyValue,
                    //    AmountPaid = AmountPaid,
                    //    RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                    //    Bank_TraceNo = current.Bank_TraceNo,
                    //    Bank_ShamsiDate = current.Bank_ShamsiDate
                    //})
                    //.FirstOrDefault();

                    //return View(retValue);
                    ////
                    //FormBody =
                    //    oMerchantUtility.PaymentUtilityAdditionalData
                    //    (oAccountNumber.MerchantId, AmountPaid, oRequest.InvoiceNumber
                    //    , oAccountNumber.TranKey, oAccountNumber.Terminal
                    //    , Infrastructure.WebServiceSetting_Sadad.ReturnURL_Last, string.Empty, out requestKey);

                    //FormBody += "<button onClick='parent.location = 'https://www.plus2net.com/'' type=\"submit\" class=\"col-md-offset-2 btn btn-success\""
                    //  + " value='@Resources.OPS.Button.Payment'>" + Resources.OPS.Button.Payment + "</button>"
                    //;

                    //#region Save Request Key

                    //oRequest.Bank_RequestKey = requestKey;
                    //oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                    //oRequest.Bank_Terminal = oAccountNumber.Terminal;

                    //#region Update CurrencyRation
                    //oRequest.CurrencyRation = oCurrency.Ratio;
                    ////if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                    ////    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio) + 33000;
                    ////else
                    //oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    //#endregion

                    //UnitOfWork.RequestRepository.Update(oRequest);

                    //#endregion

                    //#region Insert Payment Confirm Message
                    //Models.Message oMessage = new Models.Message();
                    //oMessage.UserId = oRequest.UserId;
                    //oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    //oMessage.MessageText = Resources.Message.Request.Message_PaymentRequest;
                    //oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
                    //oMessage.RequestId = oRequest.Id;
                    //UnitOfWork.MessageRepository.Insert(oMessage);
                    //UnitOfWork.Save();
                    //#endregion
                }
                //#endregion

                ViewBag.FormBody = FormBody;

                var retValue = UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == id)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Request.DisplayViewModel()
                 {
                     DepositNumber = current.DepositNumber,
                     Id = current.Id,
                     SubSystem = current.SubSystem.Name,
                     CompanyName = current.CompanyName,
                     CompanyNationalCode = current.CompanyNationalCode,
                     RecordNumber = current.RecordNumber,
                     RecordDate = current.RecordDate,
                     InvoiceNumber = current.InvoiceNumber,
                     InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                     CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                     CurrencyValue = current.CurrencyValue,
                     AmountPaid = AmountPaid,
                     RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                     Bank_TraceNo = current.Bank_TraceNo,
                     Bank_ShamsiDate = current.Bank_ShamsiDate,

                 })
                 .FirstOrDefault();

                return View(retValue);
                //}
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                //return (RedirectToAction(MVC.Error.Display(ex.InnerException.Message.ToString(),0)));
                throw ex;
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GlobalPayment(int id)
        {
            try
            {
                int TestId = id;
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                var oRequest =
                 UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == TestId)
                 .FirstOrDefault()
                 ;

                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);

                #region Login to IPGService

                string InitialSessionId = string.Empty;

                //string CertificateAddress = "C:\\inetpub\\OPS\\client.p12";
                //oIPGServices.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2
                //    (CertificateAddress, "changeit", X509KeyStorageFlags.MachineKeySet);

                //Infrastructure.Sessions.CBISessionId = oIPGServices.login(UserName, Password);
                #endregion


                long AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);

                if (oRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    //oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    oRequest.AmountPaid = 1000;
                    oRequest.Bank_RequestKey = Infrastructure.Sessions.CBISessionId;
                    UnitOfWork.RequestRepository.Update(oRequest);
                    UnitOfWork.Save();
                }

                //if (oRequest.RequestState == (int)Enums.RequestStates.PaymentOrder
                //    && oCBINasim.CreatedFormBody(oRequest, AmountPaid, out FormBody))
                if (oRequest.RequestState == (int)Enums.RequestStates.PaymentOrder && oCBINasim.CreatedFormBody(oRequest, 1000, out FormBody))
                {
                    #region Update CurrencyRation
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    UnitOfWork.RequestRepository.Update(oRequest);
                    #endregion

                    #region Insert Payment Confirm Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.MessageText = Resources.Message.Request.Message_PaymentRequest;
                    oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion
                }

                ViewBag.FormBody = FormBody;

                var retValue = UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == TestId)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Request.DisplayViewModel()
                 {
                     DepositNumber = current.DepositNumber,
                     Id = current.Id,
                     SubSystem = current.SubSystem.Name,
                     CompanyName = current.CompanyName,
                     CompanyNationalCode = current.CompanyNationalCode,
                     RecordNumber = current.RecordNumber,
                     RecordDate = current.RecordDate,
                     InvoiceNumber = current.InvoiceNumber,
                     InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                     CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                     CurrencyValue = current.CurrencyValue,
                     AmountPaid = AmountPaid,
                     RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                     Bank_TraceNo = current.Bank_TraceNo,
                     Bank_ShamsiDate = current.Bank_ShamsiDate
                 })
                 .FirstOrDefault();

                return View(retValue);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                throw ex;
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult VerifyTransaction()
        {
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                string ReferenceCode = HttpContext.Request.Form["ReferenceCode"].ToString();
                int InvoiceNumber = Convert.ToInt32(HttpContext.Request.Form["RequestIdentifier"]);
                string Message = HttpContext.Request.Form["Message"].ToString();
                string ResultCode = HttpContext.Request.Form["ResultCode"].ToString();

                Models.Request oRequest = UnitOfWork.RequestRepository
                    .Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault()
                    ;

                if (ResultCode.ToUpper() == "OK")
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (mender, certificate, chain, sslPolicyErrors) => true;

                    CBINasimService.PGServiceClient client = new PGServiceClient();

                    CBINasimService.wsContext oWSContext;

                    try
                    {
                        string CertificateAddress = string.Format("{0}\\{1}", Server.MapPath("~/App_Data"), "certficate.p12");

                        client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(CertificateAddress, "pap-ict.ir"
                            , X509KeyStorageFlags.MachineKeySet);

                        oWSContext = new OPS.CBINasimService.wsContext();
                        oWSContext.value = client.login("100000017", "@0ycBPLG_G*Bptsihla*");
                    }

                    catch (Exception ex)
                    {
                        Utilities.Net.LogHandler.Report(GetType(), null, ex);
                        throw ex;
                    }

                    string[] InvoiceList = new string[1];
                    InvoiceList[0] = ReferenceCode;
                    verifyResponseResult oVerifyResultList;
                    try
                    {
                        oVerifyResultList = client.verifyTransaction(oWSContext, InvoiceList)
                            .FirstOrDefault();

                        client.logout(oWSContext);
                    }

                    catch (Exception ex)
                    {
                        Utilities.Net.LogHandler.Report(GetType(), null, ex);
                        throw ex;
                    }

                    if (oVerifyResultList.verificationError.ToString() == "NO_ERROR")
                    {
                        #region Insert PaymentMessage
                        Models.Message oMessageP = new Models.Message();
                        oMessageP.UserId = oRequest.UserId;
                        oMessageP.LastState = oRequest.RequestState;
                        oMessageP.MessageText = Resources.Message.Request.Message_Paymented;
                        oMessageP.NewState = (int)Enums.RequestStates.Payment;
                        oMessageP.RequestId = oRequest.Id;
                        UnitOfWork.MessageRepository.Insert(oMessageP);
                        #endregion

                        #region Update RequestState
                        if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode == (int)Enums.CurrencyUnits.Rails)
                            oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                        else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                            oRequest.RequestState = (int)Enums.RequestStates.Payment;

                        else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                            oRequest.RequestState = (int)Enums.RequestStates.Payment;

                        else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Certificate)
                            oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;
                        
                        else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Lims)
                            oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

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

                        if (oRequest.RequestState > (int)Enums.RequestStates.Payment)
                        {
                            #region Insert Payment Confirm Message
                            Models.Message oMessage = new Models.Message();
                            oMessage.UserId = oRequest.UserId;
                            oMessage.LastState = (int)Enums.RequestStates.Payment;
                            oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation;
                            oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                            oMessage.RequestId = oRequest.Id;
                            UnitOfWork.MessageRepository.Insert(oMessage);
                            #endregion
                        }
                        #endregion

                        oRequest.Bank_BankReciptNumber = ReferenceCode;
                        oRequest.Bank_AppStatusDescription = "NO_ERROR";
                        oRequest.Bank_ShamsiDate = Infrastructure.Utility.Persion(DateTime.Now);
                        UnitOfWork.RequestRepository.Update(oRequest);
                        ViewBag.IsSuccess = true;
                    }

                    else
                    {
                        #region Insert Not Payment Message
                        Models.Message oMessage = new Models.Message();
                        oMessage.UserId = oRequest.UserId;
                        oMessage.LastState = oRequest.RequestState;
                        oMessage.MessageText = oVerifyResultList.verificationError.ToString();
                        oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                        oMessage.RequestId = oRequest.Id;
                        UnitOfWork.MessageRepository.Insert(oMessage);
                        #endregion
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                        ViewBag.IsSuccess = false;
                    }
                }

                //پرداخت اولیه نا موفق بوده است
                else
                {
                    #region Insert Not Payment Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Message;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    #endregion
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                    ViewBag.IsSuccess = false;
                }

                UnitOfWork.Save();

                var oRequestNew = UnitOfWork.RequestRepository.Get()
                     .Where(current => current.InvoiceNumber == InvoiceNumber)
                     .FirstOrDefault();
                return View(oRequestNew);
            }

            catch (Exception ex)
            {
                //Utilities.Net.LogHandler.Report(GetType(), null, ex);
                //return (RedirectToAction(MVC.Error.Display(Utilities.Net.LogHandler.Report(GetType(), null, ex), 0)));
                throw ex;
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult MerchantCommit()
        {
            Models.Request oRequest = null;
            Models.Request oRequestNew = null;
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                int InvoiceNumber = Convert.ToInt32(HttpContext.Request.Form["OrderId"]);

                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault()
                    ;

                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                OPS.ir.shaparak.sadad.CheckStatusResult _AppStatusCode
                    = oMerchantUtility.CheckRequestStatusResult
                    (oRequest.InvoiceNumber, oAccountNumber.MerchantId
                    , oAccountNumber.Terminal, oAccountNumber.TranKey
                    , oRequest.Bank_RequestKey, oRequest.AmountPaid)
                    ;

                DateTime ddddd = DateTime.Now;

                try
                {
                    ddddd = Convert.ToDateTime(_AppStatusCode.RealTransactionDateTime);
                }

                catch (Exception ex)
                {
                }

                oRequest.AmountPaidDate = ddddd;
                oRequest.Bank_AppStatus = _AppStatusCode.AppStatus;
                oRequest.Bank_AppStatusCode = _AppStatusCode.AppStatusCode;
                oRequest.Bank_AppStatusDescription = _AppStatusCode.AppStatusDescription;
                oRequest.Bank_BankReciptNumber = _AppStatusCode.BankReciptNumber;
                oRequest.Bank_CardHolderAccNumber = _AppStatusCode.CardHolderAccNumber;
                oRequest.Bank_CardHolderName = _AppStatusCode.CardHolderName;
                oRequest.Bank_CustomerCardNumber = _AppStatusCode.CustomerCardNumber;
                oRequest.Bank_FailCode = _AppStatusCode.FailCode;
                oRequest.Bank_NewlyCommitted = _AppStatusCode.NewlyCommitted;
                oRequest.Bank_RealTransactionDateTime = _AppStatusCode.RealTransactionDateTime;
                oRequest.Bank_RefrenceNumber = _AppStatusCode.RefrenceNumber;
                oRequest.Bank_ResponseCode = _AppStatusCode.ResponseCode;
                oRequest.Bank_ShamsiDate = _AppStatusCode.ShamsiDate;
                oRequest.Bank_TraceNo = _AppStatusCode.TraceNo;
                oRequest.Bank_Terminal = oAccountNumber.Terminal;
                oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                oRequest.UpdateDateTime = ddddd;

                if (_AppStatusCode.AppStatusCode == 0 && _AppStatusCode.AppStatusDescription == "COMMIT")
                {
                    #region Insert PaymentMessage
                    Models.Message oMessageP = new Models.Message();
                    oMessageP.UserId = oRequest.UserId;
                    oMessageP.LastState = oRequest.RequestState;
                    oMessageP.MessageText = Resources.Message.Request.Message_Paymented;
                    oMessageP.NewState = (int)Enums.RequestStates.Payment;
                    oMessageP.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessageP);
                    #endregion

                    if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode == (int)Enums.CurrencyUnits.Rails)
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                    else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                        oRequest.RequestState = (int)Enums.RequestStates.Payment;

                    else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                        oRequest.RequestState = (int)Enums.RequestStates.Payment;

                    else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Certificate)
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;
                    
                    else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Lims)
                        oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

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

                    if (oRequest.RequestState > (int)Enums.RequestStates.Payment)
                    {
                        #region Insert Payment Confirm Message
                        Models.Message oMessage = new Models.Message();
                        oMessage.UserId = oRequest.UserId;
                        oMessage.LastState = (int)Enums.RequestStates.Payment;
                        oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation;
                        oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                        oMessage.RequestId = oRequest.Id;
                        UnitOfWork.MessageRepository.Insert(oMessage);
                        #endregion
                    }
                }

                //else
                //{
                //	#region Insert Not Payment Message
                //	Models.Message oMessage = new Models.Message();
                //	oMessage.UserId = oRequest.UserId;
                //	oMessage.LastState = oRequest.RequestState;
                //	oMessage.MessageText = Resources.Message.Request.Message_NoPaymented;
                //	oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                //	oMessage.RequestId = oRequest.Id;
                //	UnitOfWork.MessageRepository.Insert(oMessage);
                //	#endregion

                //	oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
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

                UnitOfWork.RequestRepository.Update(oRequest);
                UnitOfWork.Save();

                oRequestNew =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault();

                return View(oRequestNew);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult MerchantCommitByInvoiceNumber(long? invoiceNumber)
        {
            Models.Request oRequest = null;
            Models.Request oRequestNew = null;
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                int InvoiceNumber = (Int32)invoiceNumber;

                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault()
                    ;

                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                //OPS.ir.shaparak.sadad.CheckStatusResult _AppStatusCode
                //    = oMerchantUtility.CheckRequestStatusResult
                //    (oRequest.InvoiceNumber, oAccountNumber.MerchantId
                //    , oAccountNumber.Terminal, oAccountNumber.TranKey
                //    , oRequest.Bank_RequestKey, oRequest.AmountPaid)
                //    ;

                DateTime ddddd = DateTime.Now;

                //try
                //{
                //    ddddd = Convert.ToDateTime(_AppStatusCode.RealTransactionDateTime);
                //}

                //catch (Exception ex)
                //{
                //}

                oRequest.AmountPaidDate = ddddd;
                //oRequest.Bank_AppStatus = _AppStatusCode.AppStatus;
                //oRequest.Bank_AppStatusCode = _AppStatusCode.AppStatusCode;
                //oRequest.Bank_AppStatusDescription = _AppStatusCode.AppStatusDescription;
                //oRequest.Bank_BankReciptNumber = _AppStatusCode.BankReciptNumber;
                //oRequest.Bank_CardHolderAccNumber = _AppStatusCode.CardHolderAccNumber;
                //oRequest.Bank_CardHolderName = _AppStatusCode.CardHolderName;
                //oRequest.Bank_CustomerCardNumber = _AppStatusCode.CustomerCardNumber;
                //oRequest.Bank_FailCode = _AppStatusCode.FailCode;
                //oRequest.Bank_NewlyCommitted = _AppStatusCode.NewlyCommitted;
                //oRequest.Bank_RealTransactionDateTime = _AppStatusCode.RealTransactionDateTime;
                //oRequest.Bank_RefrenceNumber = _AppStatusCode.RefrenceNumber;
                //oRequest.Bank_ResponseCode = _AppStatusCode.ResponseCode;
                //oRequest.Bank_ShamsiDate = _AppStatusCode.ShamsiDate;
                //oRequest.Bank_TraceNo = _AppStatusCode.TraceNo;
                oRequest.Bank_Terminal = oAccountNumber.Terminal;
                oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                oRequest.UpdateDateTime = ddddd;

                //if (_AppStatusCode.AppStatusCode == 0 && _AppStatusCode.AppStatusDescription == "COMMIT")
                //{
                //    #region Insert PaymentMessage
                //    Models.Message oMessageP = new Models.Message();
                //    oMessageP.UserId = oRequest.UserId;
                //    oMessageP.LastState = oRequest.RequestState;
                //    oMessageP.MessageText = Resources.Message.Request.Message_Paymented;
                //    oMessageP.NewState = (int)Enums.RequestStates.Payment;
                //    oMessageP.RequestId = oRequest.Id;
                //    UnitOfWork.MessageRepository.Insert(oMessageP);
                //    #endregion

                if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode == (int)Enums.CurrencyUnits.Rails)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Import && oRequest.CurrencyCode != (int)Enums.CurrencyUnits.Rails)
                    oRequest.RequestState = (int)Enums.RequestStates.Payment;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                    oRequest.RequestState = (int)Enums.RequestStates.Payment;

                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Certificate)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;
                
                else if (oRequest.SubSystem.Code == (int)Enums.SubSystems.Lims)
                    oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

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

                if (oRequest.RequestState > (int)Enums.RequestStates.Payment)
                {
                    #region Insert Payment Confirm Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = (int)Enums.RequestStates.Payment;
                    oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    #endregion
                }
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

                UnitOfWork.RequestRepository.Update(oRequest);
                UnitOfWork.Save();

                oRequestNew =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault();

                return View(oRequestNew);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult MerchantCommitFile(int invoiceNumber)
        {
            Models.Request oRequest = null;
            try
            {
                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == invoiceNumber)
                    .FirstOrDefault();

                //Use ViewAsPdf Class to generate pdf using GeneratePDF.cshtml view
                var File = new Rotativa.MVC.ViewAsPdf("MerchantCommitFile", oRequest) { FileName = "firstPdf.pdf" };
                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintDepositNumber(int invoiceNumber)
        {
            Models.Request oRequest = null;
            try
            {
                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == invoiceNumber)
                    .FirstOrDefault();

                //Use ViewAsPdf Class to generate pdf using GeneratePDF.cshtml view
                var File = new Rotativa.MVC.ViewAsPdf("PrintDepositNumber", oRequest) { FileName = "PrintDepositNumber.pdf" };
                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult TestAction()
        {
            try
            {
                int TestId = 274312;
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                var oRequest =
                 UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == TestId)
                 .FirstOrDefault()
                 ;

                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);

                long AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);

                if (oRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                {
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    oRequest.Bank_RequestKey = Infrastructure.Sessions.CBISessionId;
                    UnitOfWork.RequestRepository.Update(oRequest);
                    UnitOfWork.Save();
                }

                if (oRequest.RequestState == (int)Enums.RequestStates.PaymentOrder
                    && oCBINasim.CreatedFormBody(oRequest, AmountPaid, out FormBody))
                {
                    #region Update CurrencyRation
                    oRequest.CurrencyRation = oCurrency.Ratio;
                    oRequest.AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oCurrency.Ratio);
                    UnitOfWork.RequestRepository.Update(oRequest);
                    #endregion

                    #region Insert Payment Confirm Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.MessageText = Resources.Message.Request.Message_PaymentRequest;
                    oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion
                }

                ViewBag.FormBody = FormBody;

                var retValue = UnitOfWork.RequestRepository.Get()
                 .Where(current => current.InvoiceNumber == TestId)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Request.DisplayViewModel()
                 {
                     DepositNumber = current.DepositNumber,
                     Id = current.Id,
                     SubSystem = current.SubSystem.Name,
                     CompanyName = current.CompanyName,
                     CompanyNationalCode = current.CompanyNationalCode,
                     RecordNumber = current.RecordNumber,
                     RecordDate = current.RecordDate,
                     InvoiceNumber = current.InvoiceNumber,
                     InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                     CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                     CurrencyValue = current.CurrencyValue,
                     AmountPaid = AmountPaid,
                     RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                     Bank_TraceNo = current.Bank_TraceNo,
                     Bank_ShamsiDate = current.Bank_ShamsiDate
                 })
                 .FirstOrDefault();

                return View(retValue);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                throw ex;
            }
        }

        private OPS.ir.shaparak.sadad.CheckStatusResult CheckRequestStatus(Models.Request request)
        {
            string retValue = string.Empty;
            try
            {
                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == request.ProvinceId)
                    .Where(current => current.SubSystemId == request.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                //long AmountPaid = Convert.ToInt64(request.CurrencyValue * request.CurrencyRation) + 33000;

                string _RefNo, Appstatus, RealTransactionDateTime;
                oMerchantUtility.Url = Infrastructure.WebServiceSetting_Sadad.ServiceURL;

                int _AppStatusCode =
                    oMerchantUtility.CheckRequestStatusWithRealTransactionDateTime
                    (request.InvoiceNumber, oAccountNumber.MerchantId
                    , oAccountNumber.Terminal, oAccountNumber.TranKey
                    , request.Bank_RequestKey, request.AmountPaid
                    , out _RefNo, out Appstatus, out RealTransactionDateTime);


                OPS.ir.shaparak.sadad.CheckStatusResult _AppStatusCode1 =
                    oMerchantUtility.CheckRequestStatusResult
                    (request.InvoiceNumber, oAccountNumber.MerchantId
                    , oAccountNumber.Terminal, oAccountNumber.TranKey
                    , request.Bank_RequestKey, request.AmountPaid);

                return _AppStatusCode1;
            }
            catch (Exception ex)
            {
                //retValue += Resources.Message.Request.ErrorInVerification;
                throw ex;
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult MerchantCommit1()
        {
            Models.Request oRequest = null;
            Models.Request oRequestNew = null;
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                int InvoiceNumber = 230032;

                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault();

                oRequestNew =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault();

                return View(oRequestNew);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult MerchantCommit2(int invoicenumber)
        {
            Models.Request oRequest = null;
            Models.Request oRequestNew = null;
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                int InvoiceNumber = invoicenumber;

                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.InvoiceNumber == InvoiceNumber)
                    .FirstOrDefault();

                //long AmountPaid = Convert.ToInt64(oRequest.CurrencyValue * oRequest.CurrencyRation) + 33000;

                var oAccountNumber =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.ProvinceId == oRequest.ProvinceId)
                    .Where(current => current.SubSystemId == oRequest.SubSystemId)
                    .FirstOrDefault()
                    .AccountNumber
                    ;

                OPS.ir.shaparak.sadad.CheckStatusResult _AppStatusCode =
                   oMerchantUtility.CheckRequestStatusResult
                   (oRequest.InvoiceNumber, oAccountNumber.MerchantId
                   , oAccountNumber.Terminal, oAccountNumber.TranKey
                   , oRequest.Bank_RequestKey, oRequest.AmountPaid);

                oRequest.Bank_AppStatus = _AppStatusCode.AppStatus;
                oRequest.Bank_AppStatusCode = _AppStatusCode.AppStatusCode;
                oRequest.Bank_AppStatusDescription = _AppStatusCode.AppStatusDescription;
                oRequest.Bank_BankReciptNumber = _AppStatusCode.BankReciptNumber;
                oRequest.Bank_CardHolderAccNumber = _AppStatusCode.CardHolderAccNumber;
                oRequest.Bank_CardHolderName = _AppStatusCode.CardHolderName;
                oRequest.Bank_CustomerCardNumber = _AppStatusCode.CustomerCardNumber;
                oRequest.Bank_FailCode = _AppStatusCode.FailCode;
                oRequest.Bank_NewlyCommitted = _AppStatusCode.NewlyCommitted;
                oRequest.Bank_RealTransactionDateTime = _AppStatusCode.RealTransactionDateTime;
                oRequest.Bank_RefrenceNumber = _AppStatusCode.RefrenceNumber;
                oRequest.Bank_ResponseCode = _AppStatusCode.ResponseCode;
                oRequest.Bank_ShamsiDate = _AppStatusCode.ShamsiDate;
                oRequest.Bank_TraceNo = _AppStatusCode.TraceNo;
                oRequest.Bank_Terminal = oAccountNumber.Terminal;
                oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                oRequest.RequestState = (int)Enums.RequestStates.PaymentConfirmation;

                UnitOfWork.RequestRepository.Update(oRequest);
                UnitOfWork.Save();

                return View(oRequestNew);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintFinalPayment(int invoicenumber)
        {
            Models.Request oRequest = null;
            try
            {
                oRequest = UnitOfWork.RequestRepository.CustomGetByInvoiceNumber(invoicenumber);
                Infrastructure.Barcodecs oBarcodes = new Infrastructure.Barcodecs();

                //TODO: Create Barcode
                #region Create Barcode
                string strBarcode = oRequest.DepositNumber;

                ViewBag.BarCodeImage = "data:image/jpg;base64,"
                    + Convert.ToBase64String((byte[])oBarcodes.getBarcodeImage(strBarcode, string.Empty));
                #endregion

                string CreateFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + oRequest.DepositNumber + ".pdf";
                var File = new Rotativa.MVC.ViewAsPdf("PrintFinalPayment", oRequest) { FileName = CreateFileName };
                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintDepositNumber(Guid requestId)
        {
            Models.Request oRequest = null;
            try
            {
                oRequest = UnitOfWork.RequestRepository.CustomGet(requestId);
                Infrastructure.Barcodecs oBarcodes = new Infrastructure.Barcodecs();

                //TODO: Create Barcode
                #region Create Barcode
                string strBarcode = oRequest.DepositNumber;

                ViewBag.BarCodeImage = "data:image/jpg;base64,"
                    + Convert.ToBase64String((byte[])oBarcodes.getBarcodeImage(strBarcode, string.Empty));
                #endregion

                string CreateFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + oRequest.DepositNumber + ".pdf";
                var File = new Rotativa.MVC.ViewAsPdf("PrintDepositNumber", oRequest) { FileName = CreateFileName };
                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintDepositNumberNew(int invoicenumber)
        {
            Models.Request oRequest = null;
            try
            {
                oRequest = UnitOfWork.RequestRepository.CustomGetByInvoiceNumber(invoicenumber);
                Infrastructure.Barcodecs oBarcodes = new Infrastructure.Barcodecs();

                //TODO: Create Barcode
                #region Create Barcode
                string strBarcode = oRequest.DepositNumber;

                ViewBag.BarCodeImage = "data:image/jpg;base64,"
                    + Convert.ToBase64String((byte[])oBarcodes.getBarcodeImage(strBarcode, string.Empty));
                #endregion

                string CreateFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + oRequest.DepositNumber + ".pdf";
                var File = new Rotativa.MVC.ViewAsPdf("PrintDepositNumberNew", oRequest) { FileName = CreateFileName };
                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult NewMerchantCommit() { return View(); }

    }
}