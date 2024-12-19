using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class BankController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetBanks()
        {
            var varBanks =
                UnitOfWork.BankRepository.Get()
                ;

            var ViewModelsvarBanks
                = varBanks.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.Bank.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.Bank.IndexViewModel>(ViewModelsvarBanks);

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
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.Bank.CreateViewModel Bank)
        {
            ViewBag.PageMessages = null;

            Models.Bank oBank = new Models.Bank();

            var oFindBank =
                 UnitOfWork.BankRepository
                 .Get()
                 .Where(current => current.Code == Bank.Code)
                 .FirstOrDefault()
                 ;

            if (oFindBank != null)
            {
                ViewBag.PageMessages += "بانک مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oBank.Name = Bank.Name;
                oBank.IsActived = true;
                oBank.IsDeleted = false;
                oBank.IsSystem = false;
                oBank.IsVerified = true;
                oBank.Code = Bank.Code;
                oBank.InsertDateTime = DateTime.Now;
                oBank.UpdateDateTime = DateTime.Now;

                UnitOfWork.BankRepository.Insert(oBank);
                UnitOfWork.Save();

                ViewBag.PageMessages += "بانک درخواستی شما با موفقیت ثبت گردید  ";
            }

            return View(Bank);
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

            ViewModels.Areas.Administrator.Bank.DetailViewModel oBank
                = UnitOfWork.BankRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Bank.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oBank == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBank));
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

            ViewModels.Areas.Administrator.Bank.EditViewModel oBank
                = UnitOfWork.BankRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Bank.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                })
                .FirstOrDefault()
                ;

            if (oBank == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBank));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.Bank.EditViewModel Bank)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.BankRepository
                    .Get()
                    .Where(current => current.Id == Bank.Id)
                    .FirstOrDefault()
                    ;

                Models.Bank oFindedOther;
                Models.Bank oFindedBank;

                oFindedOther =
                    UnitOfWork.BankRepository
                    .Get()
                    .Where(current => current.Code == Bank.Code)
                    .Where(current => current.Id != Bank.Id)
                    .FirstOrDefault()
                    ;

                oFindedBank =
                    UnitOfWork.BankRepository
                    .Get()
                    .Where(current => current.Id == Bank.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "بانک ای با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(Bank);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedBank.UpdateDateTime = DateTime.Now;
                    oFindedBank.Name = Bank.Name;
                    oFindedBank.Code = Bank.Code;

                    UnitOfWork.BankRepository.Update(oFindedBank);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "بانک درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(Bank);
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

            ViewModels.Areas.Administrator.Bank.DetailViewModel oBank
                = UnitOfWork.BankRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Bank.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = current.Code,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oBank == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBank));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varBanks =
                    UnitOfWork.BankRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varBanks != null)
                {
                    UnitOfWork.BankRepository.Delete(varBanks);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.Bank.Index()));
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
