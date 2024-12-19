using System;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Inventoryamount;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class InventoryamountController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            InventoryamountViewModel InventoryamountViewModel = new InventoryamountViewModel();
            Viewdata(InventoryamountViewModel);
            return View(InventoryamountViewModel);
        }
        private void Viewdata(InventoryamountViewModel InventoryamountViewModel)
        {
            var Inventoryamount = UnitOfWork.InventoryamountRepository.Get().ToList(); /// سیمان
            base.ViewData["Inventoryamount"] = new System.Web.Mvc.SelectList(Inventoryamount, "Id", "Name", InventoryamountViewModel.Inventorytonnage).OrderBy(x => x.Text);
            var ProductName = UnitOfWork.ProductNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", InventoryamountViewModel.ProductName).OrderByDescending(x => x.Text);
            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(InventoryamountViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", InventoryamountViewModel.ProductType).OrderByDescending(x => x.Text);
            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(InventoryamountViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", InventoryamountViewModel.PackageType).OrderByDescending(x => x.Text);
            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(InventoryamountViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", InventoryamountViewModel.FactoryName).OrderBy(x => x.Text);
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(InventoryamountViewModel ViewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();
            var varRequest =
                UnitOfWork.InventoryamountRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted);

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)

                    .ToList()
                    .Select(current =>
                        new InventoryamountViewModel()
                        {
                            Id = current.Id,
                            Inventorytonnage = current.Inventorytonnage,
                            ProductName = current.ProductNameId,
                            stringProductName = current.ProductName.Name,
                            ProductType = current.ProductTypeId,
                            stringProductType = current.ProductType.Name,
                            PackageType = current.PackageTypeId,
                            stringPackageType = current.PackageType.Name,
                            FactoryName = current.FactoryNameId,
                            stringFactoryName = current.FactoryName.Name
                        })
                        .AsQueryable();

                var ssss = ViewModelsvarBanks.ToList();
                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<InventoryamountViewModel>(ViewModelsvarBanks);

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
            InventoryamountViewModel InventoryamountViewModel = new InventoryamountViewModel();
            Viewdata(InventoryamountViewModel);
            return View(InventoryamountViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(InventoryamountViewModel ViewModel)
        {
            ViewBag.PageMessages = null;

            var ofindsubheadline =
                 UnitOfWork.InventoryamountRepository.Get()
                 .Where(x => x.IsActived && !x.IsDeleted)
                 //.Where(model => model.Id == ViewModel.Id)
                 .Where(model => model.ProductNameId == ViewModel.ProductName)
                 .Where(model => model.ProductTypeId == ViewModel.ProductType)
                 .Where(model => model.PackageTypeId == ViewModel.PackageType)
                 .Where(model => model.FactoryNameId == ViewModel.FactoryName)
                 //.Where(model => model.Code == cementViewModel.code)
                 .FirstOrDefault();

            if (ofindsubheadline != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

            else if (ViewModel.Inventorytonnage == null)
                ViewBag.PageMessages = "باید میزان موجودی را مشخص کنید ";
            else
            {
                Models.Inventoryamount Inventoryamount = new Models.Inventoryamount();
                Inventoryamount.Inventorytonnage = ViewModel.Inventorytonnage;
                Inventoryamount.ProductNameId = ViewModel.ProductName;
                Inventoryamount.ProductTypeId = ViewModel.ProductType;
                Inventoryamount.PackageTypeId = ViewModel.PackageType;
                Inventoryamount.FactoryNameId = ViewModel.FactoryName;
                Inventoryamount.InsertDateTime = DateTime.Now;
                UnitOfWork.InventoryamountRepository.Insertdata(Inventoryamount);
                UnitOfWork.Save();

                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }
            Viewdata(ViewModel);
            return View(ViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            InventoryamountViewModel InventoryamountViewModel
             = UnitOfWork.InventoryamountRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new InventoryamountViewModel()
                {
                    Id = current.Id,
                    stringProductName = current.ProductName.Name,
                    stringProductType = current.ProductType.Name,
                    stringPackageType = current.PackageType.Name,
                    stringFactoryName = current.FactoryName.Name,
                    Inventorytonnage = current.Inventorytonnage
                })
                .FirstOrDefault()
                ;

            Viewdata(InventoryamountViewModel);
            ViewBag.PageMessages = null;

            return (View(InventoryamountViewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(InventoryamountViewModel InventoryamountViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.InventoryamountRepository
                    .Get()
                    .Where(current => current.Id == InventoryamountViewModel.Id)
                    .FirstOrDefault()
                    ;
                // **************************************************
                OlderAccount.Inventorytonnage = InventoryamountViewModel.Inventorytonnage;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.InventoryamountRepository.Update(OlderAccount);
                UnitOfWork.Save();

                // **************************************************
                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ویرایش گردید  ";
                return View(InventoryamountViewModel);
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
                = UnitOfWork.InventoryamountRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new InventoryamountViewModel()
                {
                    Id = current.Id,
                    stringProductName = current.ProductName.Name,
                    stringProductType = current.ProductType.Name,
                    stringPackageType = current.PackageType.Name,
                    stringFactoryName = current.PackageType.Name,
                    Inventorytonnage = current.Inventorytonnage
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
        public virtual System.Web.Mvc.ActionResult Delete(InventoryamountViewModel InventoryamountViewModel)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.InventoryamountRepository.Get()
                    .Where(current => current.Id == InventoryamountViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.InventoryamountRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.Inventoryamount.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }
    }
}