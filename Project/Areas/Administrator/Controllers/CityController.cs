using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class CityController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/City
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name");
            var varCities = UnitOfWork.CityRepository.Get().ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name");
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
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", cementViewModel.Province);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", cementViewModel.City);
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
                UnitOfWork.CityRepository.Get();

            #region Condition
            if (viewModel?.Province != null && viewModel.Province != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);
                Search = true;
            }
            if (viewModel?.City != null && viewModel.City != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.Id == viewModel.City);
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
                            StringCity = current.Name
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
                UnitOfWork.CityRepository.Get();
            if (viewModel?.Province != null && viewModel.Province != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);
                Search = true;
            }

            if (viewModel?.City != null && viewModel.City != Guid.Empty)
            {
                varRequest = varRequest.Where(x => x.Id == viewModel.City);
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
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Edit(Guid id)
        {
            try
            {
                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);
                ViewBag.PageMessages = null;

                var oRequest =
                 UnitOfWork.CityRepository.Get()
                 .Where(current => current.Id == id)
                 .ToList()
                 .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                 {
                     Id = current.Id,
                     IsActive = current.IsActived,
                     StringCity = current.Name
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
                    UnitOfWork.CityRepository
                    .GetById(cementViewModel.Id)
                    ;

                var City = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province).ToList(); /// کرمان
                base.ViewData["City"] = new System.Web.Mvc.SelectList(City, "Id", "Name", cementViewModel.City).OrderBy(x => x.Text);

                // **************************************************
                OlderAccount.IsActived = cementViewModel.IsActive;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.CityRepository.Update(OlderAccount);
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