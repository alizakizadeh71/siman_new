using DAL;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViewModels;
using ViewModels.Account;
using ViewModels.Areas.Administrator.Inventoryamount;

//using PAPUtilities;


namespace OPS.Controllers
{
    public partial class HomeMainController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Index(Guid? productId = null, Guid? typeId = null, Guid? packageId = null, Guid? factoryId = null)
        {
            // 1. خواندن تنظیمات سایت از کش
            var siteSetting =
                    UnitOfWork.SiteSettingRepository.Get()
                    .Select(s => new { s.IsPurchaseEnabled })
                    .FirstOrDefault();

            bool isPurchaseEnabled = siteSetting?.IsPurchaseEnabled ?? true;
            ViewBag.IsPurchaseEnabled = isPurchaseEnabled;

            // 2. مدیریت پیام‌ها و اطلاعات کاربر
            if (!isPurchaseEnabled)
            {
                ViewBag.PageMessages = "در حال حاضر امکان ثبت سفارش وجود ندارد.";
                // در صورت غیرفعال بودن خرید، مدل حداقلی ساخته و برمی‌گردد
                var modelDisabled = new ViewModels.Areas.Administrator.Cement.CementViewModel();
                ViewData(modelDisabled);
                return View(modelDisabled);
            }

            ViewBag.Message = "برای محاسبه قیمت و خرید اطلاعات را تکمیل نمایید";

            Guid? currentUserId = Infrastructure.Sessions.AuthenticatedUser?.Id;
            bool isNormalUser = currentUserId != null && Infrastructure.Sessions.AuthenticatedUser.RoleCode != 1000;

            string address = string.Empty, buyerMobile = string.Empty, buyerFullName = string.Empty;
            Guid? province = null, city = null;

            if (isNormalUser)
            {
                var user = UnitOfWork.UserRepository.GetById(currentUserId.Value);
                if (user != null)
                {
                    ViewBag.DisplaycreditAmount = $"{user.FullName} خوش آمدید. موجودی کیف پول شما: {user.creditAmount:N0} ریال";
                    province = user.ProvinceId;
                    city = user.CityId;
                    address = user.Address;
                    buyerMobile = user.BuyerMobile;
                    buyerFullName = user.FullName;
                }
            }

            if (TempData["WelcomeMessage"] != null)
            {
                ViewBag.WelcomeMessage = TempData["WelcomeMessage"];
                ViewBag.Balance = TempData["Balance"];
            }

            if (TempData["Error"] != null)
            {
                ViewBag.ErrorMessage = TempData["Error"];
            }

            // 3. تعیین مقادیر پیش‌فرض با حداقل کوئری‌ها 

            // محصول پیش‌فرض
            Guid defaultProductId = UnitOfWork.ProductNameRepository.Get()
                .Where(x => x.Code == "1")
                .Select(x => x.Id)
                .FirstOrDefault();

            Guid selectedProduct = productId ?? defaultProductId;

            // انتخاب خودکار نوع و بسته‌بندی 
            Guid? selectedProductType = typeId;
            Guid? selectedPackageType = packageId;

            if (selectedProductType == null || selectedPackageType == null)
            {
                var preferredTypes = new List<string> { "تیپ 2", "تیپ 5", "پوزولانی", "مرکب" };

                // --- بخش اصلاح شده برای رفع خطای System.NotSupportedException ---

                // مرحله الف: دریافت ProductType ها و انتقال به حافظه
                var productTypes = UnitOfWork.ProductTypeRepository.Get()
                    .Where(t => t.ProductNameId == selectedProduct && preferredTypes.Contains(t.Name))
                    .ToList();

                var productTypeIds = productTypes.Select(t => t.Id).ToList();

                // مرحله ب: دریافت PackageType های مرتبط (با نام "کیسه") و انتقال به حافظه
                var packageTypes = UnitOfWork.PackageTypeRepository.Get()
                    .Where(p => productTypeIds.Contains(p.ProductTypeId) && p.Name == "کیسه")
                    .ToList();

                // مرحله ج: ترکیب داده‌ها در حافظه (LINQ to Objects)
                var typePackageData = productTypes.Select(t => new
                {
                    Type = t,
                    BagPackageId = packageTypes
                        .Where(p => p.ProductTypeId == t.Id)
                        .Select(p => (Guid?)p.Id)
                        .FirstOrDefault()
                }).ToList();

                // ---------------------------------------------------------------

                var selectedTuple = typePackageData
                    .Where(x => x.BagPackageId != null)
                    .OrderBy(x => preferredTypes.IndexOf(x.Type.Name))
                    .FirstOrDefault();

                if (selectedTuple != null)
                {
                    selectedProductType = selectedProductType ?? selectedTuple.Type.Id;
                    selectedPackageType = selectedPackageType ?? selectedTuple.BagPackageId;
                }
            }

