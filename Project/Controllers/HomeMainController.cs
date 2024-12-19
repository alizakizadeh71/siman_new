using DAL;
using Models;
using System;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Account;
//using PAPUtilities;


namespace OPS.Controllers
{
    public partial class HomeMainController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Index()
        {
            if (TempData["WelcomeMessage"] != null && TempData["Balance"] != null)
            {
                ViewBag.WelcomeMessage = TempData["WelcomeMessage"]; // Pass the welcome message to the ViewBag
                ViewBag.Balance = TempData["Balance"];
            }

            ViewBag.Message = "برای محاسبه قیمت و خرید اطلاعات را تکمیل نمایید";
            var oUser = new User();
            if (Infrastructure.Sessions.AuthenticatedUser?.Id != null && Infrastructure.Sessions.AuthenticatedUser.RoleCode != 1000)
            {
                oUser = UnitOfWork.UserRepository.GetById(Infrastructure.Sessions.AuthenticatedUser.Id);
                ViewBag.DisplaycreditAmount = oUser.FullName + " خوش آمدید. موجودی کیف پول شما برابر است با: " + oUser.creditAmount.ToString("N0") + " ریال. ";
            }

            ViewBag.PageMessages = null;

            var product = UnitOfWork.ProductNameRepository.Get()
                .Where(x => x.Code == "1")
                .Select(x => x.Id)
                .FirstOrDefault();

            var productTypes = UnitOfWork.ProductTypeRepository.Get()
                .Where(x => x.ProductNameId == product)
                .Select(x => x.Id)
                .ToList();

            Guid? productType1 = null;
            Guid? packageType = null;
            Guid? Tonnage = null;

            foreach (var item in productTypes)
            {
                packageType = UnitOfWork.PackageTypeRepository.Get()
                    .Where(x => x.ProductTypeId == item)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (packageType != Guid.Empty)
                {
                    productType1 = item;
                    Tonnage = UnitOfWork.tonnageRepository.Get().FirstOrDefault(x => x.PackageTypeId == packageType).Id;
                    break;
                }
            }

            var factoryName = UnitOfWork.FactoryNameRepository.Get()
                .Where(x => x.ProductNameId == product)
                .Select(x => x.Id)
                .FirstOrDefault();

            Guid? province = null;
            Guid? city = null;
            string address = string.Empty;
            string buyerMobile = string.Empty;
            if (Infrastructure.Sessions.AuthenticatedUser?.Id != null && Infrastructure.Sessions.AuthenticatedUser.RoleCode != 1000)
            {
                var user = UnitOfWork.UserRepository.GetById(Infrastructure.Sessions.AuthenticatedUser.Id);
                province = user.ProvinceId;
                city = user.CityId;
                address = user.Address;
                buyerMobile = user.BuyerMobile;
            }


            var cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel
            {
                ProductName = product,
                ProductType = productType1.Value,
                PackageType = packageType.Value,
                FactoryName = factoryName,
                Tonnage = Tonnage.Value,
                Province = province ?? new Guid("d803f690-6de8-11e5-8295-c0f8daba7555"), // مقدار پیش‌فرض
                City = city ?? new Guid("f8b85020-ad88-4d89-9aa2-0de9e27fd9b1"), // مقدار پیش‌فرض
                Village = new Guid("F4125115-F66B-11EE-87BF-D039573E90CC"),
                Address = address,
                BuyerMobile = buyerMobile
            };

            ViewData(cementViewModel);
            return View(cementViewModel);
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

            var Tonnage = UnitOfWork.tonnageRepository.GetByPackageTypeId(cementViewModel.PackageType).ToList(); /// کیسه
            base.ViewData["Tonnage"] = new System.Web.Mvc.SelectList(Tonnage, "Id", "Name", cementViewModel.Tonnage).OrderBy(x => x.Text); /// 12 تن

            var Province = UnitOfWork.ProvinceRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(Province, "Id", "Name", cementViewModel.Province).OrderBy(x => x.Text);

            var City = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList(); /// کرمان
            base.ViewData["City"] = new System.Web.Mvc.SelectList(City, "Id", "Name", cementViewModel.City).OrderBy(x => x.Text); /// کوهبنان

