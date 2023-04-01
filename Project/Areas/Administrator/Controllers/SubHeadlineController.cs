using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class SubHeadLineController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            var varOffices = UnitOfWork.SubHeadLineRepository.Get().ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varOffices, "Id", "Name", null);
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetSubHeadLines()
        {
            var varSubHeadLines =
                UnitOfWork.SubHeadLineRepository.Get()
                ;

            var ViewModelsvarSubHeadLines
                = varSubHeadLines.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.SubHeadLine.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        HeadLine = current.HeadLine.Name,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.SubHeadLine.IndexViewModel>(ViewModelsvarSubHeadLines);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;

            var varOffices = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varOffices, "Id", "Name", null);

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.SubHeadLine.CreateViewModel SubHeadLine)
        {
            ViewBag.PageMessages = null;

            Models.SubHeadLine oSubHeadLine = new Models.SubHeadLine();

            var oFindSubHeadLine =
                 UnitOfWork.SubHeadLineRepository
                 .Get()
                 .Where(current => current.HeadLineId == SubHeadLine.HeadLine)
                 .Where(current => current.Code == SubHeadLine.Code)
                 .FirstOrDefault()
                 ;

            var varOffices = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varOffices, "Id", "Name", null);


            if (oFindSubHeadLine != null)
            {
                ViewBag.PageMessages += "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oSubHeadLine.HeadLineId = SubHeadLine.HeadLine;
                oSubHeadLine.Name = SubHeadLine.Name;
                oSubHeadLine.IsActived = true;
                oSubHeadLine.IsDeleted = false;
                oSubHeadLine.IsSystem = false;
                oSubHeadLine.IsVerified = true;
                oSubHeadLine.Code = SubHeadLine.Code;
                oSubHeadLine.InsertDateTime = DateTime.Now;
                oSubHeadLine.UpdateDateTime = DateTime.Now;

                UnitOfWork.SubHeadLineRepository.Insert(oSubHeadLine);
                UnitOfWork.Save();

                ViewBag.PageMessages += "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(SubHeadLine);
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Detail(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.SubHeadLine.DetailViewModel oSubHeadLine
                = UnitOfWork.SubHeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubHeadLine.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    HeadLine = current.HeadLine.Name,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oSubHeadLine == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubHeadLine));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null);

            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.SubHeadLine.EditViewModel oSubHeadLine
                = UnitOfWork.SubHeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubHeadLine.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    HeadLine = current.HeadLineId,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oSubHeadLine == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubHeadLine));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.SubHeadLine.EditViewModel SubHeadLine)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.SubHeadLineRepository
                    .Get()
                    .Where(current => current.Id == SubHeadLine.Id)
                    .FirstOrDefault()
                    ;

                Models.SubHeadLine oFindedOther;
                Models.SubHeadLine oFindedSubHeadLine;

                oFindedOther =
                    UnitOfWork.SubHeadLineRepository
                    .Get()
                    .Where(current => current.Code == SubHeadLine.Code)
                    .Where(current => current.HeadLineId == SubHeadLine.HeadLine)
                    .Where(current => current.Id != SubHeadLine.Id)
                    .FirstOrDefault()
                    ;

                oFindedSubHeadLine =
                    UnitOfWork.SubHeadLineRepository
                    .Get()
                    .Where(current => current.Id == SubHeadLine.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "خدماتی با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";

                    var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
                    ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null);

                    return View(SubHeadLine);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedSubHeadLine.UpdateDateTime = DateTime.Now;
                    oFindedSubHeadLine.Name = SubHeadLine.Name;
                    oFindedSubHeadLine.Code = SubHeadLine.Code;
                    oFindedSubHeadLine.HeadLineId = SubHeadLine.HeadLine;

                    UnitOfWork.SubHeadLineRepository.Update(oFindedSubHeadLine);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "خدمات درخواستی شما با موفقیت ثبت گردید  ";
                }

                return Index();
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

            ViewModels.Areas.Administrator.SubHeadLine.DetailViewModel oSubHeadLine
                = UnitOfWork.SubHeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubHeadLine.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    HeadLine = current.HeadLine.Name,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oSubHeadLine == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubHeadLine));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varSubHeadLines =
                    UnitOfWork.SubHeadLineRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varSubHeadLines != null)
                {
                    UnitOfWork.SubHeadLineRepository.Delete(varSubHeadLines);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.SubHeadLine.Index()));
                }

                else
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }
    }
}
