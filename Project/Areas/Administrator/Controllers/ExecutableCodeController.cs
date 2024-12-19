using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class ExecutableCodeController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetExecutableCodes()
        {
            var varExecutableCodes =
                UnitOfWork.ExecutableCodeRepository.Get()
                ;

            var ViewModelsvarExecutableCodes
                = varExecutableCodes.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.ExecutableCode.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.ExecutableCode.IndexViewModel>(ViewModelsvarExecutableCodes);

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
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.ExecutableCode.CreateViewModel ExecutableCode)
        {
            ViewBag.PageMessages = null;

            Models.ExecutableCode oExecutableCode = new Models.ExecutableCode();

            var oFindExecutableCode =
                 UnitOfWork.ExecutableCodeRepository
                 .Get()
                 .Where(current => current.Code == ExecutableCode.Code)
                 .FirstOrDefault()
                 ;

            if (oFindExecutableCode != null)
            {
                ViewBag.PageMessages += "کد دستگاه اجرایی مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oExecutableCode.Name = ExecutableCode.Name;
                oExecutableCode.IsActived = true;
                oExecutableCode.IsDeleted = false;
                oExecutableCode.IsSystem = false;
                oExecutableCode.IsVerified = true;
                oExecutableCode.Code = ExecutableCode.Code;
                oExecutableCode.InsertDateTime = DateTime.Now;
                oExecutableCode.UpdateDateTime = DateTime.Now;

                UnitOfWork.ExecutableCodeRepository.Insert(oExecutableCode);
                UnitOfWork.Save();

                ViewBag.PageMessages += "کد دستگاه اجرایی درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(ExecutableCode);
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

            ViewModels.Areas.Administrator.ExecutableCode.DetailViewModel oExecutableCode
                = UnitOfWork.ExecutableCodeRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ExecutableCode.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oExecutableCode == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oExecutableCode));
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

            ViewModels.Areas.Administrator.ExecutableCode.EditViewModel oExecutableCode
                = UnitOfWork.ExecutableCodeRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ExecutableCode.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oExecutableCode == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oExecutableCode));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.ExecutableCode.EditViewModel ExecutableCode)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.ExecutableCodeRepository
                    .Get()
                    .Where(current => current.Id == ExecutableCode.Id)
                    .FirstOrDefault()
                    ;

                Models.ExecutableCode oFindedOther;
                Models.ExecutableCode oFindedExecutableCode;

                oFindedOther =
                    UnitOfWork.ExecutableCodeRepository
                    .Get()
                    .Where(current => current.Code == ExecutableCode.Code)
                    .Where(current => current.Id != ExecutableCode.Id)
                    .FirstOrDefault()
                    ;

                oFindedExecutableCode =
                    UnitOfWork.ExecutableCodeRepository
                    .Get()
                    .Where(current => current.Id == ExecutableCode.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "کد دستگاه اجرایی با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(ExecutableCode);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedExecutableCode.UpdateDateTime = DateTime.Now;
                    oFindedExecutableCode.Name = ExecutableCode.Name;
                    oFindedExecutableCode.Code = ExecutableCode.Code;

                    UnitOfWork.ExecutableCodeRepository.Update(oFindedExecutableCode);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "کد دستگاه اجرایی درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(ExecutableCode);
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

            ViewModels.Areas.Administrator.ExecutableCode.DetailViewModel oExecutableCode
                = UnitOfWork.ExecutableCodeRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ExecutableCode.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oExecutableCode == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oExecutableCode));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varExecutableCodes =
                    UnitOfWork.ExecutableCodeRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varExecutableCodes != null)
                {
                    UnitOfWork.ExecutableCodeRepository.Delete(varExecutableCodes);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.ExecutableCode.Index()));
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
