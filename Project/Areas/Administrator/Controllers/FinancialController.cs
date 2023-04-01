using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
//using KendoGridExportExcelMvc.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class FinancialController : Infrastructure.BaseControllerWithUnitOfWork
    {

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual ActionResult Index()
        {
            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual ActionResult Index2()
        {

            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", null);

            //var varServiceTariffs = UnitOfWork.ServiceTariffRepository.Get().OrderBy(current => current.Name).ToList();
            //ViewData["ServiceTariffs"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "Name", null);

            var varServiceTariffs = UnitOfWork.ServiceTariffRepository.Get()
                .ToList()
                .Select(x => new ViewModels.ComboboxItemGuid
                {
                    Id = x.Id,
                    Name = x.NameString
                })
                .OrderBy(current => current.Name)
                .ToList();
            ViewBag.ServiceTariffs = varServiceTariffs;

            var varRequestStates = Infrastructure.Utility.EnumList(Enums.EnumTypes.RequestStates);
            ViewData["RequestState"] = new System.Web.Mvc.SelectList(varRequestStates, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual System.Web.Mvc.ActionResult CompanyOption(ViewModels.Areas.Administrator.Request.SearchViewModel viewModel)
        {
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                .Where(current => current.RequestState == viewModel.RequestState)
                //.Where(current => current.Bank_AppStatus == 3)
                .Where(current => current.Bank_AppStatusCode == 0)
                .Where(x => x.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23 ? x.AmountPaid > 0 : true)
                .Where(current => current.Bank_AppStatusDescription == "COMMIT")
                ;

            #region Condition
            viewModel.CompanyName = Utilities.Text.Utility.FixText(viewModel.CompanyName);
            viewModel.CompanyNationalCode = Utilities.Text.Utility.FixText(viewModel.CompanyNationalCode);
            viewModel.RecordNumber = Utilities.Text.Utility.FixText(viewModel.RecordNumber);
            viewModel.CommodityType = Utilities.Text.Utility.FixText(viewModel.CommodityType);

            if (viewModel.CompanyName != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyName.Contains(viewModel.CompanyName))
                    ;
            }

            if (viewModel.CommodityType != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CommodityType.Contains(viewModel.CommodityType))
                    ;
            }

            if (viewModel.CompanyNationalCode != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyNationalCode.Contains(viewModel.CompanyNationalCode))
                    ;
            }

            if (viewModel.RecordNumber != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.RecordNumber.Contains(viewModel.RecordNumber))
                    ;
            }

            if (viewModel.InvoiceNumber.HasValue)
            {
                varRequest =
                    varRequest
                    .Where(current => current.InvoiceNumber == viewModel.InvoiceNumber.Value)
                    ;
            }

            if (viewModel.RequestState.HasValue)
            {
                varRequest = varRequest.Where(current => current.RequestState == viewModel.RequestState);
            }


            if (viewModel.SubSystem != null && viewModel.SubSystem != new Guid())
            {
                //if (viewModel.SubSystem == new Guid("00000000-0000-0000-0000-000000000001"))
                //    varRequest = varRequest.Where(current => current.SecNumber == "00000");

                //else
                varRequest = varRequest.Where(current => current.SubSystemId == viewModel.SubSystem);
            }

            if (viewModel.Province != null && viewModel.Province != new Guid())
            {
                varRequest = varRequest.Where(current => current.ProvinceId == viewModel.Province);
            }

            if (viewModel.City != null && viewModel.City != new Guid())
            {
                varRequest = varRequest.Where(current => current.CityId == viewModel.City);
            }

            #endregion

            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", viewModel.SubSystem);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            var ViewModelsvarRequest
                 = varRequest
                 .OrderBy(current => current.CompanyName)
                 .ThenByDescending(current => current.Bank_RealTransactionDateTime)
                 .ToList()
                 .Select(current =>
                     new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem.Name,
                         CompanyName = current.CompanyName,
                         CommodityType = current.CommodityType,
                         TotalValue = current.TotalValue,
                         CompanyNationalCode = current.CompanyNationalCode,
                         Province = current.Province.Name,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                         RecordNumber = current.RecordNumber,
                         RecordDate = current.RecordDate,
                         PerformNumber = current.PerformNumber,
                         PerformDate = current.PerformDate,
                         CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                         CurrencyValue = current.CurrencyValue,
                         AmountPaid = current.AmountPaid,
                         Bank_TraceNo = current.Bank_TraceNo.Value.ToString(),
                         Bank_BankReciptNumber = current.Bank_BankReciptNumber.Substring(8),
                         Bank_ShamsiDate = current.Bank_ShamsiDate != null ? (current.Bank_ShamsiDate.Substring(0, 4)
                         + "/" + current.Bank_ShamsiDate.Substring(4, 2)
                         + "/" + current.Bank_ShamsiDate.Substring(6, 2)) : "[تاریخ ندارد]",
                         Tarefeh=current.Tariffs!=null?current.Tariffs.Value:0,
                         SystemTarefeh= current.ServiceTariff!=null?Convert.ToInt32(current.ServiceTariff.Amount):0,
                         LisenceNumber=current.LicenseNumber,
                         LicenseDate= current.LicenseDate!=null?new Infrastructure.Calander(current.LicenseDate.Value).Persion():"[نا مشخص]"
                     })
                     .ToList()
                     .Select(current =>
                     new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem,
                         CompanyName = current.CompanyName,
                         CommodityType = current.CommodityType,
                         TotalValue = current.TotalValue,
                         Province = current.Province,
                         CompanyNationalCode = current.CompanyNationalCode,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = current.InvoiceDate,
                         RecordNumber = current.RecordNumber,
                         RecordDate = current.RecordDate,
                         PerformNumber = current.PerformNumber,
                         PerformDate = current.PerformDate,
                         CurrencyCode = current.CurrencyCode,
                         CurrencyValue = current.CurrencyValue,
                         AmountPaid = current.AmountPaid,
                         Bank_TraceNo = current.Bank_TraceNo,
                         Bank_BankReciptNumber = current.Bank_BankReciptNumber,
                         Bank_ShamsiDate = current.Bank_ShamsiDate,
                         SystemTarefeh = current.SystemTarefeh,
                         Tarefeh = current.Tarefeh,
                         LisenceNumber = current.LisenceNumber,
                         LicenseDate = current.LicenseDate
                     })
                     .AsQueryable();

            object dataSource;

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel>(ViewModelsvarRequest, true, out dataSource);

            Infrastructure.Sessions.SearchDataSource = dataSource;

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual System.Web.Mvc.ActionResult RequestOption(ViewModels.Areas.Administrator.Request.SearchViewModel viewModel)
        {
            try
            {
                System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

                var varRequest =
                    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                    //.Where(current => current.Bank_AppStatus == 3)
                    .Where(current => current.Bank_AppStatusCode == 0)
                    .Where(x => x.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23 ? x.AmountPaid > 0 : true)
                    .Where(current => current.Bank_AppStatusDescription == "COMMIT")
                    ;

                #region Condition
                viewModel.CompanyName = Utilities.Text.Utility.FixText(viewModel.CompanyName);
                viewModel.CompanyNationalCode = Utilities.Text.Utility.FixText(viewModel.CompanyNationalCode);
                viewModel.RecordNumber = Utilities.Text.Utility.FixText(viewModel.RecordNumber);

                if (viewModel.CompanyName != string.Empty)
                {
                    varRequest =
                        varRequest
                        .Where(current => current.CompanyName.Contains(viewModel.CompanyName))
                        ;
                }

                if (viewModel.CommodityType != null && viewModel.CommodityType != string.Empty)
                {
                    varRequest =
                        varRequest
                        .Where(current => current.CommodityType.Contains(viewModel.CommodityType))
                        ;
                }

                if (viewModel.CompanyNationalCode != string.Empty)
                {
                    varRequest =
                        varRequest
                        .Where(current => current.CompanyNationalCode.Contains(viewModel.CompanyNationalCode))
                        ;
                }

                if (viewModel.RecordNumber != string.Empty)
                {
                    varRequest =
                        varRequest
                        .Where(current => current.RecordNumber.Contains(viewModel.RecordNumber))
                        ;
                }

                if (viewModel.InvoiceNumber.HasValue)
                {
                    varRequest =
                        varRequest
                        .Where(current => current.InvoiceNumber == viewModel.InvoiceNumber.Value)
                        ;
                }

                if (viewModel.RequestState.HasValue)
                {
                    varRequest = varRequest.Where(current => current.RequestState == viewModel.RequestState);
                }


                if (viewModel.SubSystem != null && viewModel.SubSystem != new Guid())
                {
                    //if (viewModel.SubSystem == new Guid("00000000-0000-0000-0000-000000000001"))
                    //    varRequest = varRequest.Where(current => current.SecNumber == "00000");

                    //else
                    varRequest = varRequest.Where(current => current.SubSystemId == viewModel.SubSystem);
                }

                if (viewModel.Province != null && viewModel.Province != new Guid())
                {
                    varRequest = varRequest.Where(current => current.ProvinceId == viewModel.Province);
                }

                if (viewModel.City != null && viewModel.City != new Guid())
                {
                    varRequest = varRequest.Where(current => current.CityId == viewModel.City);
                }

                if (viewModel.StartDate.HasValue)
                {
                    var NewDate = new Infrastructure.Calander(viewModel.StartDate.Value)
                        .Persion()
                        .Replace("/", "");

                    varRequest = varRequest
                        .Where(current => current.Bank_RealTransactionDateTime != null
                            ? current.Bank_RealTransactionDateTime >= viewModel.StartDate
                            : string.Compare(current.Bank_ShamsiDate, NewDate) >= 0)
                            .ToList()
                            .AsQueryable()
                        ;
                }

                if (viewModel.EndDate.HasValue)
                {
                    var NewDate = new Infrastructure.Calander(viewModel.EndDate.Value)
                        .Persion()
                        .Replace("/", "");

                    varRequest = varRequest
                        .Where(current => current.Bank_RealTransactionDateTime != null
                            ? current.Bank_RealTransactionDateTime <= viewModel.EndDate
                            : string.Compare(current.Bank_ShamsiDate, NewDate) <= 0)
                            .ToList()
                            .AsQueryable()
                        ;
                }

                #endregion

                var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
                ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", viewModel.SubSystem);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);


                var ViewModelsvarRequest12
                     = varRequest
                     .OrderBy(current => current.CompanyName)
                     .ThenByDescending(current => current.Bank_RealTransactionDateTime)
                     .ToList()
                     ;


                var ViewModelsvarRequest
                     = varRequest
                     .OrderBy(current => current.CompanyName)
                     .ThenByDescending(current => current.Bank_RealTransactionDateTime)
                     .ToList()
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                         {
                             Id = current.Id,
                             SubSystem = current.SubSystem.Name,
                             CompanyName = current.CompanyName,
                             CommodityType = current.CommodityType,
                             TotalValue = current.TotalValue,
                             CompanyNationalCode = current.CompanyNationalCode,
                             Province = current.Province.Name,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             PerformNumber = current.PerformNumber,
                             PerformDate = current.PerformDate,
                             CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                             CurrencyValue = current.CurrencyValue,
                             AmountPaid = current.AmountPaid,
                             Bank_TraceNo = current.Bank_TraceNo != null ? current.Bank_TraceNo.Value.ToString() : "پرداخت دستی",
                             Bank_BankReciptNumber = current.Bank_BankReciptNumber.Length > 8
                             ? current.Bank_BankReciptNumber.Substring(8) : current.Bank_BankReciptNumber,
                             Bank_ShamsiDate = (current.Bank_ShamsiDate != null && current.Bank_ShamsiDate.Length >= 8) ? (current.Bank_ShamsiDate.Substring(0, 4)
                             + "/" + current.Bank_ShamsiDate.Substring(4, 2)
                             + "/" + current.Bank_ShamsiDate.Substring(6, 2)) : "[تاریخ ندارد]",
                             Tarefeh = current.Tariffs != null ? current.Tariffs.Value : 0,
                             SystemTarefeh = current.ServiceTariff != null ? Convert.ToInt32(current.ServiceTariff.Amount) : 0,
                             LisenceNumber = current.LicenseNumber,
                             LicenseDate = current.LicenseDate != null ? new Infrastructure.Calander(current.LicenseDate.Value).Persion() : "[نا مشخص]"
                         })
                         .ToList()
                         .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                         {
                             Id = current.Id,
                             SubSystem = current.SubSystem,
                             CompanyName = current.CompanyName,
                             CommodityType = current.CommodityType,
                             TotalValue = current.TotalValue,
                             Province = current.Province,
                             CompanyNationalCode = current.CompanyNationalCode,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDate = current.InvoiceDate,
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             PerformNumber = current.PerformNumber,
                             PerformDate = current.PerformDate,
                             CurrencyCode = current.CurrencyCode,
                             CurrencyValue = current.CurrencyValue,
                             AmountPaid = current.AmountPaid,
                             Bank_TraceNo = current.Bank_TraceNo,
                             Bank_BankReciptNumber = current.Bank_BankReciptNumber,
                             Bank_ShamsiDate = current.Bank_ShamsiDate,
                             SystemTarefeh = current.SystemTarefeh,
                             Tarefeh = current.Tarefeh,
                             LisenceNumber = current.LisenceNumber,
                             LicenseDate = current.LicenseDate
                         })
                         .AsQueryable();
                var bb = ViewModelsvarRequest.ToList();
                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel>(ViewModelsvarRequest, true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual System.Web.Mvc.ActionResult DateOption(ViewModels.Areas.Administrator.Request.SearchViewModel viewModel)
        {
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                //.Where(current => current.Bank_AppStatus == 3)
                .Where(current => current.Bank_AppStatusCode == 0)
                .Where(x => x.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23 ? x.AmountPaid > 0 : true)
                .Where(current => current.Bank_AppStatusDescription == "COMMIT")
                ;

            #region Condition
            viewModel.CompanyName = Utilities.Text.Utility.FixText(viewModel.CompanyName);
            viewModel.CompanyNationalCode = Utilities.Text.Utility.FixText(viewModel.CompanyNationalCode);
            viewModel.RecordNumber = Utilities.Text.Utility.FixText(viewModel.RecordNumber);

            if (viewModel.CompanyName != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyName.Contains(viewModel.CompanyName))
                    ;
            }

            if (viewModel.CompanyNationalCode != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyNationalCode.Contains(viewModel.CompanyNationalCode))
                    ;
            }

            if (viewModel.RecordNumber != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.RecordNumber.Contains(viewModel.RecordNumber))
                    ;
            }

            if (viewModel.InvoiceNumber.HasValue)
            {
                varRequest =
                    varRequest
                    .Where(current => current.InvoiceNumber == viewModel.InvoiceNumber.Value)
                    ;
            }

            if (viewModel.RequestState.HasValue)
            {
                varRequest = varRequest.Where(current => current.RequestState == viewModel.RequestState);
            }


            if (viewModel.SubSystem != null && viewModel.SubSystem != new Guid())
            {
                //if (viewModel.SubSystem == new Guid("00000000-0000-0000-0000-000000000001"))
                //    varRequest = varRequest.Where(current => current.SecNumber == "00000");

                //else
                varRequest = varRequest.Where(current => current.SubSystemId == viewModel.SubSystem);
            }

            if (viewModel.Province != null && viewModel.Province != new Guid())
            {
                varRequest = varRequest.Where(current => current.ProvinceId == viewModel.Province);
            }

            if (viewModel.City != null && viewModel.City != new Guid())
            {
                varRequest = varRequest.Where(current => current.CityId == viewModel.City);
            }

            #endregion

            try
            {

                var ViewModelsvarRequest
                     = varRequest
                     .OrderBy(current => current.CompanyName)
                     .ThenByDescending(current => current.Bank_RealTransactionDateTime)
                     .ToList()
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                         {
                             Id = current.Id,
                             SubSystem = current.SubSystem.Name,
                             CompanyName = current.CompanyName,
                             CommodityType = current.CommodityType,
                             TotalValue = current.TotalValue,
                             CompanyNationalCode = current.CompanyNationalCode,
                             Province = current.Province.Name,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             PerformNumber = current.PerformNumber,
                             PerformDate = current.PerformDate,
                             CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                             CurrencyValue = current.CurrencyValue,
                             AmountPaid = current.AmountPaid,
                             Bank_TraceNo = current.Bank_TraceNo.Value.ToString(),
                             Bank_BankReciptNumber = current.Bank_BankReciptNumber.Substring(8),
                             Bank_ShamsiDate = current.Bank_ShamsiDate != null ? (current.Bank_ShamsiDate.Substring(0, 4)
                             + "/" + current.Bank_ShamsiDate.Substring(4, 2)
                             + "/" + current.Bank_ShamsiDate.Substring(6, 2)) : "[تاریخ ندارد]",
                             Tarefeh = current.Tariffs != null ? current.Tariffs.Value : 0,
                             LisenceNumber = current.LicenseNumber,
                             SystemTarefeh = current.ServiceTariff != null ? Convert.ToInt32(current.ServiceTariff.Amount) : 0,
                             LicenseDate = current.LicenseDate != null 
                                ? new Infrastructure.Calander(current.LicenseDate.Value).Persion() 
                                : "[نا مشخص]"
                         })
                         .ToList()
                         .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                         {
                             Id = current.Id,
                             SubSystem = current.SubSystem,
                             CommodityType = current.CommodityType,
                             TotalValue = current.TotalValue,
                             CompanyName = current.CompanyName,
                             Province = current.Province,
                             CompanyNationalCode = current.CompanyNationalCode,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDate = current.InvoiceDate,
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             PerformNumber = current.PerformNumber,
                             PerformDate = current.PerformDate,
                             CurrencyCode = current.CurrencyCode,
                             CurrencyValue = current.CurrencyValue,
                             AmountPaid = current.AmountPaid,
                             Bank_TraceNo = current.Bank_TraceNo,
                             Bank_BankReciptNumber = current.Bank_BankReciptNumber,
                             Bank_ShamsiDate = current.Bank_ShamsiDate,
                             SystemTarefeh = current.SystemTarefeh,
                             Tarefeh = current.Tarefeh,
                             LisenceNumber = current.LisenceNumber,
                             LicenseDate = current.LicenseDate
                         })
                         .AsQueryable();

                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel>(ViewModelsvarRequest, true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual System.Web.Mvc.JsonResult GetRequests()
        {
            var varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                .Where(current => current.Bank_AppStatusCode == 0)
                .Where(x => x.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23 ? x.AmountPaid > 0 : true)
                .Where(current => current.Bank_AppStatusDescription == "COMMIT")
                ;

            DateTime nowDateTime = DateTime.Now.AddDays(-3);
            int PaymentConfirmation = (int)Enums.RequestStates.PaymentConfirmation;

            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            var ViewModelsvarRequest
                 = varRequest
                 .Where(current => current.RequestState == PaymentConfirmation)
                 .Where(current => current.Bank_RealTransactionDateTime >= nowDateTime)
                 .Where(x => x.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance23 ? x.AmountPaid > 0 : true)
                 .OrderByDescending(current => current.Bank_RealTransactionDateTime)
                 .ThenBy(current => current.CompanyName)
                 .ToList()
                 .Select(current =>
                     new
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem.Name,
                         CompanyName = current.CompanyName,
                         TotalValue = current.TotalValue,
                         CommodityType = current.CommodityType,
                         CompanyNationalCode = current.CompanyNationalCode,
                         Province = current.Province.Name,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = new Infrastructure.Calander(current.InvoiceDate).Persion(),
                         RequestCode = current.RecordNumber,
                         RequestDate = current.RecordDate,
                         PerformNumber = current.PerformNumber,
                         PerformDate = current.PerformDate,
                         CurrencyCode = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.CurrencyCode),
                         CurrencyValue = current.CurrencyValue,
                         AmountPaid = current.AmountPaid,
                         Bank_TraceNo = current.Bank_TraceNo.Value.ToString(),
                         Bank_BankReciptNumber = current.Bank_BankReciptNumber.Substring(8),
                         Bank_ShamsiDate = current.Bank_ShamsiDate != null ? (current.Bank_ShamsiDate.Substring(0, 4)
                         + "/" + current.Bank_ShamsiDate.Substring(4, 2)
                         + "/" + current.Bank_ShamsiDate.Substring(6, 2)) : "[تاریخ ندارد]",
                         Tarefeh = current.Tariffs != null ? current.Tariffs.Value : 0,
                         SystemTarefeh = current.ServiceTariff != null ? Convert.ToInt32(current.ServiceTariff.Amount) : 0,
                         LisenceNumber = current.LicenseNumber,
                         LicenseDate = current.LicenseDate != null ? new Infrastructure.Calander(current.LicenseDate.Value).Persion() : "[نا مشخص]"

                     })
                     .ToList()
                     .Select(current =>
                     new ViewModels.Areas.Administrator.Request.IndexFinancialViewModel()
                     {
                         Id = current.Id,
                         SubSystem = current.SubSystem,
                         CompanyName = current.CompanyName,
                         TotalValue = current.TotalValue,
                         CommodityType = current.CommodityType,
                         CompanyNationalCode = current.CompanyNationalCode,
                         Province = current.Province,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = current.InvoiceDate,
                         RecordNumber = current.RequestCode,
                         RecordDate = current.RequestDate,
                         PerformNumber = current.PerformNumber,
                         PerformDate = current.PerformDate,
                         CurrencyCode = current.CurrencyCode,
                         CurrencyValue = current.CurrencyValue,
                         AmountPaid = current.AmountPaid,
                         Bank_TraceNo = current.Bank_TraceNo,
                         Bank_BankReciptNumber = current.Bank_BankReciptNumber,
                         Bank_ShamsiDate = current.Bank_ShamsiDate,
                         SystemTarefeh = current.SystemTarefeh,
                         Tarefeh = current.Tarefeh,
                         LisenceNumber = current.LisenceNumber,
                         LicenseDate = current.LicenseDate
                     })
                     .AsQueryable();

            object dataSource;

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel>(ViewModelsvarRequest, true, out dataSource);

            Infrastructure.Sessions.SearchDataSource = dataSource;

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual ActionResult DetailsId(Guid id)
        {
            try
            {
                var oRequest =
                 UnitOfWork.RequestRepository.Get()
                 .Where(current => current.Id == id)
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

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual ActionResult DetailsByTracingCode(string tracingcode)
        {
            try
            {
                var oRequest =
                 UnitOfWork.RequestRepository.Get()
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

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual FileResult GetExcelFile(string title)
        {
            // Is there a spreadsheet stored in session?
            if (Session[title] != null)
            {
                // Get the spreadsheet from seession.
                byte[] file = Session[title] as byte[];
                string filename = string.Format("{0}.xlsx", title);
                // Remove the spreadsheet from session.
                Session.Remove(title);
                // Return the spreadsheet.
                Response.Buffer = true;
                //  Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", filename));
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            else
            {
                throw new Exception(string.Format("{0} not found", title));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual ActionResult ExpotToExcel()
        {
            return Index();
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
            //
            DateTime nowDateTime = DateTime.Now.AddDays(-3);
            int PaymentConfirmation = (int)Enums.RequestStates.PaymentConfirmation;

            var varRequestTypes = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varRequestTypes, "Id", "Name", null);

            var newDataSource = Infrastructure.Sessions.SearchDataSource as List<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel>;

            // Pass your ef data to method
            ExcelPackage package = GenerateExcelFile(newDataSource);

            var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }

        private static ExcelPackage GenerateExcelFile(List<ViewModels.Areas.Administrator.Request.IndexFinancialViewModel> datasource)
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
            WorkSheet.Cells[1, 1, 1, 18].Merge = true;


            // Sets Headers
            WorkSheet.Cells[2, 1].Value = Resources.Model.Request.SubSystem;
            WorkSheet.Cells[2, 2].Value = Resources.Model.Request.CompanyName;
            WorkSheet.Cells[2, 3].Value = Resources.Model.Request.CommodityType;
            WorkSheet.Cells[2, 4].Value = Resources.Model.Request.TotalValue;
            WorkSheet.Cells[2, 5].Value = Resources.Model.Request.Bank_TraceNo;
            WorkSheet.Cells[2, 6].Value = Resources.Model.Request.Bank_ShamsiDate;
            WorkSheet.Cells[2, 7].Value = Resources.Model.Request.CurrencyCode;
            WorkSheet.Cells[2, 8].Value = Resources.Model.Request.CurrencyValue;
            WorkSheet.Cells[2, 9].Value = Resources.Model.Request.AmountPaid;
			WorkSheet.Cells[2, 10].Value = Resources.Model.Request.RecordNumber;
            WorkSheet.Cells[2, 11].Value = Resources.Model.Request.RecordDate;
            WorkSheet.Cells[2, 12].Value = Resources.Model.Request.PerformNumber;
            WorkSheet.Cells[2, 13].Value = Resources.Model.Request.PerformDate;
            WorkSheet.Cells[2, 14].Value = Resources.Model.Request.CompanyNationalCode;
            WorkSheet.Cells[2, 15].Value = Resources.Model.Request.InvoiceNumber;
            WorkSheet.Cells[2, 16].Value = Resources.Model.Request.InvoiceDate;
            WorkSheet.Cells[2, 17].Value = Resources.Model.Request.LisenceNumber;
            WorkSheet.Cells[2, 18].Value = Resources.Model.Request.LicenseDate;
            WorkSheet.Cells[2, 19].Value = Resources.Model.Request.Tarefeh;
			WorkSheet.Cells[2, 20].Value = Resources.Model.Request.SystemTarefeh;
			WorkSheet.Cells[2, 21].Value = "مبلغ ارز";


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

            WorkSheet.Cells[2, 19].Style.Border.Top.Style
               = WorkSheet.Cells[2, 19].Style.Border.Bottom.Style
               = WorkSheet.Cells[2, 19].Style.Border.Right.Style
               = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 20].Style.Border.Top.Style
               = WorkSheet.Cells[2, 20].Style.Border.Bottom.Style
               = WorkSheet.Cells[2, 20].Style.Border.Right.Style
               = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

			WorkSheet.Cells[2, 21].Style.Border.Top.Style
			   = WorkSheet.Cells[2, 21].Style.Border.Bottom.Style
			   = WorkSheet.Cells[2, 21].Style.Border.Right.Style
			   = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

			#endregion


			// Inserts Data
			for (int i = 0; i < datasource.Count(); i++)
            {
                #region Row Value
                WorkSheet.Cells[i + 3, 1].Value = datasource.ElementAt(i).SubSystem;
                WorkSheet.Cells[i + 3, 2].Value = datasource.ElementAt(i).CompanyName;
                WorkSheet.Cells[i + 3, 3].Value = datasource.ElementAt(i).CommodityType;
                WorkSheet.Cells[i + 3, 4].Value = datasource.ElementAt(i).TotalValue;
                WorkSheet.Cells[i + 3, 5].Value = datasource.ElementAt(i).Bank_TraceNo;
                WorkSheet.Cells[i + 3, 6].Value = datasource.ElementAt(i).Bank_ShamsiDate;
                WorkSheet.Cells[i + 3, 7].Value = datasource.ElementAt(i).CurrencyCode;
                WorkSheet.Cells[i + 3, 8].Value = datasource.ElementAt(i).CurrencyValue;
                WorkSheet.Cells[i + 3, 9].Value = datasource.ElementAt(i).AmountPaid;
                WorkSheet.Cells[i + 3, 10].Value = datasource.ElementAt(i).RecordNumber;
                WorkSheet.Cells[i + 3, 11].Value = datasource.ElementAt(i).RecordDate;
                WorkSheet.Cells[i + 3, 12].Value = datasource.ElementAt(i).PerformNumber;
                WorkSheet.Cells[i + 3, 13].Value = datasource.ElementAt(i).PerformDate;
                WorkSheet.Cells[i + 3, 14].Value = datasource.ElementAt(i).CompanyNationalCode;
                WorkSheet.Cells[i + 3, 15].Value = datasource.ElementAt(i).InvoiceNumber;
                WorkSheet.Cells[i + 3, 16].Value = datasource.ElementAt(i).InvoiceDate;
                WorkSheet.Cells[i + 3, 17].Value = datasource.ElementAt(i).LisenceNumber;
                WorkSheet.Cells[i + 3, 18].Value = datasource.ElementAt(i).LicenseDate;
                WorkSheet.Cells[i + 3, 19].Value = datasource.ElementAt(i).Tarefeh;
                WorkSheet.Cells[i + 3, 20].Value = datasource.ElementAt(i).SystemTarefeh;
				WorkSheet.Cells[i + 3, 21].Value =
					Math.Round((datasource.ElementAt(i).AmountPaid)/(datasource.ElementAt(i).CurrencyValue));
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

                WorkSheet.Cells[i + 3, 19].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 19].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 19].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 20].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 20].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 20].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

				WorkSheet.Cells[i + 3, 21].Style.Border.Top.Style
					= WorkSheet.Cells[i + 3, 21].Style.Border.Bottom.Style
					= WorkSheet.Cells[i + 3, 21].Style.Border.Right.Style
					= OfficeOpenXml.Style.ExcelBorderStyle.Thin;

				#endregion
			}

            WorkSheet.Column(1).AutoFit(25);
            WorkSheet.Column(2).AutoFit(30);
            WorkSheet.Column(3).AutoFit(30);
            WorkSheet.Column(4).AutoFit(30);
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
            WorkSheet.Column(19).AutoFit(20);
            WorkSheet.Column(20).AutoFit(20);
            WorkSheet.Column(21).AutoFit(20);

			// Format Header of Table
			using (ExcelRange rng = WorkSheet.Cells["A1:P1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }

            using (ExcelRange rng = WorkSheet.Cells["A2:U2"])
            {
                rng.Style.Font.Bold = true;
                //WorkSheet.Cells["A2:L2"].AutoFilter = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }
            return excelPackage;
        }
    }
}