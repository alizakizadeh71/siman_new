using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class CentralBankController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/CentralBank
        [System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliExpert01)]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel bankTransaction = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel();
            return View();
        }
        public string GetPaymentCodes10(ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel bankTransaction, out bool isOk)
        {
            string message = "1";
            isOk = true;
            return null;
            bankTransaction = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel
            {
                AccountNumber = "4001038301019288",
                FromDateTime = "140103150100",
                ToDateTime = "140105150100",
                PageNumber = 1,
                RecordCount = 50,
                PaymentIdentifier = "282038362160176095580110946754"
            };
            List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel> bankTransactionsDetailslist = new List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel>();
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (mender, certificate, chain, sslPolicyErrors) => true;
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

                using (var client = new BehdadService.AccountServiceClient())
                {
                    client.ClientCredentials.ClientCertificate.Certificate =
                        new System.Security.Cryptography.X509Certificates.X509Certificate2(@"D:\BankMarkazi\orgcertBundle.pfx", "OPS_P@p2021");
                    client.InnerChannel.OperationTimeout = System.TimeSpan.FromSeconds(300);

                    var AccountNumbers = client.getAccountNumbers(new BehdadService.credential() { username = "4172149286", password = "AliZaki5792568@@" });



                    string[] accountNumberlist = new string[3] { "4001038301006103", "4001038301016201", "4001038301019288" };







                    var result2 = client.getMultipleAccountTransactionsDetails(new BehdadService.credential()
                    { username = "4172149286" + bankTransaction.AccountNumber, password = "AliZaki5792568@@" },
    //{ username = bankTransaction.accountNumber, password = bankTransaction.accountNumber },
    new BehdadService.multipleAccountTransactionFilter()
    {
        fromDateTime = "139801010101",
        toDateTime = "143001010101",
        accountNumbers = accountNumberlist,
        paymentIdentifier = bankTransaction.PaymentIdentifier
    },
    new BehdadService.paging()
    {
        pageNumber = bankTransaction.PageNumber,
        recordCount = bankTransaction.RecordCount,
        pageNumberSpecified = true,
        recordCountSpecified = true
    });

















                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.JsonResult GetCentralBank(ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel viewModel)
        {
            return null;
            var bankTransaction =
                UnitOfWork.PaymentDetailRepository.Get()
                ;

            if (!string.IsNullOrEmpty(viewModel.PaymentIdentifier))
            {
                bankTransaction = bankTransaction.Where(e => e.PaymentIdentifier == viewModel.PaymentIdentifier);
            }

            var ViewModelsPaymentDetail
                = bankTransaction
                //.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel()
                    {
                        Id = current.Id,
                        AccountNumber = current.AccountNumber,
                        PaymentIdentifier = current.PaymentIdentifier,
                        //FromDateTime = Infrastructure.Utility.DisplayDateTime(current.FromDateTime, true),
                    })
                    .AsQueryable();

            object dataSource;

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel>(ViewModelsPaymentDetail, true, out dataSource);

            Infrastructure.Sessions.SearchDataSource = dataSource;
            var bankTransactio = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel();

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        //public static List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel> GetPaymentCodes(ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel bankTransaction, out bool isOk)
        //{
        //    isOk = true;
        //    bankTransaction = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel
        //    {
        //        AccountNumber = "4001038301019288",
        //        FromDateTime = "140103150100",
        //        ToDateTime = "140104150100",
        //        PageNumber = 1,
        //        RecordCount = 50
        //    };
        //    List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel> bankTransactionsDetailslist = new List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel>();
        //    try
        //    {
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback += (mender, certificate, chain, sslPolicyErrors) => true;
        //        System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

        //        using (var client = new BehdadService.AccountServiceClient())
        //        {
        //            //     < endpoint address = "https://10.0.227.51:8090/behdad/accountservice"
        //            //binding = "basicHttpBinding" bindingConfiguration = "AccountServiceImplServiceSoapBinding"
        //            //contract = "BehdadService.AccountService" name = "AccountServiceImplPort" />


        //            //client.ClientCredentials.ClientCertificate.Certificate =
        //            //	new System.Security.Cryptography.X509Certificates.X509Certificate2(@"C:\PAP\Published\MainPublish\App_Data\esCertificate.p12", "eesDoTEleYOAidOtAyR1986");
        //            client.ClientCredentials.ClientCertificate.Certificate =
        //                new System.Security.Cryptography.X509Certificates.X509Certificate2(@"D:\BankMarkazi\orgcertBundle.pfx", "OPS_P@p2021");
        //            client.InnerChannel.OperationTimeout = System.TimeSpan.FromSeconds(300);
        //            //client.ClientCredentials.ClientCertificate.Certificate = 
        //            //	new System.Security.Cryptography.X509Certificates.X509Certificate2(@"C:\PAP\Test\File\esCertificate.p12", "eesDoTEleYOAidOtAyR1986");
        //            //  var result6 = client.getAccountNumbers(new ServiceReference1.credential() { username = "4001038101016328", password = "4001038101016328" });

        //            var sssssssss = client.getAccountNumbers(new BehdadService.credential() { username = "4172149286", password = "%ivTXtt2#Jgn" });


        //            var result2 = client.getBankTransactionsDetails(new BehdadService.credential()
        //            { username = "4172149286" + bankTransaction.AccountNumber, password = "%ivTXtt2#Jgn" },
        //                //{ username = bankTransaction.accountNumber, password = bankTransaction.accountNumber },
        //                new BehdadService.accountTransactionFilter()
        //                {
        //                    accountNumber = bankTransaction.AccountNumber,
        //                    fromDateTime = bankTransaction.FromDateTime,
        //                    toDateTime = bankTransaction.ToDateTime,
        //                    paymentIdentifier = bankTransaction.PaymentIdentifier
        //                },
        //                new BehdadService.paging()
        //                {
        //                    pageNumber = bankTransaction.PageNumber,
        //                    recordCount = bankTransaction.RecordCount,
        //                    pageNumberSpecified = true,
        //                    recordCountSpecified = true
        //                });

        //            //var result2 = client.getBankTransactionsDetails(new BehdadService.credential() { username = "4001038101016328", password = "4001038101016328" },
        //            //	new BehdadService.accountTransactionFilter()
        //            //	{ accountNumber = "4001038101016328", fromDateTime = "139702120100", toDateTime = "139705120100", paymentIdentifier = "" },
        //            //new BehdadService.paging() { pageNumber = 1, recordCount = 10, pageNumberSpecified = true, recordCountSpecified = true });
        //            //new BehdadService.accountTransactionFilter()
        //            //{ accountNumber = bankTransaction.accountNumber, fromDateTime = bankTransaction.fromDateTime, toDateTime = bankTransaction.toDateTime, paymentIdentifier = bankTransaction.paymentIdentifier },
        //            //new BehdadService.paging() { pageNumber = bankTransaction.pageNumber.Value, recordCount = bankTransaction.recordCount.Value, pageNumberSpecified = true, recordCountSpecified = true });
        //            if (result2 != null && result2.currentPageData != null)
        //                for (int i = 0; i < result2.currentPageData.Length; i++)
        //                {
        //                    var paymentOnlineDetail = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel();
        //                    foreach (var item in result2.currentPageData[i] as System.Xml.XmlNode[])
        //                    {
        //                        switch (item.Name)
        //                        {
        //                            case "transactionId":
        //                                paymentOnlineDetail.TransactionNumber = item.InnerText;
        //                                break;
        //                            case "amount":
        //                                paymentOnlineDetail.Value = double.Parse(item.InnerText);
        //                                break;

        //                            //case "amount":
        //                            //	paymentOnlineDetail.Value = double.Parse(item.InnerText);
        //                            //	break;

        //                            case "accountNumber":
        //                                paymentOnlineDetail.AccountNumber = item.InnerText;

        //                                break;
        //                            case "transactionType":
        //                                paymentOnlineDetail.TransactionAccountType = 1;
        //                                //paymentOnlineDetail.TransactionAccountType = item.InnerText == "واريز" || item.InnerText == "DPS" ?
        //                                //    Common.Enums.TransactionAccountType.deposit : item.InnerText == "برگشت" || item.InnerText == "برداشت اصلاحی" || item.InnerText == "RWD" ?
        //                                //    Common.Enums.TransactionAccountType.Return : Common.Enums.TransactionAccountType.None;
        //                                break;
        //                            case "transactionBalance":
        //                                paymentOnlineDetail.Remind = double.Parse(item.InnerText);

        //                                break;
        //                            case "transactionMediaType":
        //                                paymentOnlineDetail.Device = item.InnerText;

        //                                break;
        //                            //case "transactionDescription":
        //                            //	paymentOnlineDetail.Description = item.InnerText;

        //                            //	break;
        //                            case "transactionDate":
        //                                paymentOnlineDetail.DateDone = ConvertPerToDate(item.InnerText) ?? System.DateTime.MinValue;

        //                                break;
        //                            case "description":
        //                                paymentOnlineDetail.Description = item.InnerText;

        //                                break;
        //                            case "transactionIdentifier":
        //                                paymentOnlineDetail.PaymentCode = item.InnerText;
        //                                break;

        //                            //case "transactionMediaSerial":
        //                            //	paymentOnlineDetail.Device = item.InnerText;
        //                            //	break;
        //                            //case "sourceAccountNumber":
        //                            //	BankTransactionsDetails.sourceAccountNumber = item.InnerText;
        //                            //	break;

        //                            //case "destinationAccountNumbers":
        //                            //	BankTransactionsDetails.destinationAccountNumber = item.InnerText;
        //                            //	break;

        //                            //case "transactionStatusType":
        //                            //	BankTransactionsDetails.transactionStatusType = item.InnerText;
        //                            //	break;

        //                            //case "transactionStatusDate":
        //                            //	BankTransactionsDetails.transactionStatusDate = item.InnerText;
        //                            //	break;
        //                            //case "transactionStatusTime":
        //                            //	BankTransactionsDetails.transactionStatusTime = item.InnerText;
        //                            //	break;

        //                            //case "transactionMediaType":
        //                            //	BankTransactionsDetails.transactionMediaType = item.InnerText;
        //                            //	break;

        //                            //case "transactionMediaSerial":
        //                            //	BankTransactionsDetails.transactionMediaSerial = item.InnerText;
        //                            //	break;

        //                            //case "isGroupTransfer":
        //                            //	BankTransactionsDetails.isGroupTransfer = item.InnerText;
        //                            //	break;
        //                            //case "pageIndex":
        //                            //	BankTransactionsDetails.pageIndex = item.InnerText;
        //                            //	break;
        //                            //case "pageSize":
        //                            //	BankTransactionsDetails.pageSize = item.InnerText;
        //                            //	break;
        //                            //case "totalCount":
        //                            //	BankTransactionsDetails.totalCount = item.InnerText;
        //                            //	break;
        //                            //case "amount":
        //                            //	BankTransactionsDetails.amount = item.InnerText;
        //                            //	break;
        //                            //case "description":
        //                            //	BankTransactionsDetails.description = item.InnerText;
        //                            //	break;
        //                            //case "balance":
        //                            //	BankTransactionsDetails.balance = item.InnerText;
        //                            //	break;

        //                            default:
        //                                break;
        //                        }

        //                    }
        //                    bankTransactionsDetailslist.Add(paymentOnlineDetail);
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel>();
        //    }

        //    return bankTransactionsDetailslist;
        //}

        public static System.DateTime? ConvertPerToDate
            (string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
            {
                return null;
            }

            var pattern = @"^(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})$";

            if (System.Text.RegularExpressions.Regex.IsMatch(dateTime, pattern))
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                System.Text.RegularExpressions.GroupCollection groups = regex.Match(dateTime).Groups;
                int year = Convert.ToInt32(ConvertNumberPersion(groups["year"].Value));

                int month = Convert.ToInt32(ConvertNumberPersion(groups["month"].Value));

                int day = Convert.ToInt32(ConvertNumberPersion(groups["day"].Value));

                System.Globalization.PersianCalendar calendar = new System.Globalization.PersianCalendar();

                return calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }

            return null;
        }

        public static string ConvertNumberPersion
            (string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return number;
            }

            number = number.Replace("۰", "0");
            number = number.Replace("۱", "1");
            number = number.Replace("۲", "2");
            number = number.Replace("۳", "3");
            number = number.Replace("۴", "4");
            number = number.Replace("۵", "5");
            number = number.Replace("۶", "6");
            number = number.Replace("۷", "7");
            number = number.Replace("۸", "8");
            number = number.Replace("۹", "9");

            return number;
        }






        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PaymentInquiry(string Id)
        {
            string Error = "0";
            string Message = string.Empty;
            Models.Request oRequest = null;
            try
            {
                int InvoiceNumber = System.Convert.ToInt32(Id);

                oRequest =
                UnitOfWork.RequestRepository.Get()
                .Where(current => current.InvoiceNumber == InvoiceNumber)
                .FirstOrDefault();

                System.Net.ServicePointManager.ServerCertificateValidationCallback += (mender, certificate, chain, sslPolicyErrors) => true;
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;


                using (var client = new BehdadService.AccountServiceClient())
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~");
                    string orgCertBundle = path + @"App_Data\" + @"\orgCertBundle.pfx";

                    client.ClientCredentials.ClientCertificate.Certificate =
                                 new System.Security.Cryptography.X509Certificates.X509Certificate2
                                 (orgCertBundle, "OPS_P@p2021");
                    client.InnerChannel.OperationTimeout = System.TimeSpan.FromSeconds(300);
                    Error = "1";

                    var BankAccountId = UnitOfWork.ServiceTariffRepository.Get()
                            .FirstOrDefault(current => current.Id == oRequest.ServiceTariffId.Value).BankAccountId;

                    var AccountNumber = UnitOfWork.BankAccountRepository.Get()
                            .FirstOrDefault(current => current.Id == BankAccountId).AccountNumber;
                    Error = "2";
                    var AccountNumbers = client.getAccountNumbers(new BehdadService.credential() { username = "4172149286", password = "AliZaki5792568@@" });
                    Error = "3";

                    string accountNumber = string.Join(" , ", AccountNumbers.Select(y => y.accountNumber).ToList());

                    Error = "4" + accountNumber;

                    var result2 = client.getBankTransactionsDetails(
                        new BehdadService.credential()
                        {
                            username = "4172149286",
                            password = "AliZaki5792568@@"
                        },
                        new BehdadService.accountTransactionFilter()
                        {
                            fromDateTime = "139801010101",
                            toDateTime = "143001010101",
                            accountNumber = AccountNumber,
                            paymentIdentifier = oRequest.DepositNumber,
                        },
                        new BehdadService.paging()
                        {
                            pageNumber = 1,
                            recordCount = 10,
                            pageNumberSpecified = true,
                            recordCountSpecified = true
                        });
                    Error = "5";

                    //string[] accountNumberlist = new string[3] { "4001038301006103", "4001038301016201", "4001038301019288" };
                    //var result2 = client.getMultipleAccountTransactionsDetails(
                    //    new BehdadService.credential()
                    //    {
                    //        username = "4172149286",
                    //        password = "AliZaki5792568@@"
                    //    },
                    //    new BehdadService.multipleAccountTransactionFilter()
                    //    {
                    //        fromDateTime = "139801010101",
                    //        toDateTime = "143001010101",
                    //        accountNumbers = accountNumberlist,
                    //        paymentIdentifier = oRequest.DepositNumber,
                    //    },
                    //    new BehdadService.paging()
                    //    {
                    //        pageNumber = 1,
                    //        recordCount = 10,
                    //        pageNumberSpecified = true,
                    //        recordCountSpecified = true
                    //    });

                    List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel> bankTransactionsDetailslist = new List<ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel>();

                    Error = "6";
                    if (result2 != null && result2.currentPageData != null)
                        for (int i = 0; i < result2.currentPageData.Length; i++)
                        {
                            var paymentOnlineDetail = new ViewModels.Areas.Administrator.CentralBank.bankTransactionViewModel();
                            foreach (var item in result2.currentPageData[i] as System.Xml.XmlNode[])
                            {
                                switch (item.Name)
                                {
                                    case "amount":
                                        paymentOnlineDetail.amount = double.Parse(item.InnerText);
                                        break;

                                    case "transactionType":
                                        paymentOnlineDetail.transactionType = item.InnerText == "واريز" || item.InnerText == "DPS" ? 3 : 0;
                                        break;

                                    case "transactionId":
                                        paymentOnlineDetail.traceNumber = long.Parse(item.InnerText);
                                        break;

                                    case "transactionDate":
                                        paymentOnlineDetail.transactionDate = item.InnerText;
                                        break;

                                    default:
                                        break;
                                }

                            }
                            bankTransactionsDetailslist.Add(paymentOnlineDetail);
                        }
                    Error = "7";

                    if (bankTransactionsDetailslist.Count == 0)
                    {
                        Message = " پرداخت انجام نشده است ";
                    }

                    if (bankTransactionsDetailslist.Count == 1)
                    {
                        if (bankTransactionsDetailslist.FirstOrDefault().amount == oRequest.AmountPaid)
                        {
                            if (bankTransactionsDetailslist.FirstOrDefault().transactionType == 3)
                            {
                                if (oRequest.RequestState != 3)
                                {
                                    oRequest.RequestState = 3;
                                    oRequest.Bank_TraceNo = bankTransactionsDetailslist.FirstOrDefault().traceNumber;
                                    oRequest.Bank_ShamsiDate = bankTransactionsDetailslist.FirstOrDefault().transactionDate;
                                    oRequest.Bank_BankReciptNumber = oRequest.Bank_ShamsiDate + oRequest.Bank_TraceNo;
                                    UnitOfWork.RequestRepository.Update(oRequest);
                                    UnitOfWork.Save();
                                }
                                Message = " پرداخت با موفقیت انجام شده است ";
                            }
                            else if (bankTransactionsDetailslist.FirstOrDefault().transactionType != 3)
                            {
                                Message = " پرداخت انجام نشده است ";
                            }
                        }
                    }
                    Error = "8";

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oMessage.LastState = (int)Enums.RequestStates.PaymentOrder;
                    oMessage.MessageText = " استعلام پرداخت از بانک مرکزی - " + Message;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion
                }
            }

            catch (Exception ex)
            {
                Message = " خطا در استعلام اطلاعات " + ex.Message + " → " + Error;
            }
            return Json(new { success = true, Message = Message }, JsonRequestBehavior.AllowGet);
        }
    }
}