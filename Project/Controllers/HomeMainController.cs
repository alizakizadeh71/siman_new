using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Utilities.PersianDate;
using ViewModels.Areas.Administrator.Cement;
using System.Web.Routing;
using OPS.Controllers;
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

            Models.User oUser;
            if (Infrastructure.Sessions.AuthenticatedUser?.Id != null && Infrastructure.Sessions.AuthenticatedUser.RoleCode != 1000)
            {
                oUser = UnitOfWork.UserRepository.GetById(Infrastructure.Sessions.AuthenticatedUser.Id);
                ViewBag.DisplaycreditAmount = oUser.FullName + " خوش آمدید. موجودی کیف پول شما برابر است با: " + oUser.creditAmount.ToString("N0") + " ریال. ";
            }

            ViewBag.PageMessages = null;
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel
            {
                ProductName = new Guid("c26a5f77-5a78-4dd6-9f0a-0b647c9f7195"),
                ProductType = new Guid("bd25dd8b-7845-4021-b91e-fcfdb7a21649"),
                PackageType = new Guid("85fe59ba-1eab-47cb-8635-2ce297f3cbb6"),
                FactoryName = new Guid("4ad543c7-8af2-4832-917b-88d17758a9e1"),
                Tonnage = new Guid("9133daec-834a-4303-9687-5b08b479ffdf"),
                Province = new Guid("d803f690-6de8-11e5-8295-c0f8daba7555"),
                City = new Guid("f8b85020-ad88-4d89-9aa2-0de9e27fd9b1"),
                Village = new Guid("F4125115-F66B-11EE-87BF-D039573E90CC")
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

        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        //public virtual ActionResult Continue_Authenticate()
        //{
        //    return (RedirectToAction(MVC.HomeMain.ActionNames("index")));
        //}
    }
}