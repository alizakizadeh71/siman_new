using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPS.ir.shaparak.sadad;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class RequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", null);

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

            //var varProvinces = UnitOfWork.ProvinceRepository.Get()
            //    .Select(x => new ViewModels.ComboboxItemGuid
            //    {
            //        Id = x.Id,
            //        Name = x.Name
            //    })
            //    .OrderBy(current => current.Name)
            //    .ToList();
            //ViewBag.Province = varProvinces;

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            var varCurrencyUnit = UnitOfWork.CurrencyUnitRepository.Get().ToList()
                .Select(x => new ViewModels.ComboboxItemGuid
                {
                    Id = x.Id,
                    Name = x.NameString
                })
                    .OrderBy(current => current.Name)
                    .ToList();
            ViewBag.CurrencyUnit = varCurrencyUnit;

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Request.SearchViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                ;

            #region Condition
            viewModel.CompanyName = Utilities.Text.Utility.FixText(viewModel.CompanyName);
            viewModel.CompanyNationalCode = Utilities.Text.Utility.FixText(viewModel.CompanyNationalCode);
            viewModel.RecordNumber = Utilities.Text.Utility.FixText(viewModel.RecordNumber);
            viewModel.CommodityType = Utilities.Text.Utility.FixText(viewModel.CommodityType);

            if (viewModel.Province != null && viewModel.Province != new Guid())
            {
                varRequest = varRequest.Where(current => current.ProvinceId == viewModel.Province);
                Search = true;
            }

            if (viewModel.City != null && viewModel.City != new Guid())
            {
                varRequest = varRequest.Where(current => current.CityId == viewModel.City);
                Search = true;
            }

            if (viewModel.RequestState.HasValue)
            {
                varRequest = varRequest.Where(current => current.RequestState == viewModel.RequestState);
                Search = true;
            }

            if (viewModel.SubSystem != null && viewModel.SubSystem != new Guid())
            {
                //if (viewModel.SubSystem == new Guid("00000000-0000-0000-0000-000000000001"))
                //    varRequest = varRequest.Where(current => current.SecNumber == "00000");

                //else
                varRequest = varRequest.Where(current => current.SubSystemId == viewModel.SubSystem);
                Search = true;
            }
            if (viewModel.FromAmount.ToString().Length > 0 && viewModel.ToAmount.ToString().Length > 0 && viewModel.FromAmount <= viewModel.ToAmount)
            {
                varRequest =
                    varRequest
                    .Where(current => current.AmountPaid >= viewModel.FromAmount && current.AmountPaid <= viewModel.ToAmount)
                    ;
                Search = true;
            }
            if (viewModel.StartDate.ToString().Length > 0)
            {
                varRequest =
                    varRequest
                    .Where(current => current.InvoiceDate >= viewModel.StartDate)
                    ;
                Search = true;
            }
            if (viewModel.EndDate.ToString().Length > 0)
            {
                var EndDate1 = viewModel.EndDate;
                var EndDate2 = EndDate1.Value.AddDays(1);
                varRequest =
                    varRequest
                    .Where(current => current.InvoiceDate < EndDate2)
                    ;
                Search = true;
            }

            if (viewModel.PayStartDate.ToString().Length > 0)
            {
                //string StringPayStartDate = ConvertDate(viewModel.PayStartDate.ToString());
                //DateTime dt = DateTime.Parse(viewModel.PayStartDate.ToString(), new CultureInfo("fa-IR"));
                //var NewDate = new Infrastructure.Calander(viewModel.PayStartDate.Value);

                varRequest = varRequest.Where(current => current.AmountPaidDate >= viewModel.PayStartDate);
                //varRequest = varRequest.Where(current =>
                //     (current.AmountPaidDate != null && current.AmountPaidDate >= viewModel.PayStartDate) ||
                //     (current.AmountPaidDate == null && current.Bank_ShamsiDate != null && current.Bank_ShamsiDate.Length == 10 &&
                //     (StringPayStartDate.CompareTo(current.Bank_ShamsiDate) <= 0)));
                Search = true;
            }
            if (viewModel.PayEndDate.ToString().Length > 0)
            {
                var PayEndDate1 = viewModel.PayEndDate.Value;
                var PayEndDate2 = PayEndDate1.AddDays(1);
                var StringPayEndDate = ConvertDate(PayEndDate2.ToString());
                varRequest = varRequest.Where(current => current.AmountPaidDate < PayEndDate2);
                //varRequest = varRequest.Where(current =>
                //    (current.AmountPaidDate != null && current.AmountPaidDate < PayEndDate2) ||
                //    (current.AmountPaidDate == null && current.Bank_ShamsiDate != null && current.Bank_ShamsiDate.Length == 10 &&
                //    (StringPayEndDate.CompareTo(current.Bank_ShamsiDate) >= 0)));
                Search = true;
            }

            if (viewModel.CompanyName != string.Empty && viewModel.CompanyName != "نام شرکت باید به طور دقیق وارد شود")
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyName == viewModel.CompanyName)
                    ;
                Search = true;
            }

            if (viewModel.CommodityType != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CommodityType.Contains(viewModel.CommodityType))
                    ;
                Search = true;
            }

            if (viewModel.CompanyNationalCode != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.CompanyNationalCode == viewModel.CompanyNationalCode)
                    ;
                Search = true;
            }

            if (viewModel.RecordNumber != string.Empty)
            {
                varRequest =
                    varRequest
                    .Where(current => current.RecordNumber == viewModel.RecordNumber);
                Search = true;
            }
            
            if (!string.IsNullOrEmpty(viewModel.ImportRecordNumber2))
            {
                varRequest =
                    varRequest
                    .Where(current => current.ImportRecordNumber == viewModel.ImportRecordNumber2);
                Search = true;
            }

            if (viewModel.InvoiceNumber.HasValue)
            {
                varRequest =
                    varRequest
                    .Where(current => current.InvoiceNumber == viewModel.InvoiceNumber.Value)
                    ;
                Search = true;
            }



            if (viewModel.BaseCurrencyValue.HasValue) // جستجو بر اساس مبلغ فوب
            {
                varRequest =
                    varRequest
                    .Where(current => current.BaseCurrencyValue == viewModel.BaseCurrencyValue.Value)
                    ;
                Search = true;
            }
            if (viewModel.CurrencyUnit != null && viewModel.CurrencyUnit != new Guid()) // جستجو بر اساس نوع ارز
            {
                var Code = UnitOfWork.CurrencyUnitRepository.Get()
                    .Where(current => current.Id == viewModel.CurrencyUnit)
                    .FirstOrDefault().Code;
                varRequest = varRequest.Where(current => current.CurrencyCode == Code);
                Search = true;
            }
            if (viewModel.Bank_TraceNo.HasValue) // جستجو بر اساس شماره پیگیري بانکی
            {
                varRequest =
                    varRequest
                    .Where(current => current.Bank_TraceNo == viewModel.Bank_TraceNo.Value)
                    ;
                Search = true;
            }




            if (!Search)
            {
                return (Json(null, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }

            #endregion

            //var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            //ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", viewModel.SubSystem);

            //var varRequestStates = Infrastructure.Utility.EnumList(Enums.EnumTypes.RequestStates);
            //ViewData["RequestState"] = new System.Web.Mvc.SelectList(varRequestStates, "Id", "Name", viewModel.RequestState);

            //var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            //ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            //var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            //ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            try
            {
                var ViewModelsvarRequest
                     = varRequest
                     .OrderByDescending(current => current.InvoiceNumber)
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexViewModel
                         {
                             Id = current.Id,
                             //SubSystemId = current.SubSystemId,
                             SubSystem = current.SubSystem.Name,
                             CompanyName = current.CompanyName,
                             //ServiceTariffId = current.ServiceTariffId,
                             ServiceTariff = current.ServiceTariff.Name,
                             //ProvinceId = current.ProvinceId,
                             Province=current.Province.Name,
                             SecNumber = current.SecNumber,
                             CompanyNationalCode = current.CompanyNationalCode,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDateNew = current.InvoiceDate,
                             AmountPaidDateNew = current.AmountPaidDate,
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             RequestStateNew = current.RequestState,
                             RequestState_Value = current.RequestState,
                             AmountPaid = current.AmountPaid,
                             DepositNumber = current.DepositNumber,
                             //CityId = current.CityId,
                             City=current.City.Name,
                             CeratedBy=current.User.FullName,
                             //CeratedById = current.UserId,
                             Bank_ShamsiDate = current.Bank_ShamsiDate,
                         })
                         .ToList();
                var result = new List<ViewModels.Areas.Administrator.Request.IndexViewModel>();

                ViewModelsvarRequest
                     .ForEach(current =>
                     {
                         result.Add(
                         new ViewModels.Areas.Administrator.Request.IndexViewModel
                         {
                             Id = current.Id,
                             SubSystem = current.SubSystem,
                             CompanyName = current.CompanyName,
                             ServiceTariff = current.ServiceTariff,
                             Province = current.Province,
                             SecNumber = current.SecNumber,
                             CompanyNationalCode = current.CompanyNationalCode,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDate = new Infrastructure.Calander(current.InvoiceDateNew.Value).Persion(),
                             AmountPaidDate = current.AmountPaidDate != null ? new Infrastructure.Calander(current.AmountPaidDateNew ?? DateTime.Now).Persion() : current.Bank_ShamsiDate,
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestStateNew.Value),
                             RequestState_Value = current.RequestState_Value,
                             AmountPaid = current.AmountPaid,
                             DepositNumber = current.DepositNumber,
                             City =current.City,
                             CeratedBy = current.CeratedBy,
                         });
                         return;
                     }
                );

                     //.AsQueryable();
                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(result.AsQueryable(), true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                var ViewModelsvarRequest
                     = varRequest
                     .OrderByDescending(current => current.InvoiceNumber)
                     .Take(5)
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexViewModel
                         {
                             Id = current.Id,
                             CompanyName = current.CompanyName,
                             CompanyNationalCode = current.CompanyNationalCode,
                             RequestState_Value = current.RequestState,
                             InvoiceNumber = current.InvoiceNumber,
                             AmountPaid = current.AmountPaid,
                         })
                         .ToList();

                var ViewModelsvarRequest2 = ViewModelsvarRequest
                          .Select(current =>
                          new ViewModels.Areas.Administrator.Request.IndexViewModel
                          {
                              Id = current.Id,
                              CompanyName = current.CompanyName,
                              SubSystem = "",
                              ServiceTariff = "",
                              Province = "",
                              SecNumber = "",
                              CompanyNationalCode = current.CompanyNationalCode,
                              InvoiceNumber = current.InvoiceNumber,
                              InvoiceDate = "",
                              AmountPaidDate = "",
                              RecordNumber = "",
                              RecordDate = "",
                              RequestState = "",
                              RequestState_Value = current.RequestState_Value,
                              AmountPaid = current.AmountPaid,
                              DepositNumber = "",
                              City = "",
                              CeratedBy = "",
                          })
                          .AsQueryable();

                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(ViewModelsvarRequest2, true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

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

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.JsonResult GetRequests()
        {
            var varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                .Where(x => x.SubSystem.Code < 2000 || x.SubSystem.Code > 3000)
                ;

            // اگر کاربر صدور فاکتور بود زیر سیستم بررسی نشود و در درخواست ها نمایش داده شود
            if (Infrastructure.Sessions.AuthenticatedUser.Role == Enums.Roles.ExporterOFInvoice)
            {
                varRequest =
                UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                ;
            }

            var varSubsystems = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
            ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varSubsystems, "Id", "Name", null);

            var varRequestStates = Infrastructure.Utility.EnumList(Enums.EnumTypes.RequestStates);
            ViewData["RequestState"] = new System.Web.Mvc.SelectList(varRequestStates, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            try
            {
                var ViewModelsvarRequest
                     = varRequest
                     .OrderByDescending(current => current.InvoiceNumber)
                     .Take(30)
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexViewModel
                         {
                             Id = current.Id,
                             SubSystemId = current.SubSystemId,
                             CompanyName = current.CompanyName,
                             ServiceTariffId = current.ServiceTariffId,
                             ProvinceId = current.ProvinceId,
                             SecNumber = current.SecNumber,
                             CompanyNationalCode = current.CompanyNationalCode,
                             InvoiceNumber = current.InvoiceNumber,
                             InvoiceDateNew = current.InvoiceDate,
                             AmountPaidDateNew = current.AmountPaidDate,
                             RecordNumber = current.RecordNumber,
                             RecordDate = current.RecordDate,
                             RequestStateNew = current.RequestState,
                             RequestState_Value = current.RequestState,
                             AmountPaid = current.AmountPaid,
                             DepositNumber = current.DepositNumber,
                             CityId = current.CityId,
                             CeratedById = current.UserId,
                             Bank_ShamsiDate = current.Bank_ShamsiDate,
                         })
                         .ToList();


                var ViewModelsvarRequest1 = ViewModelsvarRequest
                     .Select(current =>
                     new ViewModels.Areas.Administrator.Request.IndexViewModel
                     {
                         Id = current.Id,
                         SubSystem = UnitOfWork.SubSystemRepository.GetById(current.SubSystemId).Name,
                         CompanyName = current.CompanyName,
                         ServiceTariff = GetServiceTariff(current.ServiceTariffId),
                         Province = UnitOfWork.ProvinceRepository.GetById(current.ProvinceId).Name,
                         SecNumber = current.SecNumber,
                         CompanyNationalCode = current.CompanyNationalCode,
                         InvoiceNumber = current.InvoiceNumber,
                         InvoiceDate = new Infrastructure.Calander(current.InvoiceDateNew.Value).Persion(),
                         AmountPaidDate = current.AmountPaidDate != null ? new Infrastructure.Calander(current.AmountPaidDateNew ?? DateTime.Now).Persion() : current.Bank_ShamsiDate,
                         RecordNumber = current.RecordNumber,
                         RecordDate = current.RecordDate,
                         RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestStateNew.Value),
                         RequestState_Value = current.RequestState_Value,
                         AmountPaid = current.AmountPaid,
                         DepositNumber = current.DepositNumber,
                         City = GetCity(current.CityId),
                         CeratedBy = UnitOfWork.UserRepository.GetById(current.CeratedById).FullName,
                     })
                     .AsQueryable();

                //object dataSource;

                //var varResult =
                //    Utilities.Kendo.HtmlHelpers
                //    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(ViewModelsvarRequest);

                ////Infrastructure.Sessions.DataSource = dataSource;

                //****************** New******************//
                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(ViewModelsvarRequest1, true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                var ViewModelsvarRequest
                     = varRequest
                     .OrderByDescending(current => current.InvoiceNumber)
                     .Take(5)
                     .Select(current =>
                         new ViewModels.Areas.Administrator.Request.IndexViewModel
                         {
                             Id = current.Id,
                             CompanyName = current.CompanyName,
                             CompanyNationalCode = current.CompanyNationalCode,
                             RequestState_Value = current.RequestState,
                             InvoiceNumber = current.InvoiceNumber,
                             AmountPaid = current.AmountPaid,
                         })
                         .ToList();

                var ViewModelsvarRequest2 = ViewModelsvarRequest
                          .Select(current =>
                          new ViewModels.Areas.Administrator.Request.IndexViewModel
                          {
                              Id = current.Id,
                              CompanyName = current.CompanyName,
                              SubSystem = "",
                              ServiceTariff = "",
                              Province = "",
                              SecNumber = "",
                              CompanyNationalCode = current.CompanyNationalCode,
                              InvoiceNumber = current.InvoiceNumber,
                              InvoiceDate = "",
                              AmountPaidDate = "",
                              RecordNumber = "",
                              RecordDate = "",
                              RequestState = "",
                              RequestState_Value = current.RequestState_Value,
                              AmountPaid = current.AmountPaid,
                              DepositNumber = "",
                              City = "",
                              CeratedBy = "",
                          })
                          .AsQueryable();

                object dataSource;

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(ViewModelsvarRequest2, true, out dataSource);

                Infrastructure.Sessions.SearchDataSource = dataSource;

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

            }
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
        public virtual ActionResult Edit(Guid id)
        {
            try
            {
                ViewBag.IsApprovallicense = Infrastructure.Sessions.AuthenticatedUser.User.IsApprovallicense; // مجوز دسترسی پروانه ها
                ViewBag.IsFaVA = Infrastructure.Sessions.AuthenticatedUser.User.Role.Code == 860 ? true : false; // اگر فاوا بود دکمه ها نمایش داده نشود
                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);

                ViewBag.PageMessages = null;

                #region Update Currency Ration
                var oLastRequest =
                    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                    .Where(current => current.Id == id)
                    .FirstOrDefault()
                    ;

                var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oLastRequest.CurrencyCode);

                //if (oLastRequest.RequestState < (int)Enums.RequestStates.PaymentOrder)
                //{
                //    oLastRequest.CurrencyRation = oCurrency.Ratio;

                //    if (oLastRequest.SubSystem.Code == (int)Enums.SubSystems.Drug_Clearance)
                //        oLastRequest.AmountPaid = Convert.ToInt64(oLastRequest.CurrencyValue * oCurrency.Ratio) + 33000;
                //    else
                //        oLastRequest.AmountPaid = Convert.ToInt64(oLastRequest.CurrencyValue * oCurrency.Ratio);

                //    //oLastRequest.AmountPaid = Convert.ToInt64(oLastRequest.CurrencyValue * oCurrency.Ratio) + 33000;
                //    UnitOfWork.RequestRepository.Update(oLastRequest);
                //    UnitOfWork.Save();
                //}
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
                     ServiceTariff = current.ServiceTariff?.NameString,
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

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
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
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Request.EditViewModel request, string submit)
        {
            try
            {
                var asaasas = submit;
                ViewBag.PageMessages = null;
                string RetMessage = string.Empty;
                DAL.UnitOfWork UnitOfWork = new DAL.UnitOfWork();
                Models.Request oRequestNew = null;

                var oRequest =
                    UnitOfWork.RequestRepository.GetById(request.Id)
                    ;

                switch (submit)
                {
                    case "اعلام نواقصی":
                        {
                            Infrastructure.WebServiceClass oWebServiceClass = new Infrastructure.WebServiceClass();

                            #region Change State in OPS
                            oRequest.RequestState = (int)Enums.RequestStates.Incomplete;
                            UnitOfWork.RequestRepository.Update(oRequest);
                            RetMessage = " اعلام نواقصی در سیستم پرداخت ثبت شد.";
                            RetMessage += "<br/>";
                            #endregion

                            #region Chage State in IVO By Webservice
                            //if (!(oRequest.SubSystemId == new Guid("c3001c61-fc9b-11ea-9f5e-0050568d5b96") || oRequest.SubSystemId == new Guid("c3002c61-fc9b-11ea-9f5e-0050568d5b96"))) // اگر خالف پروانه ها یا گواهی حق ثبت بود
                            //{
                            //    RetMessage += oWebServiceClass.Insert_Incomplete(oRequest.RecordNumber, request.SystemMessage);
                            //}
                            #endregion

                            #region Insert New Message
                            Models.Message oMessage = new Models.Message();
                            oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                            oMessage.LastState = oRequest.RequestState;
                            oMessage.MessageText = Resources.Message.Request.Message_Incomplete + " - " + request.SystemMessage;
                            oMessage.NewState = (int)Enums.RequestStates.Incomplete;
                            oMessage.RequestId = oRequest.Id;
                            UnitOfWork.MessageRepository.Insert(oMessage);
                            #endregion

                            UnitOfWork.Save();

                            #region Refresh View
                            ViewBag.PageMessages = RetMessage;

                            var oNewRequest =
                              UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                              .Where(current => current.InvoiceNumber == oRequest.InvoiceNumber)
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
                            ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(oNewRequest.Id);

                            return View(oNewRequest);
                            #endregion
                        }
                    case "لغو تایید پرداخت":
                        {
                            #region Change State in OPS
                            oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                            UnitOfWork.RequestRepository.Update(oRequest);
                            RetMessage = " لغو تایید پرداخت در سیستم پرداخت ثبت شد.";
                            RetMessage += "<br/>";
                            #endregion

                            #region Insert New Message
                            Models.Message oMessage = new Models.Message();
                            oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                            oMessage.LastState = oRequest.RequestState;
                            oMessage.MessageText = " لغو تایید پرداخت انجام شد " + " - " + request.SystemMessage;
                            oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
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
                            ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(oNewRequest.Id);

                            return View(oNewRequest);
                            #endregion
                        }
                    case "لغو دستور پرداخت":
                        {
                            #region Change State in OPS
                            oRequest.RequestState = (int)Enums.RequestStates.InitialRequet;
                            UnitOfWork.RequestRepository.Update(oRequest);
                            RetMessage = " لغو دستور پرداخت در سیستم پرداخت ثبت شد.";
                            RetMessage += "<br/>";
                            #endregion

                            #region Insert New Message
                            Models.Message oMessage = new Models.Message();
                            oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                            oMessage.LastState = oRequest.RequestState;
                            oMessage.MessageText = " لغو دستور پرداخت انجام شد " + " - " + request.SystemMessage;
                            oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
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
                            ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(oNewRequest.Id);

                            return View(oNewRequest);
                            #endregion
                        }
                    case "استعلام مجدد":
                        {
                            #region استعلام مجدد
                            var oAccountNumber =
                                UnitOfWork.AccountNumberManageRepository.Get()
                                .Where(current => current.ProvinceId == oRequest.ProvinceId)
                                .Where(current => current.SubSystemId == oRequest.SubSystemId)
                                .FirstOrDefault()
                                .AccountNumber
                                ;

                            OPS.ir.shaparak.sadad.CheckStatusResult _AppStatusCode
                                = oMerchantUtility.GetRequestStatusResult
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

                            oRequest.AmountPaidDate = DateTime.Now;
                            oRequest.Bank_AppStatus = _AppStatusCode.AppStatus;
                            oRequest.Bank_AppStatusCode = _AppStatusCode.AppStatusCode;
                            oRequest.Bank_AppStatusDescription = _AppStatusCode.AppStatusDescription;
                            oRequest.Bank_BankReciptNumber = _AppStatusCode.BankReciptNumber;
                            oRequest.Bank_CardHolderAccNumber = _AppStatusCode.CardHolderAccNumber;
                            oRequest.Bank_CardHolderName = _AppStatusCode.CardHolderName;
                            oRequest.Bank_CustomerCardNumber = _AppStatusCode.CustomerCardNumber;
                            oRequest.Bank_FailCode = _AppStatusCode.FailCode;
                            oRequest.Bank_NewlyCommitted = _AppStatusCode.NewlyCommitted;
                            oRequest.Bank_RealTransactionDateTime = DateTime.Now;
                            oRequest.Bank_RefrenceNumber = _AppStatusCode.RefrenceNumber;
                            oRequest.Bank_ResponseCode = _AppStatusCode.ResponseCode;
                            oRequest.Bank_ShamsiDate = _AppStatusCode.ShamsiDate;
                            oRequest.Bank_TraceNo = _AppStatusCode.TraceNo;
                            oRequest.Bank_Terminal = oAccountNumber.Terminal;
                            oRequest.Bank_MerchantId = oAccountNumber.MerchantId;
                            oRequest.UpdateDateTime = DateTime.Now;

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
                                    oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation + " - " + request.SystemMessage;
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
                                oMessage.MessageText = Resources.Message.Request.Message_NoPaymented + " - " + request.SystemMessage;
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

                            #endregion 
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
                            ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(oNewRequest.Id);

                            return View(oNewRequest);
                            #endregion
                        }

                    default:
                        {
                            #region Change State in OPS

                            var vv = request.DepositNumber;

                            switch (oRequest.RequestState)
                            {
                                case -1:
                                case 0:
                                case 1:
                                    {
                                        oRequest.RequestState = 1;
                                        UnitOfWork.RequestRepository.Update(oRequest);
                                        ViewBag.PageMessages += " دستور پرداخت برای این درخواست در سیستم ثبت شد.";


                                        #region Insert New Message
                                        Models.Message oMessage = new Models.Message();
                                        oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                                        oMessage.LastState = oRequest.RequestState;
                                        oMessage.MessageText = Resources.Message.Request.Message_PaymentOrder + " - " + request.SystemMessage;
                                        oMessage.NewState = (int)Enums.RequestStates.PaymentOrder;
                                        oMessage.RequestId = oRequest.Id;
                                        oMessage.UserIPAddress = Request.UserHostAddress;
                                        oMessage.Browser = Request.Browser.Type; // مدل و ورژن مرورگر
                                        UnitOfWork.MessageRepository.Insert(oMessage);
                                        #endregion

                                        UnitOfWork.Save();

                                        break;
                                    }

                                case 2:
                                case 3:
                                    {
                                        oRequest.RequestState = 3;
                                        UnitOfWork.RequestRepository.Update(oRequest);
                                        ViewBag.PageMessages += " تایید پرداخت برای این درخواست در سیستم ثبت شد.";

                                        #region Insert New Message
                                        Models.Message oMessage = new Models.Message();
                                        oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                                        oMessage.LastState = oRequest.RequestState;
                                        oMessage.MessageText = Resources.Message.Request.Message_PaymentConfirmation + " - " + request.SystemMessage;
                                        oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
                                        oMessage.RequestId = oRequest.Id;
                                        UnitOfWork.MessageRepository.Insert(oMessage);
                                        #endregion

                                        UnitOfWork.Save();

                                        break;

                                    }
                            }
                            #endregion

                            #region Refresh View
                            var oCurrency = UnitOfWork.CurrencyUnitRepository.GetByCode(oRequest.CurrencyCode);

                            var oNewRequest =
                             UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                             .Where(current => current.Id == oRequest.Id)
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
                                 BaseCurrencyValue = (current.BaseCurrencyValue ?? (current.CurrencyValue * current.CurrencyRation)) + " " + oCurrency.Name,
                                 AmountPaid = current.AmountPaid,
                                 RequestState = Infrastructure.Utility.EnumValue(Enums.EnumTypes.RequestStates, current.RequestState),
                                 RequestStateCode = current.RequestState,
                                 Bank_TraceNo = current.Bank_TraceNo,
                                 Bank_ShamsiDate = current.Bank_ShamsiDate
                             })
                             .FirstOrDefault();

                            if (oNewRequest == null)
                            {
                                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                            }
                            ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(oNewRequest.Id);

                            return View(oNewRequest);
                            #endregion
                        }
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
            Models.Request oRequest = null;
            try
            {
                List<Models.DetailOfFactor> list = null;

                oRequest =
                UnitOfWork.RequestRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();

                if (oRequest.CurrencyRation == Convert.ToDecimal("1.00"))
                {
                    ViewBag.CurrencyRation = "     ---  ";
                    ViewBag.BaseCurrencyValue = "     ---  ";
                }

                else
                {
                    ViewBag.CurrencyRation = oRequest.CurrencyRation;
                    ViewBag.BaseCurrencyValue = oRequest.BaseCurrencyValue;
                }

                ViewBag.ServiceTariff = oRequest.ServiceTariff.NameString + " * " + oRequest.ServiceTariff.SubHeadLine.Name;


                string CommodityDescription = string.Empty; // نمایش شرح خدمت در فاکتور های مالی
                try
                {
                    var headoffactorid = oRequest.HeadOfFactor.Id;
                    List<Models.DetailOfFactor> detailOfFactors = UnitOfWork.DetailOfFactorRepository.Get(headoffactorid).ToList();
                    foreach (var item in detailOfFactors)
                    {
                        if (detailOfFactors.Count > 1)
                        {
                            CommodityDescription += item.CommodityDescription + " - ";
                        }
                        else
                        {
                            CommodityDescription = item.CommodityDescription;
                        }
                    }
                }
                catch (Exception)
                {
                }
                ViewBag.CommodityDescription = CommodityDescription;

                var File = new Rotativa.MVC.ViewAsPdf("PrintNewFactor", oRequest)
                {
                    FileName = oRequest.CompanyName + "-" + oRequest.InvoiceNumber + ".pdf"
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