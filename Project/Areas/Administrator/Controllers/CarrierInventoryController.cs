using System;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.CarrierInventory;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class CarrierInventoryController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var viewModel = new CarrierInventoryViewModel();
            Viewdata(viewModel);
            return View(viewModel);
        }

        private void Viewdata(CarrierInventoryViewModel viewModel)
        {
            var carrierRoleId = UnitOfWork.RoleRepository.Get()
                .Where(x => x.Code == (int)Enums.Roles.Carrier)
                .Select(x => x.Id)
                .FirstOrDefault();

            // لیست باربری‌ها — فقط کاربران با Role = Carrier
            var carriers = UnitOfWork.UserRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted && x.RoleId == carrierRoleId)
                .ToList();


            ViewData["Carrier"] = new SelectList(carriers, "Id", "FullName", viewModel.CarrierId);

            var productNames = UnitOfWork.ProductNameRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["ProductName"] = new SelectList(productNames, "Id", "Name", viewModel.ProductNameId);

            var productTypes = UnitOfWork.ProductTypeRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["ProductType"] = new SelectList(productTypes, "Id", "Name", viewModel.ProductTypeId);

            var packageTypes = UnitOfWork.PackageTypeRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["PackageType"] = new SelectList(packageTypes, "Id", "Name", viewModel.PackageTypeId);

            var factoryNames = UnitOfWork.FactoryNameRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["FactoryName"] = new SelectList(factoryNames, "Id", "Name", viewModel.FactoryNameId);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual ActionResult Search(CarrierInventoryViewModel viewModel)
        {
            var list = UnitOfWork.CarrierInventoryRepository
                .GetCarrierInventories()
                .OrderByDescending(x => x.InsertDateTime)
                .ToList()
                .Select(x => new CarrierInventoryViewModel
                {
                    Id = x.Id,
                    IsDefaultCarrier = x.IsDefaultCarrier,
                    StringCarrierName = x.Carrier != null ? x.Carrier.FullName : "-",
                    StringProductName = x.ProductName != null ? x.ProductName.Name : "-",
                    StringProductType = x.ProductType != null ? x.ProductType.Name : "-",
                    StringPackageType = x.PackageType != null ? x.PackageType.Name : "-",
                    StringFactoryName = x.FactoryName != null ? x.FactoryName.Name : "-",
                    StringInventoryTonnage = x.InventoryTonnage.ToString("N0"),
                    StringIsDefaultCarrier = x.IsDefaultCarrier ? "بله" : "خیر",
                    StringInsertDateTime = new Infrastructure.Calander(x.InsertDateTime).Persion(),
                })
                .AsQueryable();

            var result = Utilities.Kendo.HtmlHelpers
                .ParseGridData<CarrierInventoryViewModel>(list);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Create()
        {
            ViewBag.PageMessages = null;
            var viewModel = new CarrierInventoryViewModel();
            Viewdata(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Create(CarrierInventoryViewModel viewModel)
        {
            ViewBag.PageMessages = null;
            try
            {
                if (viewModel.CarrierId == Guid.Empty ||
                    viewModel.ProductNameId == Guid.Empty ||
                    viewModel.ProductTypeId == Guid.Empty ||
                    viewModel.PackageTypeId == Guid.Empty ||
                    viewModel.FactoryNameId == Guid.Empty)
                {
                    ViewBag.PageMessages = "لطفاً تمام فیلدها را تکمیل نمایید.";
                    Viewdata(viewModel);
                    return View(viewModel);
                }

                if (viewModel.InventoryTonnage < 0)
                {
                    ViewBag.PageMessages = "موجودی نمی‌تواند منفی باشد.";
                    Viewdata(viewModel);
                    return View(viewModel);
                }

                // قانون ۴: موجودی باربری نباید از موجودی شرکت بیشتر باشد
                var companyInventory = UnitOfWork.InventoryamountRepository.Get()
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Where(x => x.ProductNameId == viewModel.ProductNameId)
                    .Where(x => x.ProductTypeId == viewModel.ProductTypeId)
                    .Where(x => x.PackageTypeId == viewModel.PackageTypeId)
                    .Where(x => x.FactoryNameId == viewModel.FactoryNameId)
                    .FirstOrDefault();

                double totalCarrierInventory =
                    UnitOfWork.CarrierInventoryRepository
                        .GetCarrierInventories()
                        .Where(x =>
                            !x.IsDeleted &&
                            x.ProductNameId == viewModel.ProductNameId &&
                            x.ProductTypeId == viewModel.ProductTypeId &&
                            x.PackageTypeId == viewModel.PackageTypeId &&
                            x.FactoryNameId == viewModel.FactoryNameId)
                        .Sum(x => x.InventoryTonnage);

                double remainCompanyInventory =
                    companyInventory.Inventorytonnage - totalCarrierInventory;

                if (companyInventory == null)
                {
                    ViewBag.PageMessages = "موجودی شرکت برای این محصول تعریف نشده است.";
                    Viewdata(viewModel);
                    return View(viewModel);
                }

                if (viewModel.InventoryTonnage > companyInventory.Inventorytonnage)
                {
                    ViewBag.PageMessages = $"موجودی باربری نمی‌تواند از موجودی شرکت ({companyInventory.Inventorytonnage} تن) بیشتر باشد.";
                    Viewdata(viewModel);
                    return View(viewModel);
                }

                if (viewModel.InventoryTonnage > remainCompanyInventory)
                {
                    ViewBag.PageMessages =
                        $"موجودی آزاد شرکت فقط {remainCompanyInventory:N0} تن است.";

                    Viewdata(viewModel);
                    return View(viewModel);
                }

                // قانون ۱: اگر پیش‌فرض انتخاب شد، بقیه رو false کن
                if (viewModel.IsDefaultCarrier)
                {
                    var previousDefaults = UnitOfWork.CarrierInventoryRepository
                        .GetByProduct(
                            viewModel.ProductNameId,
                            viewModel.ProductTypeId,
                            viewModel.PackageTypeId,
                            viewModel.FactoryNameId)
                        .Where(x => x.IsDefaultCarrier)
                        .ToList();

                    foreach (var item in previousDefaults)
                    {
                        item.IsDefaultCarrier = false;
                        item.UpdateDateTime = DateTime.Now;
                        UnitOfWork.CarrierInventoryRepository.Update(item);
                    }
                    UnitOfWork.Save();
                }

                var entity = new Models.CarrierInventory
                {
                    Id = Guid.NewGuid(),
                    CarrierId = viewModel.CarrierId,
                    ProductNameId = viewModel.ProductNameId,
                    ProductTypeId = viewModel.ProductTypeId,
                    PackageTypeId = viewModel.PackageTypeId,
                    FactoryNameId = viewModel.FactoryNameId,
                    InventoryTonnage = viewModel.InventoryTonnage,
                    IsDefaultCarrier = viewModel.IsDefaultCarrier,
                    IsActived = true,
                    IsDeleted = false,
                    InsertDateTime = DateTime.Now,
                };

                UnitOfWork.CarrierInventoryRepository.Insertdata(entity);
                UnitOfWork.Save();

                ViewBag.PageMessages = "موجودی باربری با موفقیت ثبت گردید.";
            }
            catch (Exception ex)
            {
                ViewBag.PageMessages = "خطا در ثبت اطلاعات.";
            }

            Viewdata(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual ActionResult Delete(Guid id)
        {
            var item = UnitOfWork.CarrierInventoryRepository
                .GetCarrierInventories()
                .Where(x => x.Id == id)
                .ToList()
                .Select(x => new CarrierInventoryViewModel
                {
                    Id = x.Id,
                    StringCarrierName = x.Carrier != null ? x.Carrier.FullName : "-",
                    StringProductName = x.ProductName != null ? x.ProductName.Name : "-",
                    StringProductType = x.ProductType != null ? x.ProductType.Name : "-",
                    StringPackageType = x.PackageType != null ? x.PackageType.Name : "-",
                    StringFactoryName = x.FactoryName != null ? x.FactoryName.Name : "-",
                    StringInventoryTonnage = x.InventoryTonnage.ToString("N0"),
                    StringIsDefaultCarrier = x.IsDefaultCarrier ? "بله" : "خیر",
                    StringInsertDateTime = new Infrastructure.Calander(x.InsertDateTime).Persion(),
                })
                .FirstOrDefault();

            if (item == null)
                return RedirectToAction("Index", "CarrierInventory", new { area = "Administrator" });

            return View(item);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual ActionResult Delete(CarrierInventoryViewModel viewModel)
        {
            try
            {
                var entity = UnitOfWork.CarrierInventoryRepository
                    .GetCarrierInventories()
                    .Where(x => x.Id == viewModel.Id)
                    .FirstOrDefault();

                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.IsActived = false;
                    entity.UpdateDateTime = DateTime.Now;
                    UnitOfWork.CarrierInventoryRepository.Update(entity);
                    UnitOfWork.Save();
                }

                return RedirectToAction("Index", "CarrierInventory", new { area = "Administrator" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Display", "Error", new { area = "", id = (int)System.Net.HttpStatusCode.NotFound });
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual JsonResult SetDefault(Guid id)
        {
            try
            {
                // پیدا کردن رکورد مورد نظر
                var target = UnitOfWork.CarrierInventoryRepository
                    .GetCarrierInventories()
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (target == null)
                    return Json(new { Success = false, Message = "رکورد یافت نشد." });

                // false کردن تمام پیش‌فرض‌های قبلی همین محصول
                var previousDefaults = UnitOfWork.CarrierInventoryRepository
                    .GetByProduct(
                        target.ProductNameId,
                        target.ProductTypeId,
                        target.PackageTypeId,
                        target.FactoryNameId)
                    .Where(x => x.IsDefaultCarrier)
                    .ToList();

                foreach (var item in previousDefaults)
                {
                    item.IsDefaultCarrier = false;
                    item.UpdateDateTime = DateTime.Now;
                    UnitOfWork.CarrierInventoryRepository.Update(item);
                }

                // تعیین پیش‌فرض جدید
                target.IsDefaultCarrier = true;
                target.UpdateDateTime = DateTime.Now;
                UnitOfWork.CarrierInventoryRepository.Update(target);

                UnitOfWork.Save();

                return Json(new { Success = true, Message = "باربری پیش‌فرض با موفقیت تغییر کرد." });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "خطا در انجام عملیات." });
            }
        }

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult IncreaseInventory(Guid id)
        {
            var item = UnitOfWork.CarrierInventoryRepository
                .GetCarrierInventories()
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
                return RedirectToAction("Index");

            var viewModel = new CarrierInventoryViewModel
            {
                Id = item.Id,

                CarrierId = item.CarrierId,

                ProductNameId = item.ProductNameId,
                ProductTypeId = item.ProductTypeId,
                PackageTypeId = item.PackageTypeId,
                FactoryNameId = item.FactoryNameId,

                InventoryTonnage = item.InventoryTonnage,

                StringCarrierName = item.Carrier.FullName,
                StringProductName = item.ProductName.Name,
                StringProductType = item.ProductType.Name,
                StringPackageType = item.PackageType.Name,
                StringFactoryName = item.FactoryName.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult IncreaseInventory(Guid id, double addedTonnage)
        {
            try
            {
                if (addedTonnage <= 0)
                {
                    TempData["Message"] = "مقدار افزایش باید بزرگتر از صفر باشد.";
                    return RedirectToAction("IncreaseInventory", new { id });
                }

                var entity = UnitOfWork.CarrierInventoryRepository
                    .GetCarrierInventories()
                    .FirstOrDefault(x => x.Id == id);

                if (entity == null)
                {
                    TempData["Message"] = "رکورد مورد نظر یافت نشد.";
                    return RedirectToAction("Index");
                }

                //---------------------------------
                // موجودی شرکت
                //---------------------------------

                var companyInventory =
                    UnitOfWork.InventoryamountRepository
                    .Get()
                    .FirstOrDefault(x =>
                        x.IsActived &&
                        !x.IsDeleted &&
                        x.ProductNameId == entity.ProductNameId &&
                        x.ProductTypeId == entity.ProductTypeId &&
                        x.PackageTypeId == entity.PackageTypeId &&
                        x.FactoryNameId == entity.FactoryNameId);

                if (companyInventory == null)
                {
                    TempData["Message"] = "موجودی شرکت برای این محصول تعریف نشده است.";
                    return RedirectToAction("IncreaseInventory", new { id });
                }

                //---------------------------------
                // مجموع موجودی تمام باربری‌ها
                //---------------------------------

                double totalCarrierInventory =
                    UnitOfWork.CarrierInventoryRepository
                    .GetCarrierInventories()
                    .Where(x =>
                        x.IsActived &&
                        !x.IsDeleted &&
                        x.ProductNameId == entity.ProductNameId &&
                        x.ProductTypeId == entity.ProductTypeId &&
                        x.PackageTypeId == entity.PackageTypeId &&
                        x.FactoryNameId == entity.FactoryNameId)
                    .Sum(x => x.InventoryTonnage);

                //---------------------------------
                // مجموع بعد از افزایش
                //---------------------------------

                double totalAfterIncrease =
                    totalCarrierInventory + addedTonnage;

                if (totalAfterIncrease > companyInventory.Inventorytonnage)
                {
                    TempData["Message"] =
                        $"امکان افزایش موجودی وجود ندارد. مجموع موجودی باربری‌ها از موجودی شرکت ({companyInventory.Inventorytonnage:N0} تن) بیشتر می‌شود.";

                    return RedirectToAction("IncreaseInventory", new { id });
                }

                //---------------------------------
                // افزایش موجودی
                //---------------------------------

                entity.InventoryTonnage += addedTonnage;
                entity.UpdateDateTime = DateTime.Now;

                UnitOfWork.CarrierInventoryRepository.Update(entity);
                UnitOfWork.Save();

                TempData["Message"] = "موجودی با موفقیت افزایش یافت.";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Message"] = "خطا در افزایش موجودی.";
                return RedirectToAction("IncreaseInventory", new { id });
            }
        }
    }
}