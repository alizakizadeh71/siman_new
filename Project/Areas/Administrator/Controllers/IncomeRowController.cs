using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class IncomeRowController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetIncomeRows()
        {
            var varIncomeRows =
                UnitOfWork.IncomeRowRepository.Get()
                ;

            var ViewModelsvarIncomeRows
                = varIncomeRows.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.IncomeRow.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.IncomeRow.IndexViewModel>(ViewModelsvarIncomeRows);

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
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.IncomeRow.CreateViewModel IncomeRow)
        {
            ViewBag.PageMessages = null;

            Models.IncomeRow oIncomeRow = new Models.IncomeRow();

            var oFindIncomeRow =
                 UnitOfWork.IncomeRowRepository
                 .Get()
                 .Where(current => current.Code == IncomeRow.Code)
                 .FirstOrDefault()
                 ;

            if (oFindIncomeRow != null)
            {
                ViewBag.PageMessages += "ردیف درآمدی مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oIncomeRow.Name = IncomeRow.Name;
                oIncomeRow.IsActived = true;
                oIncomeRow.IsDeleted = false;
                oIncomeRow.IsSystem = false;
                oIncomeRow.IsVerified = true;
                oIncomeRow.Code = IncomeRow.Code;
                oIncomeRow.InsertDateTime = DateTime.Now;
                oIncomeRow.UpdateDateTime = DateTime.Now;

                UnitOfWork.IncomeRowRepository.Insert(oIncomeRow);
                UnitOfWork.Save();

                ViewBag.PageMessages += "ردیف درآمدی درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(IncomeRow);
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

            ViewModels.Areas.Administrator.IncomeRow.DetailViewModel oIncomeRow
                = UnitOfWork.IncomeRowRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.IncomeRow.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oIncomeRow == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oIncomeRow));
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

            ViewModels.Areas.Administrator.IncomeRow.EditViewModel oIncomeRow
                = UnitOfWork.IncomeRowRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.IncomeRow.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oIncomeRow == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oIncomeRow));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.IncomeRow.EditViewModel IncomeRow)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.IncomeRowRepository
                    .Get()
                    .Where(current => current.Id == IncomeRow.Id)
                    .FirstOrDefault()
                    ;

                Models.IncomeRow oFindedOther;
                Models.IncomeRow oFindedIncomeRow;

                oFindedOther =
                    UnitOfWork.IncomeRowRepository
                    .Get()
                    .Where(current => current.Code == IncomeRow.Code)
                    .Where(current => current.Id != IncomeRow.Id)
                    .FirstOrDefault()
                    ;

                oFindedIncomeRow =
                    UnitOfWork.IncomeRowRepository
                    .Get()
                    .Where(current => current.Id == IncomeRow.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "ردیف درآمدی با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(IncomeRow);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedIncomeRow.UpdateDateTime = DateTime.Now;
                    oFindedIncomeRow.Name = IncomeRow.Name;
                    oFindedIncomeRow.Code = IncomeRow.Code;

                    UnitOfWork.IncomeRowRepository.Update(oFindedIncomeRow);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "ردیف درآمدی درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(IncomeRow);
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

            ViewModels.Areas.Administrator.IncomeRow.DetailViewModel oIncomeRow
                = UnitOfWork.IncomeRowRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.IncomeRow.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oIncomeRow == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oIncomeRow));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varIncomeRows =
                    UnitOfWork.IncomeRowRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varIncomeRows != null)
                {
                    UnitOfWork.IncomeRowRepository.Delete(varIncomeRows);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.IncomeRow.Index()));
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
