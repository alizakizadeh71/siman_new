using Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class PreInvoiceController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var ProductName = UnitOfWork.ProductNameRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["ProductName"] =
                new SelectList(ProductName, "Id", "Name", null)
                .OrderByDescending(x => x.Text);



            var ProductType = UnitOfWork.ProductTypeRepository
                .GetByProductNameId(Guid.Empty)
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["ProductType"] =
                new SelectList(ProductType, "Id", "Name", null)
                .OrderByDescending(x => x.Text);



            var PackageType = UnitOfWork.PackageTypeRepository
                .GetByProductTypeId(Guid.Empty)
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["PackageType"] =
                new SelectList(PackageType, "Id", "Name", null)
                .OrderByDescending(x => x.Text);



            var FactoryName = UnitOfWork.FactoryNameRepository
                .GetByProductNameId(Guid.Empty)
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["FactoryName"] =
                new SelectList(FactoryName, "Id", "Name", null)
                .OrderBy(x => x.Text);



            var varProvinces = UnitOfWork.ProvinceRepository
                .Get(Infrastructure.Sessions.AuthenticatedUser.User)
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["Province"] =
                new SelectList(varProvinces, "Id", "Name", null);



            var varCities = UnitOfWork.CityRepository
                .GetByProvinceId(Guid.Empty)
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            base.ViewData["City"] =
                new SelectList(varCities, "Id", "Name", null);



            var viewModel =
                new ViewModels.Areas.Administrator.Request.IndexViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual JsonResult GetRequests() => (JsonResult)Search(null);

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Search(ViewModels.Areas.Administrator.Request.IndexViewModel viewModel)
        {
            var varRequest = UnitOfWork.FactorCementRepository
                .Get()
                .Where(x =>
                    x.IsManualPreInvoice &&
                    !x.FinalApprove &&
                    !x.IsDeleted);

            if (viewModel != null)
            {
                #region Condition
                if (!string.IsNullOrEmpty(viewModel.BuyerMobile))
                {
                    viewModel.BuyerMobile = Utilities.Text.Utility.FixText(viewModel.BuyerMobile);
                    varRequest = varRequest.Where(x => x.BuyerMobile == viewModel.BuyerMobile);
                }
                if (viewModel.InvoiceNumber != null && viewModel.InvoiceNumber != 0)
                    varRequest = varRequest.Where(x => x.InvoiceNumber == viewModel.InvoiceNumber);

                if (viewModel.ProductName != null && viewModel.ProductName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProductNameId == viewModel.ProductName);

                if (viewModel.ProductType != null && viewModel.ProductType != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProductTypeId == viewModel.ProductType);

                if (viewModel.PackageType != null && viewModel.PackageType != Guid.Empty)
                    varRequest = varRequest.Where(x => x.PackageTypeId == viewModel.PackageType);

                if (viewModel.FactoryName != null && viewModel.FactoryName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.FactoryNameId == viewModel.FactoryName);

                if (viewModel.Province != null && viewModel.Province != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);

                if (viewModel.City != null && viewModel.City != Guid.Empty)
                    varRequest = varRequest.Where(x => x.CityId == viewModel.City);

                if (viewModel.FromAmount != null && viewModel.ToAmount != null && viewModel.FromAmount > 0 && viewModel.ToAmount > 0 && viewModel.FromAmount <= viewModel.ToAmount)
                    varRequest = varRequest.Where(current => current.AmountPaid >= viewModel.FromAmount && current.AmountPaid <= viewModel.ToAmount);

                if (viewModel.StartDate != null)
                    varRequest = varRequest.Where(current => current.InsertDateTime >= viewModel.StartDate);

                if (viewModel.EndDate != null)
                {
                    var EndDate2 = viewModel.EndDate.Value.AddDays(1);
                    varRequest = varRequest.Where(current => current.InsertDateTime < EndDate2);
                }
                #endregion
            }

            try
            {
                var mappedRequests = varRequest
                    .OrderByDescending(current => current.InvoiceNumber)
                    .ToList()
                    .Select(current => new ViewModels.Areas.Administrator.Request.IndexViewModel()
                    {
                        Id = current.Id,
                        InvoiceNumber = current.InvoiceNumber,
                        StringProductName = current.ProductName?.Name,
                        StringProductType = current.ProductType?.Name,
                        StringPackageType = current.PackageType?.Name,
                        StringFactoryName = current.FactoryName?.Name,
                        StringTonnage = current.Tonnagedouble.ToString(),
                        Address = current.Address,
                        BuyerMobile = current.BuyerMobile,
                        // اعمال لاجیک نام خریدار: اگر دستی پر شده بود آن را نمایش بده، در غیر این صورت نام یوزر سیستم را نشان بده
                        BuyerName = !string.IsNullOrEmpty(current.ManualBuyerName) ? current.ManualBuyerName : current.User?.UserName,
                        Description = current.Description,
                        stringFinalApprove = "پیش فاکتور",
                        AmountPaid = current.AmountPaid,
                        MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" :
                              current.MahalTahvil == "Mahal" ? "مقصد خریدار" :
                              !string.IsNullOrEmpty(current.MahalTahvil) ? current.MahalTahvil : "-",
                        StringCarrierName = current.CarrierUser != null ? current.CarrierUser.FullName : "-",
                        StringInsertDateTime = current.InsertDateTime != null ? new Infrastructure.Calander(current.InsertDateTime).Persion() : "",
                    }).AsQueryable();

                var varResult = Utilities.Kendo.HtmlHelpers.ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(mappedRequests);
                return Json(varResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Data = new System.Collections.Generic.List<string>(), Total = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #region تایید نهایی پیش فاکتور + ارسال پیامک

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult FinalApproveInvoice(Guid id)
        {
            try
            {
                var request = UnitOfWork.FactorCementRepository
                    .Get()
                    .FirstOrDefault(x => x.Id == id);

                if (request == null)
                    return Json(new { success = false, message = "پیش فاکتور یافت نشد" });

                if (request.FinalApprove)
                    return Json(new { success = false, message = "این پیش فاکتور قبلا تایید شده است" });

                request.FinalApprove = true;
                request.RequestState = (int)RequestState.FinanciallyApproved;
                request.UpdateDateTime = DateTime.Now;

                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();

                #region ارسال پیامک

                try
                {
                    string productInfo = string.Format("{0} - {1} - {2} - {3} تن",
                        request.ProductName?.Name ?? "-",
                        request.FactoryName?.Name ?? "-",
                        request.PackageType?.Name ?? "-",
                        request.Tonnagedouble);

                    string buyerName =
                        !string.IsNullOrEmpty(request.ManualBuyerName)
                            ? request.ManualBuyerName
                            : request.User?.FullName ?? "کاربر مهمان";

                    // ── پیامک مشتری ─────────────────────────────────────
                    if (!string.IsNullOrEmpty(request.BuyerMobile) &&
                        request.CarrierUser != null)
                    {
                        bool isGuest =
                            request.User == null ||
                            request.User.FullName == "کاربر مهمان" ||
                            request.User.FullName == "میهمان";

                        if (isGuest)
                        {
                            // مشتری مهمان
                            Utilities.SMS.SmsUtility.SendGuestLoadedNotification(
                                buyerMobile: request.BuyerMobile,
                                buyerName: buyerName,
                                factorNumber: request.InvoiceNumber?.ToString() ?? "-",
                                productName: productInfo,
                                price: request.AmountPaid.ToString("N0"),
                                carrierName: request.CarrierUser.FullName ?? "-",
                                carrierMobile: request.CarrierUser.BuyerMobile ?? "-"
                            );
                        }
                        else
                        {
                            // مشتری عادی
                            long balance = request.User.InitialCredit - request.User.creditAmount;

                            string remainAmount = Math.Abs(balance).ToString("N0");

                            string remainStatus =
                                balance > 0
                                    ? "بدهکار"
                                    : balance < 0
                                        ? "طلبکار"
                                        : "تسویه";

                            Utilities.SMS.SmsUtility.SendLoadedNotification(
                                buyerMobile: request.BuyerMobile,
                                buyerName: buyerName,
                                factorNumber: request.InvoiceNumber?.ToString() ?? "-",
                                productName: productInfo,
                                price: request.AmountPaid.ToString("N0"),
                                remainAmount: remainAmount,
                                remainAmountText: remainStatus,
                                carrierName: request.CarrierUser.FullName ?? "-",
                                carrierMobile: request.CarrierUser.BuyerMobile ?? "-"
                            );
                        }
                    }

                    // ── پیامک باربری ─────────────────────────────────────
                    if (request.CarrierUser != null &&
                        !string.IsNullOrEmpty(request.CarrierUser.BuyerMobile))
                    {
                        bool hasDriverInfo =
                            !string.IsNullOrWhiteSpace(request.DriverName) ||
                            !string.IsNullOrWhiteSpace(request.DriverMobile);

                        if (hasDriverInfo)
                        {
                            Utilities.SMS.SmsUtility.SendCarrierNotificationWithDriver(
                                carrierMobile: request.CarrierUser.BuyerMobile,
                                carrierName: request.CarrierUser.FullName ?? "-",
                                factorNumber: request.InvoiceNumber?.ToString() ?? "-",
                                productName: productInfo,
                                buyerName: buyerName,
                                buyerMobile: request.BuyerMobile ?? "-",
                                driverName: request.DriverName ?? "",
                                driverMobile: request.DriverMobile ?? ""
                            );
                        }
                        else
                        {
                            Utilities.SMS.SmsUtility.SendCarrierNotification(
                                carrierMobile: request.CarrierUser.BuyerMobile,
                                carrierName: request.CarrierUser.FullName ?? "-",
                                factorNumber: request.InvoiceNumber?.ToString() ?? "-",
                                productName: productInfo,
                                buyerName: buyerName,
                                buyerMobile: request.BuyerMobile ?? "-"
                            );
                        }
                    }
                }
                catch
                {
                    // خطای ارسال پیامک باعث عدم تایید پیش فاکتور نشود
                }

                #endregion

                return Json(new { success = true, message = "پیش فاکتور با موفقیت تایید شد و پیامک ارسال گردید" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region رد پیش فاکتور و بازگشت موجودی انبار

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult RejectInvoice(Guid id)
        {
            try
            {
                var factor =
                    UnitOfWork.FactorCementRepository
                    .Get()
                    .FirstOrDefault(x => x.Id == id);

                if (factor == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "پیش فاکتور یافت نشد"
                    });
                }

                // اگر قبلا رد یا حذف شده
                if (factor.IsDeleted)
                {
                    return Json(new
                    {
                        success = false,
                        message = "این پیش فاکتور قبلا رد شده است"
                    });
                }

                #region پیدا کردن موجودی انبار

                var inventory =
                    UnitOfWork.InventoryamountRepository
                    .Get()
                    .FirstOrDefault(x =>
                        x.IsActived &&
                        !x.IsDeleted &&
                        x.ProductNameId == factor.ProductNameId &&
                        x.ProductTypeId == factor.ProductTypeId &&
                        x.PackageTypeId == factor.PackageTypeId &&
                        x.FactoryNameId == factor.FactoryNameId);

                if (inventory == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "رکورد موجودی انبار پیدا نشد"
                    });
                }

                #endregion

                #region بازگرداندن موجودی

                double currentInventory = inventory.Inventorytonnage;

                double factorTonnage = factor.Tonnagedouble;

                inventory.Inventorytonnage =
                    currentInventory + factorTonnage;

                inventory.UpdateDateTime = DateTime.Now;

                UnitOfWork.InventoryamountRepository.Update(inventory);

                #endregion

                #region رد فاکتور

                factor.IsDeleted = true;
                factor.IsActived = false;
                factor.UpdateDateTime = DateTime.Now;

                // وضعیت رد شده
                factor.RequestState = -1;

                UnitOfWork.FactorCementRepository.Update(factor);

                #endregion

                UnitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "پیش فاکتور رد شد و موجودی به انبار بازگشت"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        #endregion
        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create()
        {
            var ProductName = UnitOfWork.ProductNameRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["ProductName"] =
                new SelectList(ProductName, "Id", "Name");



            var ProductType = UnitOfWork.ProductTypeRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["ProductType"] =
                new SelectList(ProductType, "Id", "Name");



            var PackageType = UnitOfWork.PackageTypeRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["PackageType"] =
                new SelectList(PackageType, "Id", "Name");



            var FactoryName = UnitOfWork.FactoryNameRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["FactoryName"] =
                new SelectList(FactoryName, "Id", "Name");



            var Province = UnitOfWork.ProvinceRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["Province"] =
                new SelectList(Province, "Id", "Name");



            var City = UnitOfWork.CityRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["City"] =
                new SelectList(City, "Id", "Name");

            return View(
                new ViewModels.Areas.Administrator.PreInvoice.CreateViewModel()
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create(ViewModels.Areas.Administrator.PreInvoice.CreateViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PopulateCreateViewData();
                    return View(viewModel);
                }

                if (viewModel.CarrierId == null || viewModel.CarrierId == Guid.Empty)
                {
                    ViewBag.ErrorMessage = "انتخاب باربری الزامی است";
                    PopulateCreateViewData();
                    return View(viewModel);
                }

                var user = UnitOfWork.UserRepository.GetByUserName(
                    Infrastructure.Sessions.AuthenticatedUser.UserName);

                int lastInvoiceNumber = UnitOfWork.FactorCementRepository.GetLastInvoiceNumber() + 1;

                var inventory = UnitOfWork.InventoryamountRepository.Get()
                    .FirstOrDefault(x =>
                        x.IsActived && !x.IsDeleted &&
                        x.ProductNameId == viewModel.ProductName &&
                        x.ProductTypeId == viewModel.ProductType &&
                        x.PackageTypeId == viewModel.PackageType &&
                        x.FactoryNameId == viewModel.FactoryName);

                if (inventory == null)
                {
                    ViewBag.ErrorMessage = "برای این محصول موجودی انبار ثبت نشده است";
                    PopulateCreateViewData();
                    return View(viewModel);
                }

                decimal currentInventory = Convert.ToDecimal(inventory.Inventorytonnage);
                decimal requestTonnage = viewModel.Tonnage;

                if (requestTonnage <= 0)
                {
                    ViewBag.ErrorMessage = "تناژ وارد شده معتبر نیست";
                    PopulateCreateViewData();
                    return View(viewModel);
                }

                if (currentInventory < requestTonnage)
                {
                    ViewBag.ErrorMessage = "موجودی انبار کافی نیست";
                    PopulateCreateViewData();
                    return View(viewModel);
                }

                if (viewModel.CarrierId.HasValue)
                {
                    var carrierInventory = UnitOfWork.CarrierInventoryRepository
                        .GetByProduct(
                            viewModel.ProductName,
                            viewModel.ProductType,
                            viewModel.PackageType,
                            viewModel.FactoryName)
                        .FirstOrDefault(x => x.CarrierId == viewModel.CarrierId);

                    if (carrierInventory == null)
                    {
                        ViewBag.ErrorMessage = "موجودی باربری انتخابی برای این محصول ثبت نشده است";
                        PopulateCreateViewData();
                        return View(viewModel);
                    }

                    if (Convert.ToDecimal(carrierInventory.InventoryTonnage) < requestTonnage)
                    {
                        ViewBag.ErrorMessage = $"موجودی باربری کافی نیست ({carrierInventory.InventoryTonnage} تن)";
                        PopulateCreateViewData();
                        return View(viewModel);
                    }

                    carrierInventory.InventoryTonnage -= Convert.ToDouble(requestTonnage);
                    carrierInventory.UpdateDateTime = DateTime.Now;
                    UnitOfWork.CarrierInventoryRepository.Update(carrierInventory);
                }

                var factor = new Models.FactorCement
                {
                    InvoiceNumber = lastInvoiceNumber,
                    ProductNameId = viewModel.ProductName,
                    ProductTypeId = viewModel.ProductType,
                    PackageTypeId = viewModel.PackageType,
                    FactoryNameId = viewModel.FactoryName,
                    ProvinceId = viewModel.Province,
                    CityId = viewModel.City,
                    Tonnagedouble = Convert.ToDouble(viewModel.Tonnage),
                    BuyerMobile = viewModel.BuyerMobile,
                    ManualBuyerName = viewModel.BuyerName,
                    Address = viewModel.Address,
                    AmountPaid = viewModel.AmountPaid,
                    Description = viewModel.Description,
                    MahalTahvil = viewModel.MahalTahvil,
                    CarrierId = viewModel.CarrierId,
                    DriverName = viewModel.DriverName,
                    DriverLastName = viewModel.DriverLastName,
                    DriverMobile = viewModel.DriverMobile,
                    DriverLicensePlate = viewModel.DriverLicensePlate,
                    UserId = user.Id,
                    RequestState = 0,
                    FinalApprove = false,
                    IsManualPreInvoice = true,
                    UserIPAddress = Request.UserHostAddress,
                    Browser = Request.Browser?.Type,
                    URLAddress = UnitOfWork.SubSystemRepository.Get().FirstOrDefault()?.UrlTo,
                    InsertDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    IsActived = true,
                    IsDeleted = false
                };

                UnitOfWork.FactorCementRepository.Insertdata(factor);

                inventory.Inventorytonnage = Convert.ToDouble(currentInventory - requestTonnage);
                inventory.UpdateDateTime = DateTime.Now;
                UnitOfWork.InventoryamountRepository.Update(inventory);

                UnitOfWork.Save();

                TempData["SuccessMessage"] = "پیش فاکتور با موفقیت ثبت شد";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                PopulateCreateViewData();
                return View(viewModel);
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual JsonResult GetLivePriceCalc(Guid productNameId, Guid productTypeId, Guid packageTypeId, Guid factoryNameId, decimal tonnage)
        {
            try
            {
                var financial = UnitOfWork.FinancialManagementRepository
                    .Get()
                    .Where(x =>
                        x.IsActived &&
                        !x.IsDeleted &&
                        x.ProductNameId == productNameId &&
                        x.ProductTypeId == productTypeId &&
                        x.PackageTypeId == packageTypeId &&
                        x.FactoryNameId == factoryNameId &&
                        x.AmountPaid != 0)
                    .OrderByDescending(x => x.InsertDateTime)
                    .FirstOrDefault();

                if (financial == null)
                {
                    return Json(new { success = false, amount = 0 }, JsonRequestBehavior.AllowGet);
                }

                // قیمت واحد (مثلاً هر تن)
                decimal unitPrice = Convert.ToDecimal(financial.AmountPaid);

                // محاسبه نهایی بر اساس تناژ
                decimal total = unitPrice * tonnage;

                return Json(new
                {
                    success = true,
                    amount = total
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // دریافت اطلاعات یک باربری خاص
        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult GetCarrierInfo(Guid carrierId)
        {
            try
            {
                var carrierRoleId = UnitOfWork.RoleRepository.Get()
                    .Where(x => x.Code == (int)Enums.Roles.Carrier)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                var carrier = UnitOfWork.UserRepository.Get()
                    .Where(x => x.Id == carrierId && x.RoleId == carrierRoleId && x.IsActived && !x.IsDeleted)
                    .FirstOrDefault();

                if (carrier == null)
                    return Json(new { success = false });

                return Json(new
                {
                    success = true,
                    FullName = carrier.FullName,
                    BuyerMobile = carrier.BuyerMobile
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // دریافت لیست باربری‌ها بر اساس محصول
        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult GetCarriersByProduct(Guid productNameId, Guid productTypeId, Guid packageTypeId, Guid factoryNameId)
        {
            try
            {
                var carriers = UnitOfWork.CarrierInventoryRepository
                    .GetByProduct(productNameId, productTypeId, packageTypeId, factoryNameId)
                    .OrderByDescending(x => x.IsDefaultCarrier)
                    .ToList()
                    .Select(x => new
                    {
                        Id = x.CarrierId,
                        Name = x.Carrier != null ? x.Carrier.FullName : "-",
                        IsDefault = x.IsDefaultCarrier
                    });

                return Json(carriers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void PopulateCreateViewData()
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["ProductName"] = new SelectList(ProductName, "Id", "Name");

            var ProductType = UnitOfWork.ProductTypeRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["ProductType"] = new SelectList(ProductType, "Id", "Name");

            var PackageType = UnitOfWork.PackageTypeRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["PackageType"] = new SelectList(PackageType, "Id", "Name");

            var FactoryName = UnitOfWork.FactoryNameRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["FactoryName"] = new SelectList(FactoryName, "Id", "Name");

            var Province = UnitOfWork.ProvinceRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["Province"] = new SelectList(Province, "Id", "Name");

            var City = UnitOfWork.CityRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted).ToList();
            ViewData["City"] = new SelectList(City, "Id", "Name");
        }
    }
}
