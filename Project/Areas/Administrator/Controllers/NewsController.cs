using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Cement;
using Utilities.PersianDate;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class NewsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
            Viewdata(cementViewModel);
            return View(cementViewModel);
        }


        private void Viewdata(CementViewModel cementViewModel)
        {
            var News = UnitOfWork.NewsReopsitory.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["News"] = new System.Web.Mvc.SelectList(News, "Id", "Title", cementViewModel.News).OrderByDescending(x => x.Text);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Cement.CementViewModel viewModel)
        {
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();
            var varRequest =
                UnitOfWork.NewsReopsitory.Get()
                .Where(x => x.IsActived && !x.IsDeleted);

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Cement.CementViewModel()
                        {
                            Id = current.Id,
                            TitleNews = current.Title,
                            TextNews = current.newsText,
                            StringstartDateNews = current.StartDate.ConvertToShamsi(),
                            StringEndDateNews = current.EndDate.ConvertToShamsi(),
                            StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                        })
                        .AsQueryable();

                var ssss = ViewModelsvarBanks.ToList();
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            var ofindsubheadline =
                 UnitOfWork.NewsReopsitory.Get()
                 .Where(x => x.IsActived && !x.IsDeleted)
                 .Where(model => model.Id == cementViewModel.News)
                 //.Where(model => model.Code == cementViewModel.code)
                 .FirstOrDefault();

            if (ofindsubheadline != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";

            else if (cementViewModel.TitleNews == null || cementViewModel.TextNews == null)
                ViewBag.PageMessages = "فیلد های عنوان خبر و کد نباید خالی باشد ";
            else if(cementViewModel.startDateNews == null || cementViewModel.EndDateNews == null)
                ViewBag.PageMessages = "باید تاریخ شروع و پایان خبر مشخص شود";
            else
            {
                Models.newsweb newsweb = new Models.newsweb();
                newsweb.Title = cementViewModel.TitleNews;
                newsweb.newsText = cementViewModel.TextNews;
                //var SD = DateTime.Parse(cementViewModel.startDateNews.ConvertToShamsi());
                //newsweb.StartDate = SD;
                //var ED = DateTime.Parse(cementViewModel.EndDateNews.ConvertToShamsi());
                //newsweb.EndDate = ED;
                                var SD = DateTime.Parse(cementViewModel.StringstartDateNews);
                newsweb.StartDate = SD;
                var ED = DateTime.Parse(cementViewModel.StringEndDateNews);
                newsweb.EndDate = ED;

                UnitOfWork.NewsReopsitory.Insertdata(newsweb);
                UnitOfWork.Save();

                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }
            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel
                 =UnitOfWork.NewsReopsitory.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    Id = current.Id,
                    TitleNews = current.Title,
                    TextNews = current.newsText,
                    StringstartDateNews = current.StartDate.ConvertToShamsi(),
                    StringEndDateNews  = current.EndDate.ConvertToShamsi(),
                    StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                })
                .FirstOrDefault()
                ;

            Viewdata(cementViewModel);
            ViewBag.PageMessages = null;

            return (View(cementViewModel));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OlderAccount =
                    UnitOfWork.NewsReopsitory
                    .Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault()
                    ;

                var News = UnitOfWork.NewsReopsitory.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
                base.ViewData["News"] = new System.Web.Mvc.SelectList(News, "Id", "Title", cementViewModel.News).OrderByDescending(x => x.Text);
                // **************************************************
                OlderAccount.IsDeleted = true;
                OlderAccount.IsActived = false;
                OlderAccount.UpdateDateTime = DateTime.Now;
                UnitOfWork.NewsReopsitory.Update(OlderAccount);
                // **************************************************
                Models.newsweb newsweb = new Models.newsweb();
                newsweb.Title = cementViewModel.TitleNews;
                newsweb.newsText = cementViewModel.TextNews;
                var SD = DateTime.Parse(cementViewModel.StringstartDateNews);
                newsweb.StartDate = SD;
                var ED = DateTime.Parse(cementViewModel.StringEndDateNews);
                newsweb.EndDate = ED;
                UnitOfWork.NewsReopsitory.Insertdata(newsweb);
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

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            var oAccountNumberManage
                = UnitOfWork.NewsReopsitory.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                {
                    Id = current.Id,
                    TitleNews = current.Title,
                    TextNews = current.newsText,
                    StringstartDateNews = current.StartDate.ConvertToShamsi(),
                    StringEndDateNews = current.EndDate.ConvertToShamsi(),
                    StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                })
                .FirstOrDefault()
                ;

            if (oAccountNumberManage == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oAccountNumberManage));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.NewsReopsitory.Get()
                    .Where(current => current.Id == cementViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.NewsReopsitory.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.News.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }
    }
}