            var varVilages = UnitOfWork.VillageRepository.GetBycityId(cementViewModel.City).ToList();
            base.ViewData["Village"] = new System.Web.Mvc.SelectList(varVilages, "Id", "Name", null);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Index(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            try
            {
                ViewBag.PageMessages = null;

                if (ModelState.IsValid)
                {

                    var oFinancialManagement =
                         UnitOfWork.FinancialManagementRepository
                         .Get()
                         .Where(x => x.IsActived && !x.IsDeleted)
                         .Where(current => current.ProductNameId == cementViewModel.ProductName)
                         .Where(current => current.ProductTypeId == cementViewModel.ProductType)
                         .Where(current => current.PackageTypeId == cementViewModel.PackageType)
                         .Where(current => current.FactoryNameId == cementViewModel.FactoryName)
                         .SingleOrDefault()
                         ;

                    var Tonnage = Convert.ToInt32(UnitOfWork.tonnageRepository.Get()
                    .Where(x => x.Id == cementViewModel.Tonnage).FirstOrDefault().Code);
                    var InventoryTonnage = Convert.ToInt32(UnitOfWork.InventoryamountRepository.Get()
                        .Where(x => x.ProductNameId == cementViewModel.ProductName)
                        .Where(x => x.ProductTypeId == cementViewModel.ProductType)
                        .Where(x => x.PackageTypeId == cementViewModel.PackageType)
                        .Where(x => x.FactoryNameId == cementViewModel.FactoryName)
                        .Select(x => x.Inventorytonnage)
                        .FirstOrDefault());

                    if (oFinancialManagement == null)
                    {
                        ViewBag.PageMessages = " قیمت توسط ادمین در سیستم ثبت نشده است ";
                    }
                    else if (Tonnage > InventoryTonnage)
                    {
                        ViewBag.PageMessages = "ثناژ درخواستی شما در انبار موجود نمی باشد";
                    }
                    else
                    {
                        var varRequest =
                            UnitOfWork.DestinationManagementRepository.Get()
                            .Where(x => x.IsActived && !x.IsDeleted)
                            .Where(current => current.FinancialManagementId == oFinancialManagement.Id)
                            .Where(current => current.ProvinceId == cementViewModel.Province)
                            .Where(cuurrent => cuurrent.CityId == cementViewModel.City)
                            .Select(x => x.DestinationAmountPaid).SingleOrDefault();

                        long? DestinationAmountPaid = varRequest;
                        var oDestinationManagement =
                                 UnitOfWork.DestinationManagementRepository
                                 .Get()
                                 .Where(x => x.IsActived && !x.IsDeleted)
                                 .Where(x => x.FinancialManagement.IsActived && !x.FinancialManagement.IsDeleted)
                                 .Where(current => current.FinancialManagement.ProductNameId == cementViewModel.ProductName)
                                 .Where(current => current.FinancialManagement.ProductTypeId == cementViewModel.ProductType)
                                 .Where(current => current.FinancialManagement.PackageTypeId == cementViewModel.PackageType)
                                 .Where(current => current.FinancialManagement.FactoryNameId == cementViewModel.FactoryName)
                                 .Where(current => current.ProvinceId == cementViewModel.Province)
                                 .Where(current => current.CityId == cementViewModel.City)
                                 .SingleOrDefault()
                                 ;
                        if (oDestinationManagement != null)
                        {
                            DestinationAmountPaid = (oDestinationManagement.FinancialManagement.AmountPaid * Tonnage) + oDestinationManagement.DestinationAmountPaid;
                        }

                        //{
                        //    DestinationAmountPaid = (oDestinationManagement.FinancialManagement.AmountPaid * Tonnage) + oDestinationManagement.DestinationAmountPaid;
                        //}
                        long AmountPaid = oFinancialManagement.AmountPaid * Tonnage; /// محاسبه مبلغ
                        int LastInvoiceNumber = UnitOfWork.FactorCementRepository.GetLastInvoiceNumber() + 1;
                        Models.User oUser;
                        if (Infrastructure.Sessions.AuthenticatedUser?.UserName != null)
                        {
                            oUser = UnitOfWork.UserRepository.GetByUserName(Infrastructure.Sessions.AuthenticatedUser.UserName);
                            if (oUser.creditAmount > 0)
                            {
                                ViewBag.DisplaycreditAmount = " پس از ورود به درگاه پرداخت مبلغ اعتبار بصورت خودکار از کیف پول کسر خواهد شد ";
                            }
                        }
                        else
                        {
                            oUser = UnitOfWork.UserRepository.GetByUserName("Guest");
                        }
                        Models.FactorCement oFactorCement = new Models.FactorCement()
                        {
                            ProductNameId = cementViewModel.ProductName,
                            ProductTypeId = cementViewModel.ProductType,
                            PackageTypeId = cementViewModel.PackageType,
                            FactoryNameId = cementViewModel.FactoryName,
                            TonnageId = cementViewModel.Tonnage,
                            ProvinceId = cementViewModel.Province,
                            CityId = cementViewModel.City,
                            BuyerMobile = cementViewModel.BuyerMobile,
                            Address = cementViewModel.Address,
                            AmountPaid = AmountPaid,
                            DestinationAmountPaid = DestinationAmountPaid,
                            Description = cementViewModel.Description,
                            RequestState = Convert.ToInt32(Enums.RequestStates.PaymentOrder),
                            UserIPAddress = Request.UserHostAddress,
                            Browser = Request.Browser.Type, // مدل و ورژن مرورگر
                            URLAddress = UnitOfWork.SubSystemRepository.Get()?.FirstOrDefault()?.UrlTo,
                            UserId = oUser.Id,
                        };

                        oFactorCement.InvoiceNumber = LastInvoiceNumber;
                        UnitOfWork.FactorCementRepository.Insertdata(oFactorCement);
                        DestinationAmountPaid = varRequest + AmountPaid;
                        cementViewModel.InvoiceNumber = LastInvoiceNumber;
                        ViewBag.Karkhane = "تحویل درب کارخانه: " + String.Format("{0:n0}", AmountPaid) + " ریال ";
                        if (oDestinationManagement != null)
                        {
                            ViewBag.Mahal = "قیمت تقریبی محل تحویل: " + String.Format("{0:n0}", DestinationAmountPaid) + " ریال ";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.PageMessages = " خطا " + ex.Message;
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
                UserIPAddress = Request.UserHostAddress,
                Browser = Request.Browser.Type, // مدل و ورژن مرورگر
                URLAddress = UnitOfWork.SubSystemRepository.Get()?.FirstOrDefault()?.UrlTo,
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