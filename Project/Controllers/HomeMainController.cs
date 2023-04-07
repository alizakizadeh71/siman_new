using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using PAPUtilities;


namespace OPS.Controllers
{
    public partial class HomeMainController : Infrastructure.BaseControllerWithUnitOfWork
    {




        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Index()
        {
            ViewBag.PageMessages = null;
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel
            {
                ProductName = new Guid("c26a5f77-5a78-4dd6-9f0a-0b647c9f7195"),
                ProductType = new Guid("5f6f25ba-4ee7-4b56-8bdf-fefc3e390b0d"),
                PackageType = new Guid("167fd28a-fdec-4385-866f-e8c08dbfd8a6"),
                FactoryName = new Guid("4ad543c7-8af2-4832-917b-88d17758a9e1"),
                Tonnage = new Guid("bf031e9a-6d4e-400f-a98e-e86ed3b954c9"),
                Province = new Guid("c9cea679-6de8-11e5-8295-c0f8daba7555"),
                City = new Guid("92f077e9-bd95-4756-bc78-ec8537c75d07"),
            };
            ViewData(cementViewModel);
            return View(cementViewModel);
        }

        private void ViewData(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList(); /// تیپ یک
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text); /// کیسه

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text); /// ممتازان کرمان

            var Tonnage = UnitOfWork.tonnageRepository.GetByPackageTypeId(cementViewModel.PackageType).ToList(); /// کیسه
            base.ViewData["Tonnage"] = new System.Web.Mvc.SelectList(Tonnage, "Id", "Name", cementViewModel.Tonnage).OrderBy(x => x.Text); /// 12 تن

            var Province = UnitOfWork.ProvinceRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(Province, "Id", "Name", cementViewModel.Province).OrderBy(x => x.Text);

            var City = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList(); /// کرمان
            base.ViewData["City"] = new System.Web.Mvc.SelectList(City, "Id", "Name", cementViewModel.City).OrderBy(x => x.Text); /// کوهبنان
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
                    long AmountPaid = 1100; /// محاسبه مبلغ
                    Models.User oUser = UnitOfWork.UserRepository.GetByUserName("Guest");
                    //Models.FactorCement oFactorCement = new Models.FactorCement()
                    //{
                    //    ProductNameId = cementViewModel.ProductName,
                    //    ProductTypeId = cementViewModel.ProductType,
                    //    PackageTypeId = cementViewModel.PackageType,
                    //    FactoryNameId = cementViewModel.FactoryName,
                    //    TonnageId = cementViewModel.Tonnage,
                    //    ProvinceId = cementViewModel.Province,
                    //    CityId = cementViewModel.City,
                    //    BuyerMobile = cementViewModel.BuyerMobile,
                    //    Address = cementViewModel.Address,
                    //    AmountPaid = AmountPaid,
                    //    Description = cementViewModel.Description,
                    //    RequestState = Convert.ToInt32(Enums.RequestStates.PaymentOrder),
                    //    UserIPAddress = Request.UserHostAddress,
                    //    Browser = Request.Browser.Type, // مدل و ورژن مرورگر
                    //    URLAddress = UnitOfWork.SubSystemRepository.Get()?.FirstOrDefault()?.UrlTo,
                    //    UserId = oUser.Id,
                    //};
                    //UnitOfWork.FactorCementRepository.Insert(oFactorCement); // شناسه واریز
                    ViewBag.PageMessages = " مبلغ پرداختی " + AmountPaid + " تومان ";
                }
            }
            catch (Exception ex)
            {
                ViewBag.PageMessages = " خطا " + ex.Message;
            }
            ViewData(cementViewModel);
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
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
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

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Continue_Authenticate()
        {
            return (RedirectToAction(MVC.HomeMain.Main()));
        }
    }
}