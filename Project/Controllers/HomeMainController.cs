using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.PersianDate;
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
                ProductType = new Guid("bd25dd8b-7845-4021-b91e-fcfdb7a21649"),
                PackageType = new Guid("85fe59ba-1eab-47cb-8635-2ce297f3cbb6"),
                FactoryName = new Guid("4ad543c7-8af2-4832-917b-88d17758a9e1"),
                Tonnage = new Guid("9133daec-834a-4303-9687-5b08b479ffdf"),
                Province = new Guid("d803f690-6de8-11e5-8295-c0f8daba7555"),
                City = new Guid("f8b85020-ad88-4d89-9aa2-0de9e27fd9b1"),
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
                    var varRequest =
                    UnitOfWork.DestinationManagementRepository.Get()
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Where(current => current.FinancialManagementId == oFinancialManagement.Id)
                    .Where(current => current.ProvinceId == cementViewModel.Province)
                    .Where(cuurrent => cuurrent.CityId == cementViewModel.City)
                    .Select(x => x.DestinationAmountPaid).SingleOrDefault();

                    if (oFinancialManagement == null)
                    {
                        ViewBag.PageMessages = " قیمت توسط ادمین در سیستم ثبت نشده است ";
                    }
                    else
                    {
                        var Tonnage = Convert.ToInt32(UnitOfWork.tonnageRepository.Get()
                            .Where(x => x.Id == cementViewModel.Tonnage).FirstOrDefault().Code);

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
                        Models.User oUser = UnitOfWork.UserRepository.GetByUserName("Guest");
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
                            ViewBag.Mahal = "تحویل در محل: " + String.Format("{0:n0}", DestinationAmountPaid) + " ریال ";
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.None)]
        public virtual ActionResult Continue_Authenticate()
        {
            return (RedirectToAction(MVC.HomeMain.Main()));
        }
    }
}