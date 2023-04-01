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
    public partial class SubSystemController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.JsonResult GetSubSystems()
        {
            var varSubSystems =
                UnitOfWork.SubSystemRepository.Get()
                ;

            var ViewModelsvarSubSystems
                = varSubSystems.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.SubSystem.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code,
                        UrlFrom = current.UrlFrom,
                        UrlTo = current.UrlTo,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                        UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.SubSystem.IndexViewModel>(ViewModelsvarSubSystems);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.SubSystem.CreateViewModel subsystem)
        {
            ViewBag.PageMessages = null;
            
            Models.SubSystem oFindedSubSystem = new Models.SubSystem();

            oFindedSubSystem =
                UnitOfWork.SubSystemRepository
                .Get()
                .Where(current => current.Name == subsystem.Name || current.Code == subsystem.Code)
                .FirstOrDefault()
                ;

            if (oFindedSubSystem != null)
            {
                ViewBag.PageMessages += "زیرسیستم با نام یا کد مشابه در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            // **************************************************
            // **************************************************
            if (ModelState.IsValid)
            {
                Models.SubSystem oSubSystem = new Models.SubSystem();
                {
                    oSubSystem.Code = subsystem.Code;
                    oSubSystem.IsActived = true;
                    oSubSystem.IsDeleted = false;
                    oSubSystem.IsSystem = false;
                    oSubSystem.IsVerified = true;
                    oSubSystem.Name = subsystem.Name;
                    oSubSystem.UrlFrom = subsystem.UrlFrom;
                    oSubSystem.UrlTo = subsystem.UrlTo;
                    oSubSystem.InsertDateTime = DateTime.Now;
                    oSubSystem.UpdateDateTime = DateTime.Now;

                    UnitOfWork.SubSystemRepository.Insert(oSubSystem);
                    UnitOfWork.Save();

                    #region Insert AccountNumberManage

                    var AcountNumberList = UnitOfWork.AccountNumberRepository.Get().ToList();
                    Models.AccountNumberManage newRow;
                    foreach (var row in AcountNumberList)
                    {
                        newRow = new AccountNumberManage();
                        newRow.AccountNumberId = row.Id;
                        newRow.InsertDateTime = DateTime.Now;
                        newRow.IsActived = true;
                        newRow.IsDeleted = false;
                        newRow.IsSystem = false;
                        newRow.IsVerified = true;
                        newRow.ProvinceId = row.ProvinceId;
                        newRow.SubSystemId = oSubSystem.Id;
                        newRow.UpdateDateTime = DateTime.Now;
                        UnitOfWork.AccountNumberManageRepository.Insert(newRow);
                        UnitOfWork.Save();
                    }

                    #endregion

                    ViewBag.PageMessages += "زیرسیستم درخواستی شما با موفقیت ثبت گردید  ";
                }
            }

            return View(subsystem);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Detail(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id==null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.SubSystem.DetailViewModel oSubSystem
                = UnitOfWork.SubSystemRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubSystem.DetailViewModel()
                {
                    Id=current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    UrlFrom = current.UrlFrom,
                    UrlTo = current.UrlTo,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oSubSystem == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubSystem));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id==null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.SubSystem.EditViewModel oSubSystem
                = UnitOfWork.SubSystemRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubSystem.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    UrlFrom = current.UrlFrom,
                    UrlTo = current.UrlTo
                })
                .FirstOrDefault()
                ;

            if (oSubSystem == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubSystem));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.SubSystem.EditViewModel subsystem)
        {
            ViewBag.PageMessages = null;

            try
            {
                Models.SubSystem oFindedOther;
                Models.SubSystem oFindedSubSystem;

                oFindedOther =
                    UnitOfWork.SubSystemRepository
                    .Get()
                    .Where(current => current.Name == subsystem.Name)
                    .Where(current => current.Id != subsystem.Id)
                    .FirstOrDefault()
                    ;

                oFindedSubSystem =
                    UnitOfWork.SubSystemRepository
                    .Get()
                    .Where(current => current.Id == subsystem.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "زیرسیستم با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View();
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedSubSystem.Name = subsystem.Name;
                    oFindedSubSystem.Code = subsystem.Code;
                    oFindedSubSystem.UrlFrom = subsystem.UrlFrom;
                    oFindedSubSystem.UrlTo = subsystem.UrlTo;
                    oFindedSubSystem.UpdateDateTime = DateTime.Now;

                    UnitOfWork.SubSystemRepository.Update(oFindedSubSystem);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "زیرسیستم درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(subsystem);
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

            if (id==null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.SubSystem.DetailViewModel oSubSystem
                = UnitOfWork.SubSystemRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.SubSystem.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    UrlFrom = current.UrlFrom,
                    UrlTo = current.UrlTo,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oSubSystem == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oSubSystem));
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varSubSystems =
                    UnitOfWork.SubSystemRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varSubSystems != null)
                {
                    UnitOfWork.SubSystemRepository.Delete(varSubSystems);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.SubSystem.Index()));
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
