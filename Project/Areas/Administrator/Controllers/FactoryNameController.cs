using DAL;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Cement;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class FactoryNameController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/FactoryName
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        private void Viewdata(CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);
            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text);
            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text);
            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.FactoryNameRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted);

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            StringProductName = current.ProductName.Name,
                            StringFactoryName = current.Name,
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
                 UnitOfWork.FactoryNameRepository.Get()
                 .Where(x => x.IsActived && !x.IsDeleted)
                 .Where(model => model.ProductNameId == cementViewModel.ProductName)
                 .Where(model => model.Name == cementViewModel.StringFactoryName)
                 .Where(model => model.Code == cementViewModel.code)
                 .FirstOrDefault();


            if (ofindsubheadline != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

            else if (cementViewModel.StringFactoryName == null || cementViewModel.code == null)
                ViewBag.PageMessages = "فیلد های نام شرکت و کد نباید خالی باشد";

            else
            {
                Models.FactoryName factoryName = new Models.FactoryName();
                factoryName.ProductNameId = cementViewModel.ProductName;
                factoryName.Name = cementViewModel.StringFactoryName;
                factoryName.Code = cementViewModel.code;
                UnitOfWork.FactoryNameRepository.Insertdata(factoryName);
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
                = UnitOfWork.FactoryNameRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    ProductName = current.ProductName.Id,
                    StringFactoryName = current.Name,
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
                    UnitOfWork.FactoryNameRepository.Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.FactoryNameRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.FactoryName.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }
    }
}