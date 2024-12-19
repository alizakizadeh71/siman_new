using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class HeadLineController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetHeadLines()
        {
            var varOffices =
                UnitOfWork.HeadLineRepository.Get()
                ;

            var ViewModelsvarOffices
                = varOffices.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.HeadLine.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.HeadLine.IndexViewModel>(ViewModelsvarOffices);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

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
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.HeadLine.CreateViewModel Office)
        {
            ViewBag.PageMessages = null;

            Models.HeadLine oHeadLine = new Models.HeadLine();

            var oFindOffice =
                 UnitOfWork.HeadLineRepository
                 .Get()
                 .Where(current => current.Code == Office.Code)
                 .FirstOrDefault()
                 ;

            if (oFindOffice != null)
            {
                ViewBag.PageMessages += "دفتر مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oHeadLine.Name = Office.Name;
                oHeadLine.IsActived = true;
                oHeadLine.IsDeleted = false;
                oHeadLine.IsSystem = false;
                oHeadLine.IsVerified = true;
                oHeadLine.Code = Office.Code;
                oHeadLine.InsertDateTime = DateTime.Now;
                oHeadLine.UpdateDateTime = DateTime.Now;

                UnitOfWork.HeadLineRepository.Insert(oHeadLine);
                UnitOfWork.Save();

                ViewBag.PageMessages += "دفتر درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(Office);
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

            ViewModels.Areas.Administrator.HeadLine.DetailViewModel oOffice
                = UnitOfWork.HeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadLine.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oOffice == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oOffice));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.HeadLine.EditViewModel oOffice
                = UnitOfWork.HeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadLine.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oOffice == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oOffice));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.HeadLine.EditViewModel Office)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Id == Office.Id)
                    .FirstOrDefault()
                    ;

                Models.HeadLine oFindedOther;
                Models.HeadLine oFindedOffice;

                oFindedOther =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Code == Office.Code)
                    .Where(current => current.Id != Office.Id)
                    .FirstOrDefault()
                    ;

                oFindedOffice =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(current => current.Id == Office.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "تعرفه ای با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(Office);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedOffice.UpdateDateTime = DateTime.Now;
                    oFindedOffice.Name = Office.Name;
                    oFindedOffice.Code = Office.Code;

                    UnitOfWork.HeadLineRepository.Update(oFindedOffice);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "تعرفه درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(Office);
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

            ViewModels.Areas.Administrator.HeadLine.DetailViewModel oOffice
                = UnitOfWork.HeadLineRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadLine.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oOffice == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oOffice));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varOffices =
                    UnitOfWork.HeadLineRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varOffices != null)
                {
                    UnitOfWork.HeadLineRepository.Delete(varOffices);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.HeadLine.Index()));
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
