using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class BankAccountController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Index()
        {
            #region DropDownList
            var varBanks = UnitOfWork.BankRepository.Get().ToList();
            ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

            var varCertains = UnitOfWork.CertainRepository.Get().ToList();
            ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

            var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
            ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

            var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
            ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
            #endregion

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.BankAccount.SearchViewModel viewModel)
        {
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varBankAccount =
                UnitOfWork.BankAccountRepository.Get()
                ;

            #region Condition
            viewModel.AccountNumber = Utilities.Text.Utility.FixText(viewModel.AccountNumber);
            viewModel.AccountTitel = Utilities.Text.Utility.FixText(viewModel.AccountTitel);

            if (viewModel.AccountNumber != string.Empty)
            {
                varBankAccount =
                    varBankAccount
                    .Where(current => current.AccountNumber.Contains(viewModel.AccountNumber))
                    ;
            }

            if (viewModel.AccountTitel != string.Empty)
            {
                varBankAccount =
                    varBankAccount
                    .Where(current => current.AccountTitel.Contains(viewModel.AccountTitel))
                    ;
            }

            if (viewModel.Bank != null && viewModel.Bank!= new Guid())
            {
                varBankAccount = varBankAccount.Where(current => current.BankId== viewModel.Bank);
            }

            if (viewModel.Certain != null && viewModel.Certain != new Guid())
            {
                varBankAccount = varBankAccount.Where(current => current.CertainId == viewModel.Certain);
            }

            if (viewModel.ExecutableCode != null && viewModel.ExecutableCode != new Guid())
            {
                varBankAccount = varBankAccount.Where(current => current.ExecutableCodeId == viewModel.ExecutableCode);
            }

            if (viewModel.IncomeRow != null && viewModel.IncomeRow != new Guid())
            {
                varBankAccount = varBankAccount.Where(current => current.IncomeRowId == viewModel.IncomeRow);
            }
            #endregion

            #region DropDownList
            var varBanks = UnitOfWork.BankRepository.Get().ToList();
            ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

            var varCertains = UnitOfWork.CertainRepository.Get().ToList();
            ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

            var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
            ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

            var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
            ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
            #endregion

            var ViewModelsBankAccount
                 = varBankAccount
                 .OrderBy(current => current.AccountTitel)
                 .ToList()
                 .Select(current =>
                     new ViewModels.Areas.Administrator.BankAccount.IndexViewModel()
                     {
                         Id = current.Id,
                         AccountNumber = current.AccountNumber,
                         AccountTitel = current.AccountTitel,
                         Bank = current.Bank.Name,
                         Certain = current.Certain.Name,
                         ExecutableCode = current.ExecutableCode.Name,
                         IncomeRow = current.IncomeRow.Name,
                     })
                     .ToList()
                     .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.BankAccount.IndexViewModel>(ViewModelsBankAccount);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetBankAccounts()
        {
            try
            {
                var varBankAccount =
                    UnitOfWork.BankAccountRepository.Get()
                    ;

                #region DropDownList
                var varBanks = UnitOfWork.BankRepository.Get().ToList();
                ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

                var varCertains = UnitOfWork.CertainRepository.Get().ToList();
                ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

                var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
                ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

                var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
                ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
                #endregion

                var ViewModelsBankAccount
                     = varBankAccount
                 .OrderBy(current => current.AccountTitel)
                 .ToList()
                 .Select(current =>
                     new ViewModels.Areas.Administrator.BankAccount.IndexViewModel()
                     {
                         Id = current.Id,
                         AccountNumber = current.AccountNumber,
                         AccountTitel = current.AccountTitel,
                         Bank = current.Bank.Name,
                         Certain = current.Certain.Name,
                         ExecutableCode = current.ExecutableCode.Name,
                         IncomeRow = current.IncomeRow.Name,
                     })
                     .ToList()
                     .AsQueryable();

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.BankAccount.IndexViewModel>(ViewModelsBankAccount);

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;

            #region DropDownList
            var varBanks = UnitOfWork.BankRepository.Get().ToList();
            ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

            var varCertains = UnitOfWork.CertainRepository.Get().ToList();
            ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

            var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
            ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

            var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
            ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
            #endregion

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.BankAccount.CreateViewModel BankAccount)
        {
            ViewBag.PageMessages = null;

            Models.BankAccount oFindedBankAccount = new Models.BankAccount();

            oFindedBankAccount =
                UnitOfWork.BankAccountRepository
                .Get()
                .Where(current => current.BankId == BankAccount.Bank)
                .Where(current => current.CertainId == BankAccount.Certain)
                .Where(current => current.ExecutableCodeId == BankAccount.ExecutableCode)
                .Where(current => current.IncomeRowId == BankAccount.IncomeRow)
                .FirstOrDefault()
                ;

            if (oFindedBankAccount != null)
            {
                ViewBag.PageMessages += "اطلاعات حساب مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                Models.BankAccount oBankAccount = new Models.BankAccount();
                {
                    oBankAccount.BankId = BankAccount.Bank;
                    oBankAccount.CertainId = BankAccount.Certain;
                    oBankAccount.ExecutableCodeId = BankAccount.ExecutableCode;
                    oBankAccount.IncomeRowId = BankAccount.IncomeRow;
                    oBankAccount.IsActived = true;
                    oBankAccount.IsDeleted = false;
                    oBankAccount.IsSystem = false;
                    oBankAccount.IsVerified = true;
                    oBankAccount.InsertDateTime = DateTime.Now;
                    oBankAccount.UpdateDateTime = DateTime.Now;
                    oBankAccount.AccountNumber = BankAccount.AccountNumber;
                    oBankAccount.AccountTitel = BankAccount.AccountTitel;
                    UnitOfWork.BankAccountRepository.Insert(oBankAccount);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "اطلاعات حساب درخواستی شما با موفقیت ثبت گردید  ";
                }

                #region DropDownList
                var varBanks = UnitOfWork.BankRepository.Get().ToList();
                ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

                var varCertains = UnitOfWork.CertainRepository.Get().ToList();
                ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

                var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
                ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

                var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
                ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
                #endregion

            }

            return View(BankAccount);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Detail(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.BankAccount.DetailViewModel oBankAccount
                = UnitOfWork.BankAccountRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.BankAccount.DetailViewModel()
                {
                    Id = current.Id,
                    AccountNumber = current.AccountNumber,
                    AccountTitel = current.AccountTitel,
                    Bank = current.Bank.Name,
                    Certain = current.Certain.Name,
                    ExecutableCode = current.ExecutableCode.Name,
                    IncomeRow = current.IncomeRow.Name,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oBankAccount == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBankAccount));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewModels.Areas.Administrator.BankAccount.EditViewModel oBankAccount
                = UnitOfWork.BankAccountRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.BankAccount.EditViewModel()
                {
                    Id = current.Id,
                    AccountNumber = current.AccountNumber,
                    AccountTitel = current.AccountTitel,
                    Bank = current.BankId,
                    Certain = current.CertainId,
                    ExecutableCode = current.ExecutableCodeId,
                    IncomeRow = current.IncomeRowId,
                })
                .FirstOrDefault()
                ;

            #region DropDownList
            var varBanks = UnitOfWork.BankRepository.Get().ToList();
            ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

            var varCertains = UnitOfWork.CertainRepository.Get().ToList();
            ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

            var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
            ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

            var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
            ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
            #endregion

            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            if (oBankAccount == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBankAccount));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.BankAccount.EditViewModel BankAccount)
        {
            ViewBag.PageMessages = null;

            try
            {
                Models.BankAccount oFindedBankAccount;

                var OlderAccount =
                    UnitOfWork.BankAccountRepository
                    .Get()
                    .Where(current => current.Id == BankAccount.Id)
                    .FirstOrDefault()
                    ;

                var oFindedOther =
                    UnitOfWork.BankAccountRepository
                    .Get()
                    .Where(current => current.BankId == BankAccount.Bank)
                    .Where(current => current.CertainId == BankAccount.Certain)
                    .Where(current => current.ExecutableCodeId == BankAccount.ExecutableCode)
                    .Where(current => current.IncomeRowId == BankAccount.IncomeRow)
                    .Where(current => current.Id != BankAccount.Id)
                    .FirstOrDefault()
                    ;

                #region DropDownList
                var varBanks = UnitOfWork.BankRepository.Get().ToList();
                ViewData["Bank"] = new System.Web.Mvc.SelectList(varBanks, "Id", "Name", null);

                var varCertains = UnitOfWork.CertainRepository.Get().ToList();
                ViewData["Certain"] = new System.Web.Mvc.SelectList(varCertains, "Id", "Name", null);

                var varExecutableCodes = UnitOfWork.ExecutableCodeRepository.Get().ToList();
                ViewData["ExecutableCode"] = new System.Web.Mvc.SelectList(varExecutableCodes, "Id", "Name", null);

                var varIncomeRows = UnitOfWork.IncomeRowRepository.Get().ToList();
                ViewData["IncomeRow"] = new System.Web.Mvc.SelectList(varIncomeRows, "Id", "Name", null);
                #endregion

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "اطلاعات حساب با این مشخصات در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View();
                }

                else if (ModelState.IsValid)
                {
                    OlderAccount.BankId = BankAccount.Bank;
                    OlderAccount.CertainId = BankAccount.Certain;
                    OlderAccount.ExecutableCodeId = BankAccount.ExecutableCode;
                    OlderAccount.IncomeRowId = BankAccount.IncomeRow;
                    OlderAccount.AccountNumber = BankAccount.AccountNumber;
                    OlderAccount.AccountTitel = BankAccount.AccountTitel;
                    UnitOfWork.BankAccountRepository.Update(OlderAccount);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "اطلاعات حساب درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(BankAccount);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.BankAccount.DetailViewModel oBankAccount
                = UnitOfWork.BankAccountRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.BankAccount.DetailViewModel()
                {
                    Id = current.Id,
                    AccountNumber = current.AccountNumber,
                    AccountTitel = current.AccountTitel,
                    Bank = current.Bank.Name,
                    Certain = current.Certain.Name,
                    ExecutableCode = current.ExecutableCode.Name,
                    IncomeRow = current.IncomeRow.Name,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oBankAccount == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oBankAccount));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varBankAccount =
                    UnitOfWork.BankAccountRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varBankAccount != null)
                {
                    //UnitOfWork.BankAccountRepository.Delete(varBankAccount);
                    //UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.BankAccount.Index()));
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