            // کارخانه پیش‌فرض (استفاده از ?Guid به جای Guid)
            Guid? selectedFactoryName = factoryId ??
                UnitOfWork.FactoryNameRepository.Get()
                .Where(x => x.ProductNameId == selectedProduct)
                .Select(x => (Guid?)x.Id)
                .FirstOrDefault();

            // 4. ساخت ViewModel نهایی
            var model = new ViewModels.Areas.Administrator.Cement.CementViewModel
            {
                ProductName = selectedProduct,
                ProductType = selectedProductType ?? Guid.Empty,
                PackageType = selectedPackageType ?? Guid.Empty,
                FactoryName = selectedFactoryName ?? Guid.Empty,
                Province = province ?? new Guid("C9CEA679-6DE8-11E5-8295-C0F8DABA7555"),
                City = city ?? new Guid("F99E8A31-0562-4918-8295-2CBFC78D6269"),
                Village = new Guid("7189A747-02F3-11EF-9994-7C8AE1A25092"),
                Address = address,
                BuyerMobile = buyerMobile,
                BuyerFullName = buyerFullName
            };

            ViewData(model);
            return View(model);
        }


        private void ViewData(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text); /// ممتازان کرمان
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual System.Web.Mvc.ActionResult Index(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            try
            {
                ViewBag.PageMessages = null;

                if (ModelState.IsValid)
                {
                    var oFinancialManagement = UnitOfWork.FinancialManagementRepository
                        .Get()
                        .Where(x => x.IsActived && !x.IsDeleted)
                        .Where(current => current.ProductNameId == cementViewModel.ProductName)
                        .Where(current => current.ProductTypeId == cementViewModel.ProductType)
                        .Where(current => current.PackageTypeId == cementViewModel.PackageType)
                        .Where(current => current.FactoryNameId == cementViewModel.FactoryName)
                        .SingleOrDefault();

                    var Tonnage = cementViewModel.Tonnage;

                    // بررسی موجودی انبار
                    var inventoryTonnage = UnitOfWork.InventoryamountRepository.Get()
                        .Where(x => x.ProductNameId == cementViewModel.ProductName)
                        .Where(x => x.ProductTypeId == cementViewModel.ProductType)
                        .Where(x => x.PackageTypeId == cementViewModel.PackageType)
                        .Where(x => x.FactoryNameId == cementViewModel.FactoryName)
                        .Select(x => x.Inventorytonnage)
                        .FirstOrDefault();

                    // بررسی موجودی باربری‌ها
                    var carrierInventories = UnitOfWork.CarrierInventoryRepository
                        .GetByProduct(
                            cementViewModel.ProductName,
                            cementViewModel.ProductType,
                            cementViewModel.PackageType,
                            cementViewModel.FactoryName)
                        .ToList();

                    double totalCarrierInventory = carrierInventories.Sum(x => x.InventoryTonnage);
                    bool hasAnyCarrierStock = carrierInventories.Any(x => x.InventoryTonnage >= Tonnage);

                    if (oFinancialManagement == null)
                    {
                        ViewBag.PageMessages = "قیمت توسط ادمین در سیستم ثبت نشده است.";
                    }
                    else if (Tonnage <= 0)
                    {
                        ViewBag.PageMessages = "تناژ باید بیشتر از صفر باشد.";
                    }
                    else if (Tonnage < 10)
                    {
                        ViewBag.PageMessages = "حداقل تناژ قابل سفارش ۱۰ تن می‌باشد.";
                    }
                    else if (Tonnage > Convert.ToDouble(inventoryTonnage))
                    {
                        ViewBag.PageMessages = $"تناژ درخواستی شما در انبار موجود نمی‌باشد. موجودی فعلی: {inventoryTonnage:N0} تن";
                    }
                    else if (!carrierInventories.Any())
                    {
                        ViewBag.PageMessages = "در حال حاضر هیچ باربری برای این محصول تعریف نشده است. لطفاً با پشتیبانی تماس بگیرید.";
                    }
                    else if (!hasAnyCarrierStock)
                    {
                        ViewBag.PageMessages = $"متأسفانه موجودی هیچ‌یک از باربری‌های این محصول برای تناژ درخواستی کافی نیست. حداکثر تناژ قابل سفارش: {totalCarrierInventory:N0} تن";
                    }
                    else if (Tonnage > Convert.ToDouble(inventoryTonnage))  // ← چک مجدد انبار بعد از باربری
                    {
                        ViewBag.PageMessages = $"موجودی انبار کافی نیست. موجودی فعلی: {inventoryTonnage:N0} تن";
                    }
                    else
                    {
                        var varRequest = UnitOfWork.DestinationManagementRepository.Get()
                            .Where(x => x.IsActived && !x.IsDeleted)
                            .Where(current => current.FinancialManagementId == oFinancialManagement.Id)
                            .Where(current => current.ProvinceId == cementViewModel.Province)
                            .Where(current => current.CityId == cementViewModel.City)
                            .Select(x => x.DestinationAmountPaid)
                            .SingleOrDefault();

                        double? DestinationAmountPaid = varRequest;

                        var oDestinationManagement = UnitOfWork.DestinationManagementRepository
                            .Get()
                            .Where(x => x.IsActived && !x.IsDeleted)
                            .Where(x => x.FinancialManagement.IsActived && !x.FinancialManagement.IsDeleted)
                            .Where(current => current.FinancialManagement.ProductNameId == cementViewModel.ProductName)
                            .Where(current => current.FinancialManagement.ProductTypeId == cementViewModel.ProductType)
                            .Where(current => current.FinancialManagement.PackageTypeId == cementViewModel.PackageType)
                            .Where(current => current.FinancialManagement.FactoryNameId == cementViewModel.FactoryName)
                            .Where(current => current.ProvinceId == cementViewModel.Province)
                            .Where(current => current.CityId == cementViewModel.City)
                            .SingleOrDefault();

                        if (oDestinationManagement != null)
                        {
                            DestinationAmountPaid = (oDestinationManagement.FinancialManagement.AmountPaid * Tonnage)
                                                  + oDestinationManagement.DestinationAmountPaid;
                        }

                        double AmountPaid = oFinancialManagement.AmountPaid * Tonnage;
                        int LastInvoiceNumber = UnitOfWork.FactorCementRepository.GetLastInvoiceNumber() + 1;

                        string userAddress = null;
                        Models.User oUser;

                        if (Infrastructure.Sessions.AuthenticatedUser?.UserName != null)
                        {
                            oUser = UnitOfWork.UserRepository.GetByUserName(Infrastructure.Sessions.AuthenticatedUser.UserName);
                            if (oUser.creditAmount > 0)
                                ViewBag.DisplaycreditAmount = "پس از ورود به درگاه پرداخت مبلغ اعتبار بصورت خودکار از کیف پول کسر خواهد شد";
                            userAddress = oUser.Address;
                        }
                        else
                        {
                            oUser = UnitOfWork.UserRepository.GetByUserName("Guest");
                            userAddress = UnitOfWork.UserRepository.GetAddressByPhoneNumebr(cementViewModel.BuyerMobile);
                        }

                        Models.FactorCement oFactorCement = new Models.FactorCement()
                        {
                            ProductNameId = cementViewModel.ProductName,
                            ProductTypeId = cementViewModel.ProductType,
                            PackageTypeId = cementViewModel.PackageType,
                            FactoryNameId = cementViewModel.FactoryName,
                            Tonnagedouble = cementViewModel.Tonnage,
                            BuyerMobile = cementViewModel.BuyerMobile,
                            Address = userAddress,
                            AmountPaid = AmountPaid,
                            DestinationAmountPaid = DestinationAmountPaid,
                            Description = cementViewModel.Description,
                            RequestState = Convert.ToInt32(RequestState.PendingFinancialApproval),
                            UserIPAddress = Request.UserHostAddress,
                            Browser = Request.Browser.Type,
                            URLAddress = UnitOfWork.SubSystemRepository.Get()?.FirstOrDefault()?.UrlTo,
                            UserId = oUser.Id,
                            BuyerFullName = cementViewModel.BuyerFullName
                        };

                        oFactorCement.InvoiceNumber = LastInvoiceNumber;
                        UnitOfWork.FactorCementRepository.Insertdata(oFactorCement);

                        DestinationAmountPaid = varRequest + AmountPaid;
                        cementViewModel.InvoiceNumber = LastInvoiceNumber;

                        ViewBag.Karkhane = "تحویل درب کارخانه: " + String.Format("{0:n0}", AmountPaid) + " ریال";
                        if (oDestinationManagement != null)
                            ViewBag.Mahal = "قیمت تقریبی محل تحویل: " + String.Format("{0:n0}", DestinationAmountPaid) + " ریال";

                        Session["LastInvoiceNumber"] = LastInvoiceNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.PageMessages = "خطا: " + ex.Message;
            }

            ViewData(cementViewModel);
            return View(cementViewModel);
        }


        public virtual ActionResult ShowFactor(int invoicenumber)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            var oFactorCement = UnitOfWork.FactorCementRepository.GetByinvoicenumber(invoicenumber).FirstOrDefault();
            cementViewModel = ZarinpalController.ConvertCementViewModel(oFactorCement);
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Login()
        {
            ViewModels.Account.LoginViewModel loginViewModel = new ViewModels.Account.LoginViewModel();
            return View(loginViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult About()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Contact()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult GetLivePrice()
        {
            var financialList = UnitOfWork.FinancialManagementRepository
                .Get()
                .Where(f => f.AmountPaid != 0 && f.IsActived && !f.IsDeleted)
                .ToList();

            var productIdList = financialList
                .Select(x => new InventoryViewModel
                {
                    ProductTypeId = x.ProductTypeId,
                    ProductNameId = x.ProductNameId,
                    FactoryNameId = x.FactoryNameId,
                    PackageType = x.PackageTypeId
                })
                .Distinct()
                .ToList();

            var inventoryList = UnitOfWork.InventoryamountRepository
                .GetByProductId(productIdList);

            var liveProducts = new List<LiveProductViewModel>();

            foreach (var f in financialList)
            {
                // پیدا کردن موجودی مربوط به محصول فعلی (مقایسه دقیق‌تر)
                var inventory = inventoryList
                    .FirstOrDefault(inv => inv.ProductNameId == f.ProductNameId
                    && inv.ProductTypeId == f.ProductTypeId
                    && inv.PackageType == f.PackageType
                    && inv.FactoryNameId == f.FactoryNameId);
                if (inventory != null)
                {
                    liveProducts.Add(new LiveProductViewModel
                    {
                        ProductNameId = f.ProductNameId,
                        ProductTypeId = f.ProductTypeId,
                        PackageTypeId = f.PackageTypeId,
                        FactoryNameId = f.FactoryNameId,
                        ProductName = f.ProductName.Name,
                        ProductTypeName = f.ProductType.Name,
                        PackageType = f.PackageType.Name,
                        FactoryName = f.FactoryName.Name,
                        AmountPaid = f.AmountPaid,
                        Inventorytonnage = inventory != null ? inventory.Inventorytonnage : 0
                    });
                }
            }

            // فیلتر و مرتب‌سازی
            liveProducts = liveProducts
                .Where(x => x != null)
                .OrderBy(x => x.PackageType.Contains("کیسه") ? 0 : 1)
                .ThenBy(x => x.ProductTypeName == "تیپ 2" ? 0 : 1)
                 .ThenBy(x => x.FactoryName)
                .ToList();



            return View(liveProducts);
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult ErrorAccount()
        {
            ViewBag.Message = "ErrorAccount";
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Main()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult UserGuid()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Authenticate()
        {
            return View();
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Price()
        {
            var test = UnitOfWork.FinancialManagementRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();
            return View(test);
        }


        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult News()
        {
            DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var get = UnitOfWork.NewsReopsitory.Get().Where(s => s.IsActived && !s.IsDeleted && s.StartDate <= date1 && s.EndDate >= date1);
            ViewBag.count = get.Count();
            return PartialView(get);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Rechargewallet(string userName)
        {
            Rechargewallet test = new Rechargewallet();
            test.UserName = userName;
            return View(test);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Rechargewallet(Rechargewallet rechargewallet)
        {
            Models.User oUser;
            int LastInvoiceNumber = UnitOfWork.walletFactorRepository.GetLastInvoiceNumber() + 1;
            oUser = UnitOfWork.UserRepository.GetByUserName(rechargewallet.UserName);
            Models.walletFactor OwalletFactor = new Models.walletFactor()
            {
                Id = Guid.NewGuid(),
                UserId = oUser.Id,
                Chargeamount = rechargewallet.Chargeamount,
                BuyerMobile = oUser.BuyerMobile
            };

            OwalletFactor.InvoiceNumber = LastInvoiceNumber;
            UnitOfWork.walletFactorRepository.Insertdata(OwalletFactor);
            return RedirectToAction("Paymentwallet", "Zarinpal", new { Chargeamount = rechargewallet.Chargeamount, invoiceNumber = LastInvoiceNumber });

        }

        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        //public virtual ActionResult Continue_Authenticate()
        //{
        //    return (RedirectToAction(MVC.HomeMain.ActionNames("index")));
        //}
    }
}