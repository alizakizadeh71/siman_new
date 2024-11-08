using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPS.ir.shaparak.sadad;
using ViewModels.Areas.Administrator.Cement;
using ViewModels.Areas.Administrator.Request;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class RequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            //var listItems = new SelectList(
            //    new List<SelectListItem>
            //    {
            //    new SelectListItem { Selected = true, Text = "نهایی شده ها", Value = "true"},
            //    new SelectListItem { Selected = false, Text = "نهایی نشده ها", Value = "false"},
            //    });
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", null).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(new Guid()).ToList(); /// نوع کالا
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", null).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(new Guid()).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", null).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(new Guid()).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", null).OrderBy(x => x.Text); /// ممتازان کرمان

            var Tonnage = UnitOfWork.tonnageRepository.GetByPackageTypeId(new Guid()).ToList(); /// کیسه
            base.ViewData["Tonnage"] = new System.Web.Mvc.SelectList(Tonnage, "Id", "Name", null).OrderBy(x => x.Text); /// 12 تن

            var stringFinalApprove = Infrastructure.Utility.EnumList(Enums.EnumTypes.FinalApprove);
            base.ViewData["stringFinalApprove"] = new System.Web.Mvc.SelectList(stringFinalApprove, "Id", "Name", 1);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            base.ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.FactorCementRepository.GetByUser(Infrastructure.Sessions.AuthenticatedUser.User);

            if (viewModel == null)
                varRequest = varRequest.Where(x => x.FinalApprove == true);

            #region Condition
            if (!string.IsNullOrEmpty(viewModel?.BuyerMobile))
            {
                viewModel.BuyerMobile = Utilities.Text.Utility.FixText(viewModel.BuyerMobile);
                varRequest = varRequest.Where(x => x.BuyerMobile == viewModel.BuyerMobile);
                Search = true;
            }

            if (!string.IsNullOrEmpty(viewModel?.InvoiceNumber.ToString()))
            {
                varRequest = varRequest.Where(x => x.InvoiceNumber == viewModel.InvoiceNumber);
                Search = true;
            }

            if (viewModel?.ProductName != null && viewModel.ProductName != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.ProductNameId == viewModel.ProductName);
                Search = true;
            }

            if (viewModel?.ProductType != null && viewModel.ProductType != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.ProductTypeId == viewModel.ProductType);
                Search = true;
            }

            if (viewModel?.PackageType != null && viewModel.PackageType != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.PackageTypeId == viewModel.PackageType);
                Search = true;
            }

            if (viewModel?.FactoryName != null && viewModel.FactoryName != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.FactoryNameId == viewModel.FactoryName);
                Search = true;
            }

            if (viewModel?.Tonnage != null && viewModel.Tonnage != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.TonnageId == viewModel.Tonnage);
                Search = true;
            }

            if (viewModel?.stringFinalApprove == "1")
            {
                viewModel.FinalApprove = true;
                varRequest = varRequest.Where(x => x.FinalApprove == viewModel.FinalApprove);
                Search = true;
            }

            if (viewModel?.stringFinalApprove == "0")
            {
                viewModel.FinalApprove = false;
                varRequest = varRequest.Where(x => x.FinalApprove == viewModel.FinalApprove);
                Search = true;
            }


            if (viewModel?.Province != null && viewModel.Province != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);
                Search = true;
            }

            if (viewModel?.City != null && viewModel?.City != new Guid())
            {
                varRequest = varRequest.Where(x => x.CityId == viewModel.City);
                Search = true;
            }
            if (viewModel?.FromAmount.ToString().Length > 0 && viewModel.ToAmount.ToString().Length > 0 && viewModel.FromAmount <= viewModel.ToAmount)
            {
                varRequest =
                    varRequest
                    .Where(current => current.AmountPaid >= viewModel.FromAmount && current.AmountPaid <= viewModel.ToAmount)
                    ;
                Search = true;
            }
            if (viewModel?.StartDate.ToString().Length > 0)
            {
                varRequest =
                    varRequest
                    .Where(current => current.InsertDateTime >= viewModel.StartDate)
                    ;
                Search = true;
            }
            if (viewModel?.EndDate.ToString().Length > 0)
            {
                var EndDate1 = viewModel.EndDate;
                var EndDate2 = EndDate1.Value.AddDays(1);
                varRequest =
                    varRequest
                    .Where(current => current.InsertDateTime < EndDate2)
                    ;
                Search = true;
            }

            if (viewModel?.PayStartDate.ToString().Length > 0)
            {
                varRequest = varRequest.Where(current => current.AmountPaidDate >= viewModel.PayStartDate);
                Search = true;
            }
            if (viewModel?.PayEndDate.ToString().Length > 0)
            {
                var PayEndDate1 = viewModel.PayEndDate.Value;
                var PayEndDate2 = PayEndDate1.AddDays(1);
                var StringPayEndDate = ConvertDate(PayEndDate2.ToString());
                varRequest = varRequest.Where(current => current.AmountPaidDate < PayEndDate2);
                Search = true;
            }
            #endregion

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InvoiceNumber)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            InvoiceNumber = current.InvoiceNumber,
                            StringProductName = current.ProductName.Name,
                            StringProductType = current.ProductType.Name,
                            StringPackageType = current.PackageType.Name,
                            StringFactoryName = current.FactoryName.Name,
                            StringTonnage = current.Tonnage.Name,
                            StringProvince = current.Province.Name,
                            StringCity = current.City.Name,
                            BuyerMobile = current.BuyerMobile,
                            RemittanceNumber = current.RemittanceNumber,
                            //FinalApprove = current.FinalApprove,
                            stringFinalApprove = current.FinalApprove == true ? "نهایی شده" : "نهایی نشده",
                            AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? current.DestinationAmountPaid.Value : 0,
                            MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                            StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                        })
                        .AsQueryable();

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Cement.CementViewModel>(ViewModelsvarBanks);

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool NewMethod(Models.Request current, string StringPayStartDate, DateTime? PayStartDate)
        {
            if (current.AmountPaidDate != null)
            {
                if (current.AmountPaidDate >= PayStartDate)
                    return true;
            }

            if (current.Bank_ShamsiDate == null)
                return false;

            if (current.Bank_ShamsiDate.Length != 10)
                return false;

            return (StringPayStartDate.CompareTo(current.Bank_ShamsiDate) <= 0);
        }

        private string ConvertDate(string PayStartDate)
        {
            string Year = PayStartDate.Substring(6, 4);
            string Month = PayStartDate.Substring(3, 2);
            string Day = PayStartDate.Substring(0, 2);
            return Year + "/" + Month + "/" + Day;
        }

        private void ViewData(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text); /// ممتازان کرمان

            var Tonnage = UnitOfWork.tonnageRepository.GetByPackageTypeId(cementViewModel.PackageType).ToList(); /// کیسه
            base.ViewData["Tonnage"] = new System.Web.Mvc.SelectList(Tonnage, "Id", "Name", cementViewModel.Tonnage).OrderBy(x => x.Text); /// 12 تن

            var Province = UnitOfWork.ProvinceRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(Province, "Id", "Name", cementViewModel.Province).OrderBy(x => x.Text);

            var City = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList(); /// کرمان
            base.ViewData["City"] = new System.Web.Mvc.SelectList(City, "Id", "Name", cementViewModel.City).OrderBy(x => x.Text); /// کوهبنان
        }

        private string GetCity(Guid? cityId)
        {
            string Name = string.Empty;
            if (cityId != null)
            {
                Name = UnitOfWork.CityRepository.GetById(cityId.Value).Name
                 ;
            }

            return Name;
        }

        private string GetServiceTariff(Guid? serviceTariffId)
        {
            string Name = string.Empty;
            if (serviceTariffId != null)
            {
                var ser = UnitOfWork.ServiceTariffRepository.GetById(serviceTariffId.Value)
                 ;
                Name = ser.Name + ser.Amount;
            }
            return Name;
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DetailsInvoiceNumber(int id)
        {
            try
            {
                if (Request.UrlReferrer == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
                }

                else if (Request.UrlReferrer.AbsoluteUri != Infrastructure.WebServiceSetting_Sadad.ImportReffererUrl)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
                }


                else
                {
                    var oRequest =
                     UnitOfWork.RequestRepository.Get()
                     .Where(current => current.InvoiceNumber == id)
                     .FirstOrDefault()
                     ;

                    if (oRequest == null)
                    {
                        return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                    }

                    return View(oRequest);
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Display(Guid id)
        {
            try
            {
                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);
                ViewBag.PageMessages = null;

                var oRequest =
                 UnitOfWork.FactorCementRepository.GetByUser(Infrastructure.Sessions.AuthenticatedUser.User)
                 .Where(current => current.Id == id)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                 {
                     Id = current.Id,
                     InvoiceNumber = current.InvoiceNumber,
                     StringProductName = current.ProductName.Name,
                     StringProductType = current.ProductType.Name,
                     StringPackageType = current.PackageType.Name,
                     StringFactoryName = current.FactoryName.Name,
                     StringTonnage = current.Tonnage.Name,
                     StringProvince = current.Province.Name,
                     StringCity = current.City.Name,
                     BuyerMobile = current.BuyerMobile,
                     AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? current.DestinationAmountPaid.Value : 0,
                     MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                     StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                     Description = current.Description,
                 })
                 .FirstOrDefault();

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                return View(oRequest);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Edit(Guid id)
        {
            CementViewModel CementViewModel
            = UnitOfWork.FactorCementRepository.Get()
            .Where(current => current.Id == id)
            .ToList()
            .Select(current => new CementViewModel()
            {
                Id = current.Id,
                InvoiceNumber = current.InvoiceNumber,
                StringProductName = current.ProductName.Name,
                StringProductType = current.ProductType.Name,
                StringPackageType = current.PackageType.Name,
                StringFactoryName = current.FactoryName.Name,
                StringTonnage = current.Tonnage.Name,
                StringProvince = current.Province.Name,
                StringCity = current.City.Name,
                BuyerMobile = current.BuyerMobile,
                AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? current.DestinationAmountPaid.Value : 0,
                MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                Description = current.Description,
                RemittanceNumber = current.RemittanceNumber
            })
            .FirstOrDefault();


            ViewBag.PageMessages = null;

            return (View(CementViewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.FactorCementRepository
                    .Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault()
                    ;
                // **************************************************
                OlderAccount.InvoiceNumber = cementViewModel.InvoiceNumber;
                OlderAccount.ProductName.Name = cementViewModel.StringProductName;
                OlderAccount.ProductType.Name = cementViewModel.StringProductType;
                OlderAccount.PackageType.Name = cementViewModel.StringPackageType;
                OlderAccount.FactoryName.Name = cementViewModel.StringFactoryName;
                OlderAccount.Tonnage.Name = cementViewModel.StringTonnage;
                OlderAccount.Province.Name = cementViewModel.StringProvince;
                OlderAccount.City.Name = cementViewModel.StringCity;
                OlderAccount.BuyerMobile = cementViewModel.BuyerMobile;
                OlderAccount.AmountPaid = cementViewModel.AmountPaid;
                OlderAccount.MahalTahvil = cementViewModel.MahalTahvil;
                OlderAccount.RemittanceNumber = cementViewModel.RemittanceNumber;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.FactorCementRepository.Update(OlderAccount);
                UnitOfWork.Save();

                // **************************************************
                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ویرایش گردید  ";
                return View(cementViewModel);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult EditIVO(Guid id)
        {
            try
            {
                List<int> roleList = new List<int> { 800, 1000 };
                if (roleList.Contains(Infrastructure.Sessions.AuthenticatedUser.User.Role.Code))
                {
                    ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);

                    ViewBag.PageMessages = null;

                    #region Update Currency Ration
                    var oLastRequest =
                        UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                        .Where(current => current.Id == id)
                        .FirstOrDefault()
                        ;

                    var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oLastRequest.CurrencyCode);

                    #endregion

                    var oRequest =
                     UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                     .Where(current => current.Id == id)
                     .ToList()
                     .Select(current => new ViewModels.Areas.Administrator.Request.EditViewModel()
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem.Name,
                         CompanyName = current.CompanyName,
                         Province = current.Province.Name,
                         CompanyNationalCode = current.CompanyNationalCode,
                         RecordNumber = current.RecordNumber,
                         RecordDate = current.RecordDate,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                         CurrencyValue = current.CurrencyValue,
                         CurrencyRation = current.CurrencyRation,
                         BaseCurrencyValue
                         = (current.BaseCurrencyValue ?? (current.CurrencyValue * current.CurrencyRation))
                         + " " + oCurrency.Name,
                         AmountPaid = current.AmountPaid,
                         RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                         RequestStateCode = current.RequestState,
                         Bank_TraceNo = current.Bank_TraceNo,
                         Bank_ShamsiDate = current.Bank_ShamsiDate,
                         Description = current.Description,
                         DepositNumber = current.DepositNumber,
                         SystemMessage = string.Empty
                     })
                     .FirstOrDefault();

                    if (oRequest == null)
                    {
                        return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                    }

                    return View(oRequest);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual ActionResult GetTransactionReportByPageNumber(Guid id)
        {
            Models.Request oRequest = null;
            Models.Request oRequestNew = null;
            try
            {
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                oRequest =
                    UnitOfWork.RequestRepository.Get()
                    .Where(current => current.Id == id)
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

                else
                {
                    #region Insert Not Payment Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = oRequest.UserId;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText = Resources.Message.Request.Message_NoPaymented;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    #endregion

                    oRequest.RequestState = (int)Enums.RequestStates.PaymentOrder;
                }

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


                return Json
                    (data: oRequestNew,
                    behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);

                //return View(oRequestNew);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult UpAmountPaid(Guid id)
        {
            try
            {
                List<int> roleList = new List<int> { 800, 1000 };
                if (roleList.Contains(Infrastructure.Sessions.AuthenticatedUser.User.Role.Code))
                {
                    ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);

                    ViewBag.PageMessages = null;

                    #region Update Currency Ration
                    var oLastRequest =
                        UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                        .Where(current => current.Id == id)
                        .FirstOrDefault()
                        ;

                    var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oLastRequest.CurrencyCode);

                    if (oLastRequest.RequestState <= (int)Enums.RequestStates.PaymentOrder)
                    {
                        oLastRequest.CurrencyRation = oCurrency.Ratio;
                        oLastRequest.AmountPaid = Convert.ToInt64(oLastRequest.AmountPaid);

                        //UnitOfWork.RequestRepository.Update(oLastRequest);
                        //UnitOfWork.Save();
                    }
                    #endregion

                    var oRequest =
                     UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                     .Where(current => current.Id == id)
                     .ToList()
                     .Select(current => new ViewModels.Areas.Administrator.Request.UpAmountPaidViewModel()
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem.Name,
                         //+ ((current.SecNumber != null && current.SecNumber == "00000") ? (" - " + "[تبصره 23]") : string.Empty),

                         CompanyName = current.CompanyName,
                         CompanyNationalCode = current.CompanyNationalCode,

                         RecordNumber = current.RecordNumber,
                         RecordDate = current.RecordDate,

                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),

                         AmountPaid = current.AmountPaid,
                         RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),

                         Bank_TraceNo = current.Bank_TraceNo,
                         Bank_ShamsiDate = current.Bank_ShamsiDate,
                         Description = current.Description,
                         Bank_RefrenceNumber = current.Bank_RefrenceNumber

                     })
                     .FirstOrDefault();

                    if (oRequest == null)
                    {
                        return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                    }

                    return View(oRequest);
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult UpAmountPaid(ViewModels.Areas.Administrator.Request.UpAmountPaidViewModel request, string submit)
        {
            try
            {
                string RetMessage = string.Empty;

                var oRequest =
                    UnitOfWork.RequestRepository.GetById(request.Id)
                    ;

                if (submit == Resources.Action.ManulOrder)
                {
                    #region صدور دستور پرداخت

                    oRequest.RequestState = 1;
                    var OldAmount = oRequest.AmountPaid;
                    var NewAmount = request.AmountPaid;

                    oRequest.AmountPaid = request.AmountPaid;

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.MessageText =
                        "بروزرسانی قیمت به " + request.AmountPaid + " و صدور دستور پرداخت"
                        + " - " + request.SystemMessage;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                    oMessage.RequestId = oRequest.Id;
                    if (OldAmount != NewAmount)
                    {
                        UnitOfWork.RequestRepository.SetControlCode(oRequest);// برای تغیر شناسه موقع تغیر مبلغ
                    }
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion

                    #endregion
                }


                if (submit == Resources.Action.ManulPayment)
                {
                    #region ثبت اطلاعات پرداخت و تایید نهایی پرداخت

                    oRequest.RequestState = 3;
                    oRequest.Bank_AppStatus = 3;
                    oRequest.Bank_AppStatusDescription = "COMMIT";
                    oRequest.Bank_AppStatusCode = 0;
                    oRequest.Bank_RefrenceNumber = request.Bank_RefrenceNumber;
                    oRequest.Bank_ShamsiDate = request.Bank_ShamsiDate;
                    oRequest.Bank_TraceNo = request.Bank_TraceNo;
                    oRequest.Bank_BankReciptNumber = request.Bank_ShamsiDate + request.Bank_TraceNo;
                    // جلو گیری از ایجاد شناسه جدید موقع تایید نهایی
                    // UnitOfWork.RequestRepository.Update(oRequest);
                    ViewBag.PageMessages += " تایید پرداخت برای این درخواست در سیستم ثبت شد.";

                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oMessage.LastState = oRequest.RequestState;
                    oMessage.MessageText =
                        Resources.Message.Request.Message_PaymentConfirmation
                        + " - " + request.SystemMessage;
                    oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion

                    #endregion
                }

                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(request.Id);
                ViewBag.PageMessages = null;

                #region Refresh View
                var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);

                var oNewRequest =
                    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                    .Where(current => current.Id == oRequest.Id)
                    .ToList()
                    .Select(current => new ViewModels.Areas.Administrator.Request.UpAmountPaidViewModel()
                    {
                        Id = current.Id,
                        SubSystem = current.SubSystem.Name,
                        CompanyName = current.CompanyName,
                        CompanyNationalCode = current.CompanyNationalCode,
                        RecordNumber = current.RecordNumber,
                        RecordDate = current.RecordDate,
                        InvoiceNumber = current.InvoiceNumber,
                        InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                        CurrencyValue = current.CurrencyValue,
                        CurrencyRation = current.CurrencyRation,
                        BaseCurrencyValue = (current.BaseCurrencyValue ?? (current.CurrencyValue * current.CurrencyRation)) + " " + oCurrency.Name,
                        AmountPaid = current.AmountPaid,
                        RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                        RequestStateCode = current.RequestState,
                        Bank_TraceNo = current.Bank_TraceNo,
                        Bank_ShamsiDate = current.Bank_ShamsiDate,
                        Bank_RefrenceNumber = current.Bank_RefrenceNumber
                    })
                     .FirstOrDefault();

                if (oNewRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }
                return View(oNewRequest);
                #endregion

            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }



        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Incomplete(string RecordNumber, string SystemMessage)
        {
            try
            {
                Infrastructure.WebServiceClass oWebServiceClass = new Infrastructure.WebServiceClass();
                string RetMessage = string.Empty;
                ViewBag.PageMessages = null;

                #region Change State in OPS
                var oRequest =
                    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                    .Where(current => current.RecordNumber == RecordNumber)
                    .FirstOrDefault();


                oRequest.RequestState = (int)Enums.RequestStates.Incomplete;
                UnitOfWork.RequestRepository.Update(oRequest);
                RetMessage = " اعلام نواقصی در سیستم پرداخت ثبت شد.";
                RetMessage += "<br/>";
                UnitOfWork.Save();
                #endregion

                #region Chage State in IVO By Webservice
                RetMessage += oWebServiceClass.Insert_Incomplete(RecordNumber, SystemMessage);
                #endregion

                ViewBag.PageMessages = RetMessage;

                var oNewRequest =
                  UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                  .Where(current => current.RecordNumber == RecordNumber)
                  .ToList()
                  .Select(current => new ViewModels.Areas.Administrator.Request.EditViewModel()
                  {
                      Id = current.Id,
                      SubSystem = current.SubSystem.Name,
                      CompanyName = current.CompanyName,
                      CompanyNationalCode = current.CompanyNationalCode,
                      RecordNumber = current.RecordNumber,
                      RecordDate = current.RecordDate,
                      InvoiceNumber = current.InvoiceNumber,
                      InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                      CurrencyValue = current.CurrencyValue,
                      CurrencyRation = current.CurrencyRation,
                      BaseCurrencyValue = current.BaseCurrencyValue.ToString(),
                      AmountPaid = current.AmountPaid,
                      RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                      RequestStateCode = current.RequestState,
                      Bank_TraceNo = current.Bank_TraceNo,
                      Bank_ShamsiDate = current.Bank_ShamsiDate,
                      SystemMessage = string.Empty
                  })
                  .FirstOrDefault();

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                return View(oRequest);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult EditIVO(ViewModels.Areas.Administrator.Request.EditViewModel request)
        {
            try
            {
                ViewBag.PageMessages = null;
                string RetMessage = string.Empty;
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();

                var oRequest =
                    UnitOfWork.RequestRepository.GetById(request.Id)
                    ;

                OPS.ReportIVO.OPSReportSoapClient oService = new ReportIVO.OPSReportSoapClient();

                oService.GetOPSDetailes("OPS", "Xx12345", oRequest.InvoiceNumber, request.Bank_TraceNo.Value.ToString(),
                    request.AmountPaid, request.Bank_ShamsiDate, out RetMessage);


                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                oMessage.LastState = oRequest.RequestState;

                oMessage.MessageText = "ثبت پرداخت مجدد فوب" + " || " + request.Bank_TraceNo.Value.ToString()
                    + " || " + request.AmountPaid + " || " + request.Bank_ShamsiDate + " - " + request.SystemMessage;

                oMessage.NewState = oRequest.RequestState;
                oMessage.RequestId = oRequest.Id;
                UnitOfWork.MessageRepository.Insert(oMessage);
                #endregion

                UnitOfWork.Save();

                #region Refresh View
                ViewBag.PageMessages = RetMessage;

                var oNewRequest =
                  UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                  .Where(current => current.RecordNumber == oRequest.RecordNumber)
                  .ToList()
                  .Select(current => new ViewModels.Areas.Administrator.Request.EditViewModel()
                  {
                      Id = current.Id,
                      SubSystem = current.SubSystem.Name,
                      CompanyName = current.CompanyName,
                      CompanyNationalCode = current.CompanyNationalCode,
                      RecordNumber = current.RecordNumber,
                      RecordDate = current.RecordDate,
                      InvoiceNumber = current.InvoiceNumber,
                      InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                      CurrencyValue = current.CurrencyValue,
                      CurrencyRation = current.CurrencyRation,
                      BaseCurrencyValue = current.BaseCurrencyValue.ToString(),
                      AmountPaid = current.AmountPaid,
                      RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                      RequestStateCode = current.RequestState,
                      Bank_TraceNo = current.Bank_TraceNo,
                      Bank_ShamsiDate = current.Bank_ShamsiDate,
                      SystemMessage = string.Empty
                  })
                  .FirstOrDefault();

                if (oNewRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                return View(oNewRequest);
                #endregion
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DetailsByTracingCode(string tracingcode)
        {
            try
            {
                var oRequest =
                 UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                 .Where(current => current.Bank_BankReciptNumber == tracingcode)
                 .FirstOrDefault()
                 ;

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                return View(oRequest);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult ViewFiles(string Id)
        {
            try
            {
                Guid requestId = new Guid(Id);

                var oFile =
                 UnitOfWork.FileRepository.Get()
                 .Where(current => current.RequestId == requestId)
                 .ToList()
                 ;

                if (oFile.Count > 0)
                {
                    List<Models.File> oFinalFile = new List<Models.File>();
                    foreach (var file in oFile)
                    {
                        Models.File newFile = new Models.File();
                        newFile.Id = file.Id;
                        newFile.FileAddress = file.FileAddress;
                        newFile.InsertDateTime = file.InsertDateTime;
                        newFile.IsActived = file.IsActived;
                        newFile.IsDeleted = file.IsDeleted;
                        newFile.IsSystem = file.IsSystem;
                        newFile.IsVerified = file.IsVerified;
                        newFile.Name = file.Name;
                        newFile.RequestId = file.RequestId;
                        newFile.UpdateDateTime = file.UpdateDateTime;
                        oFinalFile.Add(newFile);
                    }
                    return Json(data: oFinalFile, behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: oFile, behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual FileContentResult Download()
        {
            var fileDownloadName = String.Format("FileName.xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //var varRequest =
            //    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
            //    ;

            DateTime nowDateTime = DateTime.Now.AddDays(-3);
            int PaymentConfirmation = (int)Enums.RequestStates.PaymentConfirmation;

            //var varRequestTypes = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            //ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varRequestTypes, "Id", "Name", null);

            var newDataSource = Infrastructure.Sessions.SearchDataSource as List<ViewModels.Areas.Administrator.Request.IndexViewModel>;

            // Pass your ef data to method
            ExcelPackage package = GenerateExcelFile(newDataSource);

            var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }

        private static ExcelPackage GenerateExcelFile(List<ViewModels.Areas.Administrator.Request.IndexViewModel> datasource)
        {
            //*****************************************************//
            //*****************************************************//
            //http://www.codeproject.com/Articles/680421/Create-Read-Edit-Advance-Excel-Report-in
            //https://epplus.codeplex.com/wikipage?title=FAQ
            //*****************************************************//
            //*****************************************************//

            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Application = "OPS.IVO.IR";
            excelPackage.Workbook.Properties.Author = Infrastructure.Sessions.AuthenticatedUser.UserName;
            excelPackage.Workbook.Properties.Comments = (new Infrastructure.Calander(DateTime.Now)).Persion();
            excelPackage.Workbook.Properties.Subject = "گزارشات مالی";
            excelPackage.Workbook.Properties.Title = "گزارشات مالی";
            //excelPackage.Workbook.Properties.SetCustomPropertyValue("IsRightToLeft",true);
            //var dfsbgfg1 = excelPackage.Workbook.Properties.GetCustomPropertyValue("IsRightToLeft");

            //Create the worksheet 
            ExcelWorksheet WorkSheet = excelPackage.Workbook.Worksheets.Add("گزارشات مالی");
            WorkSheet.Cells.AutoFitColumns(100, 400);
            WorkSheet.Cells[1, 1].Value = "گزارشات مالی سیستم پرداخت آنلاین سازمان دامپزشکی کل کشور";
            WorkSheet.Cells[1, 1, 1, 12].Merge = true;

            // Sets Headers
            WorkSheet.Cells[2, 1].Value = Resources.Model.Request.SubSystem;
            WorkSheet.Cells[2, 2].Value = Resources.Model.Request.CompanyName;
            WorkSheet.Cells[2, 3].Value = Resources.Model.Request.ServiceTariff;
            WorkSheet.Cells[2, 4].Value = Resources.Model.Request.CompanyNationalCode;
            WorkSheet.Cells[2, 5].Value = Resources.Model.Request.InvoiceNumber;
            WorkSheet.Cells[2, 6].Value = Resources.Model.Request.InvoiceDate;
            WorkSheet.Cells[2, 7].Value = Resources.Model.Request.RecordNumber;
            WorkSheet.Cells[2, 8].Value = Resources.Model.Request.RecordDate;
            WorkSheet.Cells[2, 9].Value = Resources.Model.Request.RequestState;
            WorkSheet.Cells[2, 10].Value = Resources.Model.Request.Province;
            WorkSheet.Cells[2, 11].Value = Resources.Model.Request.AmountPaid;
            WorkSheet.Cells[2, 12].Value = Resources.Model.Request.City;
            WorkSheet.Cells[2, 13].Value = Resources.Model.Request.DepositNumber;
            WorkSheet.Cells[2, 14].Value = Resources.Model.Request.BaseCurrencyValue;
            WorkSheet.Cells[2, 15].Value = Resources.Model.Request.CurrencyUnit;
            WorkSheet.Cells[2, 16].Value = Resources.Model.Request.Bank_TraceNo;
            WorkSheet.Cells[2, 17].Value = " کاربر سازنده ";
            WorkSheet.Cells[2, 18].Value = Resources.Model.Request.AmountPaidDate;
            #region Cell Borders
            WorkSheet.Cells[2, 1].Style.Border.Top.Style
                = WorkSheet.Cells[2, 1].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 1].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 2].Style.Border.Top.Style
                = WorkSheet.Cells[2, 2].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 2].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 3].Style.Border.Top.Style
                = WorkSheet.Cells[2, 3].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 3].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 4].Style.Border.Top.Style
                = WorkSheet.Cells[2, 4].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 4].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 5].Style.Border.Top.Style
                = WorkSheet.Cells[2, 5].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 5].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 6].Style.Border.Top.Style
                = WorkSheet.Cells[2, 6].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 6].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 7].Style.Border.Top.Style
                = WorkSheet.Cells[2, 7].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 7].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 8].Style.Border.Top.Style
                = WorkSheet.Cells[2, 8].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 8].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 9].Style.Border.Top.Style
                = WorkSheet.Cells[2, 9].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 9].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 10].Style.Border.Top.Style
                = WorkSheet.Cells[2, 10].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 10].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 11].Style.Border.Top.Style
                = WorkSheet.Cells[2, 11].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 11].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


            WorkSheet.Cells[2, 12].Style.Border.Top.Style
                = WorkSheet.Cells[2, 12].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 12].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 13].Style.Border.Top.Style
                = WorkSheet.Cells[2, 13].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 13].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 14].Style.Border.Top.Style
                = WorkSheet.Cells[2, 14].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 14].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 15].Style.Border.Top.Style
                = WorkSheet.Cells[2, 15].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 15].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 16].Style.Border.Top.Style
                = WorkSheet.Cells[2, 16].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 16].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 17].Style.Border.Top.Style
                = WorkSheet.Cells[2, 17].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 17].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 18].Style.Border.Top.Style
                = WorkSheet.Cells[2, 18].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 18].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            #endregion


            // Inserts Data
            for (int i = 0; i < datasource.Count(); i++)
            {
                #region Row Value
                WorkSheet.Cells[i + 3, 1].Value = datasource.ElementAt(i).SubSystem;
                WorkSheet.Cells[i + 3, 2].Value = datasource.ElementAt(i).CompanyName;
                WorkSheet.Cells[i + 3, 3].Value = datasource.ElementAt(i).ServiceTariff;
                WorkSheet.Cells[i + 3, 4].Value = datasource.ElementAt(i).CompanyNationalCode;
                WorkSheet.Cells[i + 3, 5].Value = datasource.ElementAt(i).InvoiceNumber;
                WorkSheet.Cells[i + 3, 6].Value = datasource.ElementAt(i).InvoiceDate;
                WorkSheet.Cells[i + 3, 7].Value = datasource.ElementAt(i).RecordNumber;
                WorkSheet.Cells[i + 3, 8].Value = datasource.ElementAt(i).RecordDate;
                WorkSheet.Cells[i + 3, 9].Value = datasource.ElementAt(i).RequestState;
                WorkSheet.Cells[i + 3, 10].Value = datasource.ElementAt(i).Province;
                WorkSheet.Cells[i + 3, 11].Value = datasource.ElementAt(i).AmountPaid;
                WorkSheet.Cells[i + 3, 12].Value = datasource.ElementAt(i).City;
                WorkSheet.Cells[i + 3, 13].Value = datasource.ElementAt(i).DepositNumber;
                WorkSheet.Cells[i + 3, 14].Value = datasource.ElementAt(i).BaseCurrencyValue;
                WorkSheet.Cells[i + 3, 15].Value = datasource.ElementAt(i).CurrencyUnit;
                WorkSheet.Cells[i + 3, 16].Value = datasource.ElementAt(i).Bank_TraceNo;
                WorkSheet.Cells[i + 3, 17].Value = datasource.ElementAt(i).CeratedBy;
                WorkSheet.Cells[i + 3, 18].Value = datasource.ElementAt(i).AmountPaidDate;

                #endregion

                #region Cell Borders
                WorkSheet.Cells[i + 3, 1].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 1].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 1].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 2].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 2].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 2].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 3].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 3].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 3].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 4].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 4].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 4].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 5].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 5].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 5].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 6].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 6].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 6].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 7].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 7].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 7].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 8].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 8].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 8].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 9].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 9].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 9].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 10].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 10].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 10].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 11].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 11].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 11].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 12].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 12].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 12].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 13].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 13].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 13].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 14].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 14].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 14].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 15].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 15].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 15].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 16].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 16].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 16].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 17].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 17].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 17].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 18].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 18].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 18].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion
            }


            WorkSheet.Column(1).AutoFit(25);
            WorkSheet.Column(2).AutoFit(30);
            WorkSheet.Column(3).AutoFit(30);
            WorkSheet.Column(4).AutoFit(15);
            WorkSheet.Column(5).AutoFit(15);
            WorkSheet.Column(6).AutoFit(15);
            WorkSheet.Column(7).AutoFit(15);
            WorkSheet.Column(8).AutoFit(15);
            WorkSheet.Column(9).AutoFit(20);
            WorkSheet.Column(10).AutoFit(20);
            WorkSheet.Column(11).AutoFit(20);
            WorkSheet.Column(12).AutoFit(20);
            WorkSheet.Column(13).AutoFit(20);
            WorkSheet.Column(14).AutoFit(20);
            WorkSheet.Column(15).AutoFit(20);
            WorkSheet.Column(16).AutoFit(20);
            WorkSheet.Column(17).AutoFit(20);
            WorkSheet.Column(18).AutoFit(20);

            // Format Header of Table
            using (ExcelRange rng = WorkSheet.Cells["A1:I1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }

            using (ExcelRange rng = WorkSheet.Cells["A2:Q2"])

            {
                rng.Style.Font.Bold = true;
                //WorkSheet.Cells["A2:L2"].AutoFilter = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }
            return excelPackage;
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetCities(System.Guid provinceId)
        {
            try
            {
                var cities =
                 UnitOfWork.CityRepository.GetByProvinceId(provinceId)
                 .Select(x => new
                 {
                     Name = x.Name,
                     Id = x.Id
                 })
                 .ToList()
                 ;

                return Json
                    (data: cities,
                    behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintNewFactor(int invoiceNumber)
        {
            try
            {
                var factorCement =
                UnitOfWork.FactorCementRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();

                ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
                cementViewModel.Id = factorCement.Id;
                cementViewModel.InvoiceNumber = factorCement.InvoiceNumber;
                cementViewModel.StringInsertDateTime = new Infrastructure.Calander(factorCement.InsertDateTime).Persion();
                cementViewModel.BuyerMobile = factorCement.BuyerMobile;
                cementViewModel.BuyerName = factorCement.User.FullName;
                cementViewModel.BuyerNationalCode = factorCement.User.NationalCode;
                cementViewModel.StringProvince = factorCement.Province.Name;
                cementViewModel.StringCity = factorCement.City.Name;
                cementViewModel.StringProductName = factorCement.ProductName.Name;
                cementViewModel.StringProductType = factorCement.ProductType.Name;
                cementViewModel.StringPackageType = factorCement.PackageType.Name;
                cementViewModel.StringFactoryName = factorCement.FactoryName.Name;
                cementViewModel.StringTonnage = factorCement.Tonnage.Name;
                cementViewModel.AmountPaid = factorCement.AmountPaid;
                cementViewModel.Address = factorCement.Address;
                cementViewModel.RemittanceNumber = factorCement.RemittanceNumber;
                cementViewModel.DestinationAmountPaid = factorCement.DestinationAmountPaid != null ? factorCement.DestinationAmountPaid.Value : 0;
                cementViewModel.MahalTahvil = factorCement.MahalTahvil == "Karkhane" ? "درب کارخانه" : factorCement.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ";
                cementViewModel.ref_id = factorCement.ref_id.ToString();
                cementViewModel.card_pan = factorCement.card_pan;

                var File = new Rotativa.MVC.ViewAsPdf("PrintNewFactor", cementViewModel)
                {
                    FileName = factorCement.InvoiceNumber + ".pdf"
                };
                return File;
            }

            catch (Exception ex)
            {
                return null;
            }
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintNewFactorWallet(int invoiceNumber)
        {
            try
            {
                var factorCement =
                UnitOfWork.walletFactorRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();

                WalletViewModel factor = new WalletViewModel();
                factor.Id = factorCement.Id;
                factor.InvoiceNumber = factorCement.InvoiceNumber;
                factor.StringInsertDateTime = factorCement.InsertDateTime.ToString();
                factor.BuyerName = factorCement.User.FullName;
                factor.BuyerMobile = factorCement.BuyerMobile;
                factor.AmountPaid = factorCement.Chargeamount.ToString();
                factor.ref_id = factorCement.ref_id.ToString();
                factor.card_pan = factorCement.card_pan;

                var File = new Rotativa.MVC.ViewAsPdf("PrintNewFactorWallet", factor)
                {
                    FileName = factorCement.InvoiceNumber + ".pdf"
                };
                return File;
            }

            catch (Exception ex)
            {
                return null;
            }
        }



        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintDepositNumber(int invoiceNumber)
        {
            Models.Request oRequest = null;
            try
            {
                List<Models.DetailOfFactor> list = null;

                oRequest =
                UnitOfWork.RequestRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();


                if (oRequest?.HeadOfFactor != null)
                {
                    var headoffactorid =
                        oRequest.HeadOfFactor.Id;

                    var headRow =
                         UnitOfWork.HeadOfFactorRepository.Get()
                         .Where(current => current.Id == headoffactorid)
                         .FirstOrDefault()
                         ;
                    ViewBag.headRow = headRow;


                    list =
                        UnitOfWork.DetailOfFactorRepository.Get(headoffactorid)
                        .ToList();

                    var File = new Rotativa.MVC.ViewAsPdf("PrintFactor", list)
                    {
                        FileName = list.FirstOrDefault().HeadOfFactor.CompanyName + "-" + list.FirstOrDefault().HeadOfFactor.InvoiceNumber + ".pdf"
                    };
                    return File;
                }
                else
                {
                    oRequest =
                        UnitOfWork.RequestRepository.Get()
                        .Where(current => current.InvoiceNumber == invoiceNumber)
                        .FirstOrDefault();

                    //Use ViewAsPdf Class to generate pdf using GeneratePDF.cshtml view
                    var File = new Rotativa.MVC.ViewAsPdf("PrintDepositNumber", oRequest) { FileName = "PrintDepositNumber.pdf" };
                    return File;
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }


        [HttpPost]
        public virtual JsonResult AddPayment(PCPOS_Res data, Guid Id)
        {
            var pcPosPaymentResult = new PcPosPaymentResult();

            foreach (var item in data.resp_tlv.children)
            {
                switch (item.Tag)
                {
                    case "TM":
                        pcPosPaymentResult.PosNumber = item.Value;
                        break;
                    case "PN":
                        pcPosPaymentResult.CartNumber = item.Value;
                        break;
                    case "SR":
                        pcPosPaymentResult.Serial = item.Value + "/" + pcPosPaymentResult.Serial;
                        break;
                    case "TR":
                        pcPosPaymentResult.Serial = item.Value;
                        pcPosPaymentResult.Bank_TraceNo = item.Value;
                        break;
                    case "TI":
                        pcPosPaymentResult.Date = item.Value;
                        break;
                    case "RN":
                        pcPosPaymentResult.Bank_BankReciptNumber = item.Value;
                        break;
                }
            }

            var request = UnitOfWork.RequestRepository.GetById(Id);
            request.Bank_BankReciptNumber = pcPosPaymentResult.Bank_BankReciptNumber;
            request.Bank_ShamsiDate = (pcPosPaymentResult.Date).ToString().Substring(0, 15);
            request.Bank_CardHolderAccNumber = pcPosPaymentResult.CartNumber;
            request.Bank_CustomerCardNumber = pcPosPaymentResult.CartNumber;
            request.Bank_Terminal = pcPosPaymentResult.PosNumber;
            request.AmountPaidDate = DateTime.Now;
            // request.Bank_BankReciptNumber = pcPosPaymentResult.Serial;
            //      request.Bank_TraceNo = Convert.ToInt64(Convert.ToDecimal(pcPosPaymentResult.Serial));
            request.Bank_TraceNo = Convert.ToInt64(Convert.ToDecimal(pcPosPaymentResult.Bank_TraceNo));
            request.RequestState = (int)Enums.RequestStates.PaymentConfirmation;
            UnitOfWork.RequestRepository.Update(request);
            UnitOfWork.Save();
            try
            {
                #region Insert New Message
                Models.Message oMessage = new Models.Message();
                oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                oMessage.MessageText =
                    Resources.Message.Request.PaymentConfirmation;
                oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                oMessage.RequestId = request.Id;
                UnitOfWork.MessageRepository.Insert(oMessage);
                UnitOfWork.Save();
                #endregion
            }
            catch
            {
            }


            return new JsonResult { Data = new { success = "true" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

    public class PCPOS_Res
    {
        public BTLV resp_tlv { get; set; }
        public int resp_code { get; set; }
        public string resp_msg { get; set; }
        public string resp_tagID { get; set; }

    }

    public class BTLV
    {
        public List<BTLV> children { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }
    }

    public class PcPosPaymentResult
    {
        public string PosNumber { get; set; }
        public string CartNumber { get; set; }
        public string Serial { get; set; }
        public string Date { get; set; }
        public string Bank_BankReciptNumber { get; set; }
        public string Bank_TraceNo { get; set; }
        //public string Price { get; set; }
    }
}