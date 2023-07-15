using DAL;
using OPS.ir.shaparak.sadad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class ProvinceController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DestinationIndex()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }

        private void Viewdata(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult DestinationGetRequests() => (JsonResult)DestinationSearch(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.ProvinceRepository.Get();
            #region Condition
            if (viewModel?.Province != null && viewModel.Province != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.Id == viewModel.Province);
                Search = true;
            }
            #endregion
            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.Name)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            IsActive = current.IsActived,
                            StringProvince = current.Name
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
        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult DestinationSearch(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varRequest =
                UnitOfWork.ProvinceRepository.Get();
            if (viewModel?.Province != null && viewModel.Province != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.Name == viewModel.StringProvince);
                Search = true;
            }

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            StringProvince = current.Name
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Edit(Guid id)
        {
            try
            {
                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);
                ViewBag.PageMessages = null;

                var oRequest =
                 UnitOfWork.ProvinceRepository.Get()
                 .Where(current => current.Id == id)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                 {
                     Id = current.Id,
                     IsActive = current.IsActived,
                     StringProvince = current.Name
                 })
                 .FirstOrDefault();

                if (oRequest == null)
                {
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
                }

                return View(oRequest);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }
        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.ProvinceRepository
                    .GetById(cementViewModel.Id)
                    ;

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                // **************************************************
                OlderAccount.IsActived = cementViewModel.IsActive;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.ProvinceRepository.Update(OlderAccount);
                UnitOfWork.Save();
                // **************************************************
                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ویرایش گردید  ";

                return View(cementViewModel);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));

            }
        }
    }
}