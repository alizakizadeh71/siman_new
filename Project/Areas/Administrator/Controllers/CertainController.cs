using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class CertainController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetCertains()
        {
            var varCertains =
                UnitOfWork.CertainRepository.Get()
                ;

            var ViewModelsvarCertains
                = varCertains.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.Certain.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.Certain.IndexViewModel>(ViewModelsvarCertains);

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
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.Certain.CreateViewModel Certain)
        {
            ViewBag.PageMessages = null;

            Models.Certain oCertain = new Models.Certain();

            var oFindCertain =
                 UnitOfWork.CertainRepository
                 .Get()
                 .Where(current => current.Code == Certain.Code)
                 .FirstOrDefault()
                 ;

            if (oFindCertain != null)
            {
                ViewBag.PageMessages += "کد معین مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oCertain.Name = Certain.Name;
                oCertain.IsActived = true;
                oCertain.IsDeleted = false;
                oCertain.IsSystem = false;
                oCertain.IsVerified = true;
                oCertain.Code = Certain.Code;
                oCertain.InsertDateTime = DateTime.Now;
                oCertain.UpdateDateTime = DateTime.Now;

                UnitOfWork.CertainRepository.Insert(oCertain);
                UnitOfWork.Save();

                ViewBag.PageMessages += "کد معین درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(Certain);
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

            ViewModels.Areas.Administrator.Certain.DetailViewModel oCertain
                = UnitOfWork.CertainRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Certain.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oCertain == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCertain));
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

            ViewModels.Areas.Administrator.Certain.EditViewModel oCertain
                = UnitOfWork.CertainRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Certain.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oCertain == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCertain));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Certain.EditViewModel Certain)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.CertainRepository
                    .Get()
                    .Where(current => current.Id == Certain.Id)
                    .FirstOrDefault()
                    ;

                Models.Certain oFindedOther;
                Models.Certain oFindedCertain;

                oFindedOther =
                    UnitOfWork.CertainRepository
                    .Get()
                    .Where(current => current.Code == Certain.Code)
                    .Where(current => current.Id != Certain.Id)
                    .FirstOrDefault()
                    ;

                oFindedCertain =
                    UnitOfWork.CertainRepository
                    .Get()
                    .Where(current => current.Id == Certain.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "کد معین با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(Certain);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedCertain.UpdateDateTime = DateTime.Now;
                    oFindedCertain.Name = Certain.Name;
                    oFindedCertain.Code = Certain.Code;

                    UnitOfWork.CertainRepository.Update(oFindedCertain);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "کد معین درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(Certain);
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

            ViewModels.Areas.Administrator.Certain.DetailViewModel oCertain
                = UnitOfWork.CertainRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Certain.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oCertain == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCertain));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varCertains =
                    UnitOfWork.CertainRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varCertains != null)
                {
                    UnitOfWork.CertainRepository.Delete(varCertains);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.Certain.Index()));
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
