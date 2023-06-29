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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DestinationIndex()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }
        private void Viewdata(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text); /// ممتازان کرمان

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", cementViewModel.Province);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", cementViewModel.City);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult DestinationGetRequests() => (JsonResult)DestinationSearch(null);


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.FinancialManagementRepository.GetByUser(Infrastructure.Sessions.AuthenticatedUser.User);

            #region Condition

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
            #endregion

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            //InvoiceNumber = current.InvoiceNumber,
                            StringProductName = current.ProductName.Name,
                            StringProductType = current.ProductType.Name,
                            StringPackageType = current.PackageType.Name,
                            StringFactoryName = current.FactoryName.Name,
                            AmountPaid = current.AmountPaid,
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

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult DestinationSearch(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.DestinationManagementRepository.GetByUser(Infrastructure.Sessions.AuthenticatedUser.User);

            #region Condition

            if (viewModel?.ProductName != null && viewModel.ProductName != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.FinancialManagement.ProductNameId == viewModel.ProductName);
                Search = true;
            }

            if (viewModel?.ProductType != null && viewModel.ProductType != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.FinancialManagement.ProductTypeId == viewModel.ProductType);
                Search = true;
            }

            if (viewModel?.PackageType != null && viewModel.PackageType != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.FinancialManagement.PackageTypeId == viewModel.PackageType);
                Search = true;
            }

            if (viewModel?.FactoryName != null && viewModel.FactoryName != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.FinancialManagement.FactoryNameId == viewModel.FactoryName);
                Search = true;
            }

            if (viewModel?.FromAmount.ToString().Length > 0 && viewModel.ToAmount.ToString().Length > 0 && viewModel.FromAmount <= viewModel.ToAmount)
            {
                varRequest =
                    varRequest
                    .Where(current => current.FinancialManagement.AmountPaid >= viewModel.FromAmount && current.FinancialManagement.AmountPaid <= viewModel.ToAmount)
                    ;
                Search = true;
            }
            if (viewModel?.StartDate.ToString().Length > 0)
            {
                varRequest =
                    varRequest
                    .Where(current => current.FinancialManagement.InsertDateTime >= viewModel.StartDate)
                    ;
                Search = true;
            }
            if (viewModel?.EndDate.ToString().Length > 0)
            {
                var EndDate1 = viewModel.EndDate;
                var EndDate2 = EndDate1.Value.AddDays(1);
                varRequest =
                    varRequest
                    .Where(current => current.FinancialManagement.InsertDateTime < EndDate2)
                    ;
                Search = true;
            }
            #endregion

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            //InvoiceNumber = current.InvoiceNumber,
                            StringProductName = current.FinancialManagement.ProductName.Name,
                            StringProductType = current.FinancialManagement.ProductType.Name,
                            StringPackageType = current.FinancialManagement.PackageType.Name,
                            StringFactoryName = current.FinancialManagement.FactoryName.Name,
                            AmountPaid = current.FinancialManagement.AmountPaid,
                            StringProvince = current.Province.Name,
                            StringCity = current.City.Name,
                            DestinationAmountPaid = current.DestinationAmountPaid,
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

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;


            var oFindSubHeadLine =
                 UnitOfWork.FinancialManagementRepository
                 .GetByUser(Infrastructure.Sessions.AuthenticatedUser.User)
                 .Where(current => current.ProductNameId == cementViewModel.ProductName)
                 .Where(current => current.ProductTypeId == cementViewModel.ProductType)
                 .Where(current => current.PackageTypeId == cementViewModel.PackageType)
                 .Where(current => current.FactoryNameId == cementViewModel.FactoryName)
                 .FirstOrDefault()
                 ;

            if (oFindSubHeadLine != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

            else if (cementViewModel.AmountPaid <= 0)
                ViewBag.PageMessages = "مبلغ را وارد نمایید.";

            else
            {
                Models.FinancialManagement financialManagement = new Models.FinancialManagement();
                financialManagement.UserId = Infrastructure.Sessions.AuthenticatedUser.User.Id;
                financialManagement.ProductNameId = cementViewModel.ProductName;
                financialManagement.ProductTypeId = cementViewModel.ProductType;
                financialManagement.PackageTypeId = cementViewModel.PackageType;
                financialManagement.FactoryNameId = cementViewModel.FactoryName;
                financialManagement.AmountPaid = cementViewModel.AmountPaid;
                UnitOfWork.FinancialManagementRepository.Insertdata(financialManagement);
                UnitOfWork.Save();

                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }

            Viewdata(cementViewModel);
            return View(cementViewModel);
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel
                = UnitOfWork.FinancialManagementRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    Id = current.Id,
                    AmountPaid = current.AmountPaid,
                    ProductName = current.ProductNameId,
                    ProductType = current.ProductTypeId,
                    PackageType = current.PackageTypeId,
                    FactoryName = current.FactoryNameId,
                    ProductName1 = current.ProductNameId,
                    ProductType1 = current.ProductTypeId,
                    PackageType1 = current.PackageTypeId,
                    FactoryName1 = current.FactoryNameId,
                })
                .FirstOrDefault()
                ;

            Viewdata(cementViewModel);
            ViewBag.PageMessages = null;

            return (View(cementViewModel));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DestinationEdit(System.Guid id)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel
                = UnitOfWork.DestinationManagementRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    Id = current.Id,
                    FinancialManagementId = current.FinancialManagementId,
                    AmountPaid = current.FinancialManagement.AmountPaid,
                    AmountPaid1 = current.FinancialManagement.AmountPaid,
                    ProductName = current.FinancialManagement.ProductNameId,
                    ProductType = current.FinancialManagement.ProductTypeId,
                    PackageType = current.FinancialManagement.PackageTypeId,
                    FactoryName = current.FinancialManagement.FactoryNameId,
                    ProductName1 = current.FinancialManagement.ProductNameId,
                    ProductType1 = current.FinancialManagement.ProductTypeId,
                    PackageType1 = current.FinancialManagement.PackageTypeId,
                    FactoryName1 = current.FinancialManagement.FactoryNameId,
                    Province = current.ProvinceId.Value,
                    Province1 = current.ProvinceId.Value,
                    City = current.CityId,
                    City1 = current.CityId,
                    DestinationAmountPaid = current.DestinationAmountPaid,
                })
                .FirstOrDefault()
                ;

            Viewdata(cementViewModel);
            ViewBag.PageMessages = null;

            return (View(cementViewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.FinancialManagementRepository
                    .Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault()
                    ;

                var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
                base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName1).OrderByDescending(x => x.Text);

                var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType1).OrderByDescending(x => x.Text); /// تیپ یک

                var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType1).ToList(); /// تیپ یک
                base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType1).OrderByDescending(x => x.Text); /// کیسه

                var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName1).OrderBy(x => x.Text); /// ممتازان کرمان

                // **************************************************
                OlderAccount.IsDeleted = true;
                OlderAccount.IsActived = false;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.FinancialManagementRepository.Update(OlderAccount);
                // **************************************************
                Models.FinancialManagement financialManagement = new Models.FinancialManagement();
                financialManagement.UserId = Infrastructure.Sessions.AuthenticatedUser.User.Id;
                financialManagement.ProductNameId = cementViewModel.ProductName1;
                financialManagement.ProductTypeId = cementViewModel.ProductType1;
                financialManagement.PackageTypeId = cementViewModel.PackageType1;
                financialManagement.FactoryNameId = cementViewModel.FactoryName1;
                financialManagement.AmountPaid = cementViewModel.AmountPaid;
                UnitOfWork.FinancialManagementRepository.Insertdata(financialManagement);
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


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DestinationEdit(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.DestinationManagementRepository
                    .Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault()
                    ;

                var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
                base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName1).OrderByDescending(x => x.Text);

                var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType1).OrderByDescending(x => x.Text); /// تیپ یک

                var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType1).ToList(); /// تیپ یک
                base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType1).OrderByDescending(x => x.Text); /// کیسه

                var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName1).OrderBy(x => x.Text); /// ممتازان کرمان

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", cementViewModel.Province1);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province1).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", cementViewModel.City1);

                // **************************************************
                OlderAccount.IsDeleted = true;
                OlderAccount.IsActived = false;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.DestinationManagementRepository.Update(OlderAccount);
                // **************************************************
                Models.DestinationManagement destinationManagement = new Models.DestinationManagement();
                destinationManagement.FinancialManagementId = cementViewModel.FinancialManagementId;
                destinationManagement.ProvinceId = cementViewModel.Province1;
                destinationManagement.CityId = cementViewModel.City1;
                destinationManagement.DestinationAmountPaid = cementViewModel.DestinationAmountPaid;
                UnitOfWork.DestinationManagementRepository.Insertdata(destinationManagement);
                UnitOfWork.Save();

                // **************************************************
                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ویرایش گردید  ";
                cementViewModel.AmountPaid = cementViewModel.AmountPaid1;
                return View(cementViewModel);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DestinationPrice(System.Guid id)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel
                = UnitOfWork.FinancialManagementRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    Id = current.Id,
                    AmountPaid = current.AmountPaid,
                    ProductName = current.ProductNameId,
                    ProductType = current.ProductTypeId,
                    PackageType = current.PackageTypeId,
                    FactoryName = current.FactoryNameId,
                    ProductName1 = current.ProductNameId,
                    ProductType1 = current.ProductTypeId,
                    PackageType1 = current.PackageTypeId,
                    FactoryName1 = current.FactoryNameId,
                    AmountPaid1 = current.AmountPaid,
                })
                .FirstOrDefault()
                ;

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            Viewdata(cementViewModel);
            ViewBag.PageMessages = null;

            return (View(cementViewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DestinationPrice(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var oFinancialManagement =
                    UnitOfWork.FinancialManagementRepository
                    .Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault()
                    ;

                var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
                base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName1).OrderByDescending(x => x.Text);

                var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType1).OrderByDescending(x => x.Text); /// تیپ یک

                var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType1).ToList(); /// تیپ یک
                base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType1).OrderByDescending(x => x.Text); /// کیسه

                var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName1).ToList(); /// سیمان
                base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName1).OrderBy(x => x.Text); /// ممتازان کرمان

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", cementViewModel.Province);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", cementViewModel.City);



                var oDestinationManagement =
                     UnitOfWork.DestinationManagementRepository
                     .GetByUser(Infrastructure.Sessions.AuthenticatedUser.User)
                     .Where(current => current.FinancialManagement.ProductNameId == cementViewModel.ProductName1)
                     .Where(current => current.FinancialManagement.ProductTypeId == cementViewModel.ProductType1)
                     .Where(current => current.FinancialManagement.PackageTypeId == cementViewModel.PackageType1)
                     .Where(current => current.FinancialManagement.FactoryNameId == cementViewModel.FactoryName1)
                     .Where(current => current.ProvinceId == cementViewModel.Province)
                     .Where(current => current.CityId == cementViewModel.City)
                     .FirstOrDefault()
                     ;

                if (oDestinationManagement != null)
                    ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

                else if (cementViewModel.DestinationAmountPaid <= 0)
                    ViewBag.PageMessages = "مبلغ را وارد نمایید.";

                else
                {
                    // **************************************************
                    Models.DestinationManagement newDestinationManagement = new Models.DestinationManagement();
                    //newDestinationManagement.UserId = Infrastructure.Sessions.AuthenticatedUser.User.Id;
                    newDestinationManagement.FinancialManagementId = cementViewModel.Id;
                    newDestinationManagement.ProvinceId = cementViewModel.Province;
                    newDestinationManagement.CityId = cementViewModel.City;
                    newDestinationManagement.DestinationAmountPaid = cementViewModel.DestinationAmountPaid;
                    UnitOfWork.DestinationManagementRepository.Insertdata(newDestinationManagement);
                    //UnitOfWork.Save();
                    // **************************************************
                    ViewBag.PageMessages = "قیمت در مقصد با موفقیت ثبت گردید  ";
                }

                cementViewModel.ProductName = cementViewModel.ProductName1;
                cementViewModel.ProductType = cementViewModel.ProductType1;
                cementViewModel.PackageType = cementViewModel.PackageType1;
                cementViewModel.FactoryName = cementViewModel.FactoryName1;
                cementViewModel.AmountPaid = cementViewModel.AmountPaid1;

                return View(cementViewModel);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            var oAccountNumberManage
                = UnitOfWork.FinancialManagementRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    StringProductName = current.ProductName.Name,
                    StringProductType = current.ProductType.Name,
                    StringPackageType = current.PackageType.Name,
                    StringFactoryName = current.FactoryName.Name,
                    AmountPaid = current.AmountPaid,
                    StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                })
                .FirstOrDefault()
                ;

            if (oAccountNumberManage == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oAccountNumberManage));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DestinationDelete(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            var oAccountNumberManage
                = UnitOfWork.DestinationManagementRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    StringProductName = current.FinancialManagement.ProductName.Name,
                    StringProductType = current.FinancialManagement.ProductType.Name,
                    StringPackageType = current.FinancialManagement.PackageType.Name,
                    StringFactoryName = current.FinancialManagement.FactoryName.Name,
                    AmountPaid = current.FinancialManagement.AmountPaid,
                    StringProvince = current.Province.Name,
                    StringCity = current.City.Name,
                    DestinationAmountPaid = current.DestinationAmountPaid,
                    StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                })
                .FirstOrDefault()
                ;

            if (oAccountNumberManage == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oAccountNumberManage));
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DestinationDelete(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.DestinationManagementRepository.Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.DestinationManagementRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.Financial.DestinationIndex()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.FinancialManagementRepository.Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.FinancialManagementRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.Financial.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }





















        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit1(System.Guid id)
        {
            ViewBag.PageMessages = null;
            var financialManagement = UnitOfWork.FinancialManagementRepository.GetById(id);

            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            cementViewModel.AmountPaid = financialManagement.AmountPaid;
            cementViewModel.StringProductName = UnitOfWork.ProductNameRepository.GetById(financialManagement.ProductNameId).Name;
            cementViewModel.StringProductType = UnitOfWork.ProductTypeRepository.GetById(financialManagement.ProductTypeId).Name;
            cementViewModel.StringPackageType = UnitOfWork.PackageTypeRepository.GetById(financialManagement.PackageTypeId).Name;
            cementViewModel.StringFactoryName = UnitOfWork.FactoryNameRepository.GetById(financialManagement.FactoryNameId).Name;
            cementViewModel.StringInsertDateTime = new Infrastructure.Calander(financialManagement.InsertDateTime).Persion();
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit1(ViewModels.Areas.Administrator.HeadLine.EditViewModel Office)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Id == Office.Id)
                    .FirstOrDefault()
                    ;

                Models.HeadLine oFindedOther;
                Models.HeadLine oFindedOffice;

                oFindedOther =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Code == Office.Code)
                    .Where(current => current.Id != Office.Id)
                    .FirstOrDefault()
                    ;

                oFindedOffice =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Id == Office.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "تعرفه ای با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(Office);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedOffice.UpdateDateTime = DateTime.Now;
                    oFindedOffice.Name = Office.Name;
                    oFindedOffice.Code = Office.Code;

                    UnitOfWork.HeadLineRepository.Update(oFindedOffice);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "تعرفه درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(Office);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
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
                    Math.Round((datasource.ElementAt(i).AmountPaid) / (datasource.ElementAt(i).CurrencyValue));
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