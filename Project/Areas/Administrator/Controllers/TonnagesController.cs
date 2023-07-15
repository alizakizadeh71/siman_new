using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class TonnagesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        private void Viewdata(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text); /// ممتازان کرمان

            var Tonage = UnitOfWork.tonnageRepository.GetByPackageTypeId(cementViewModel.PackageType).ToList();
            base.ViewData["Tonage"] = new System.Web.Mvc.SelectList(Tonage, "Id", "Name", cementViewModel.Tonnage).OrderBy(x => x.Text);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.tonnageRepository.Get().Where(x => x.IsActived && !x.IsDeleted);

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            StringPackageType = current.PackageType.Name,
                            StringTonnage = current.Name,
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


            var ofindsubheadline =
                 UnitOfWork.tonnageRepository.Get()
                 .Where(x => x.IsActived && !x.IsDeleted)
                 .Where(model => model.PackageTypeId == cementViewModel.PackageType)
                 .Where(model => model.Name == cementViewModel.StringTonnage)
                 .FirstOrDefault();


            if (ofindsubheadline != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

            else if (cementViewModel.StringTonnage == null || cementViewModel.code == null)
                ViewBag.PageMessages = "فیلد های تناژ و کد نباید خالی باشد";

            else
            {
                Models.Tonnage tonnage = new Models.Tonnage();
                tonnage.PackageTypeId = cementViewModel.PackageType;
                tonnage.Name = cementViewModel.StringTonnage;
                tonnage.Code = cementViewModel.code;
                UnitOfWork.tonnageRepository.Insertdata(tonnage);
                UnitOfWork.Save();

                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }

            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(HttpStatusCode.BadRequest)));
            }

            var oAccountNumberManage
                = UnitOfWork.tonnageRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    
                    Id = current.Id,
                    PackageType = current.PackageTypeId,
                    StringTonnage = current.Name,
                    code = current.Code,
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
        public virtual System.Web.Mvc.ActionResult Delete(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)

        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.tonnageRepository.Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.tonnageRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.Tonnages.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }
    }
}