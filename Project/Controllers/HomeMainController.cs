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
            var varHeadLines = UnitOfWork.ProductNameRepository.Get().ToList();
            ViewData["ProductName"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null).OrderBy(x => x.Value);

            var varProductTypes = new List<Models.ProductType>();
            ViewData["ProductType"] = new System.Web.Mvc.SelectList(varProductTypes, "Id", "Name", null).OrderBy(x => x.Text);
            ViewData["PackageType"] = new System.Web.Mvc.SelectList(varProductTypes, "Id", "Name", null).OrderBy(x => x.Text);
            ViewData["FactoryName"] = new System.Web.Mvc.SelectList(varProductTypes, "Id", "Name", null).OrderBy(x => x.Text);
            ViewData["Tonnage"] = new System.Web.Mvc.SelectList(varProductTypes, "Id", "Name", null).OrderBy(x => x.Text);
            ViewData["Village"] = new System.Web.Mvc.SelectList(varProductTypes, "Id", "Name", null).OrderBy(x => x.Text);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null).OrderBy(x => x.Text);

            var varCities = new List<Models.City>();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null).OrderBy(x => x.Text);

            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
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