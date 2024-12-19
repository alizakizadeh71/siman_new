using Models;
using System;
using System.Data;
using System.Linq;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class AccountNumberManageController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetAccountNumberManages()
        {
            var varAccountNumberManages =
                UnitOfWork.AccountNumberManageRepository.Get()
                ;

            var ViewModelsAccountNumberManages
                = varAccountNumberManages
                .OrderByDescending(current => current.InsertDateTime)
                .ThenBy(current => current.Province.Name)
                .ThenBy(current => current.AccountNumber.Account)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.AccountNumberManage.IndexViewModel()
                    {
                        Id = current.Id,
                        SubSystem = current.SubSystem.Name,
                        Province = current.Province.Name,
                        AccountNumber = current.AccountNumber.Account,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                        UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.AccountNumberManage.IndexViewModel>(ViewModelsAccountNumberManages);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.AccountNumberManage.SearchViewModel viewModel)
        {
            var varAccountNumberManage =
               UnitOfWork.AccountNumberManageRepository.Get()
               ;

            #region Condition
            viewModel.SubSystem = Utilities.Text.Utility.FixText(viewModel.SubSystem);
            viewModel.Province = Utilities.Text.Utility.FixText(viewModel.Province);
            viewModel.AccountNumber = Utilities.Text.Utility.FixText(viewModel.AccountNumber);

            #region SubSystem Condition
            if (viewModel.SubSystem != string.Empty)
            {
                varAccountNumberManage =
                    varAccountNumberManage
                    .Where(current => current.SubSystem.Name.Contains(viewModel.SubSystem))
                    ;
            }
            #endregion

            #region Province Condition
            if (viewModel.Province != string.Empty)
            {
                varAccountNumberManage =
                    varAccountNumberManage
                    .Where(current => current.Province.Name.Contains(viewModel.Province))
                    ;
            }
            #endregion

            #region SubSystem Condition
            if (viewModel.AccountNumber != string.Empty)
            {
                varAccountNumberManage =
                    varAccountNumberManage
                    .Where(current => current.AccountNumber.Account.Contains(viewModel.AccountNumber))
                    ;
            }
            #endregion

            #endregion

            //var qwqwqwqwqw
            //    = varAccountNumberManage.OrderBy(current => current.SubSystem.Name)
            //    .ThenBy(current => current.Province.Name)
            //    .ThenBy(current => current.AccountNumber.Name)
            //    .ToList()
            //    ;

            var ViewModelsAccountNumberManages
                = varAccountNumberManage.OrderBy(current => current.SubSystem.Name)
                .ThenBy(current => current.Province.Name)
                .ThenBy(current => current.AccountNumber.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.AccountNumberManage.IndexViewModel()
                    {
                        Id = current.Id,
                        SubSystem = current.SubSystem.Name,
                        Province = current.Province.Name,
                        AccountNumber = current.AccountNumber.Account,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                        UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.AccountNumberManage.IndexViewModel>(ViewModelsAccountNumberManages);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;
            #region DropDownList

            ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel oAccountNumberManage
    = UnitOfWork.AccountNumberManageRepository.Get().ToList()
    .Select(current => new ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel()
    {
        Id = current.Id,
        SubSystem = current.SubSystemId,
        Province = current.ProvinceId,
        AccountNumber = current.AccountNumberId,
        BaseAccountNumber = current.AccountNumber.Account
    })
    .FirstOrDefault()
    ;
            var varSubSystem = UnitOfWork.SubSystemRepository.Get().ToList();
            var varProvince = UnitOfWork.ProvinceRepository.Get().ToList();
            var varAccountNumber = UnitOfWork.AccountNumberRepository.Get().ToList();
            if (oAccountNumberManage != null)
            {
                ViewBag.SubSystem = new System.Web.Mvc.SelectList(varSubSystem, "Id", "Name", oAccountNumberManage.SubSystem);
                ViewBag.Province = new System.Web.Mvc.SelectList(varProvince, "Id", "Name", oAccountNumberManage.Province);
                ViewBag.AccountNumber = new System.Web.Mvc.SelectList(varAccountNumber, "Id", "Name", oAccountNumberManage.AccountNumber);
            }
            #endregion
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.AccountNumberManage.CreateViewModel accountnumbermanage)
        {
            ViewBag.PageMessages = null;


            Models.AccountNumberManage oFindedAccountNumberManage = new Models.AccountNumberManage();

            oFindedAccountNumberManage =
                UnitOfWork.AccountNumberManageRepository
                .Get()
                .Where(current => current.ProvinceId == accountnumbermanage.Province)
                .Where(current => current.SubSystemId == accountnumbermanage.SubSystem)
                .Where(current => current.AccountNumberId == accountnumbermanage.AccountNumber)
                .FirstOrDefault()
                ;

            if (oFindedAccountNumberManage != null)
            {
                ViewBag.PageMessages += "حساب مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                #region DropDownList

                ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel oAccountNumberManage
        = UnitOfWork.AccountNumberManageRepository.Get().ToList()
        .Select(current => new ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel()
        {
            Id = current.Id,
            SubSystem = current.SubSystemId,
            Province = current.ProvinceId,
            AccountNumber = current.AccountNumberId,
            BaseAccountNumber = current.AccountNumber.Account
        })
        .FirstOrDefault()
        ;
                var varSubSystem = UnitOfWork.SubSystemRepository.Get().ToList();
                ViewBag.SubSystem = new System.Web.Mvc.SelectList(varSubSystem, "Id", "Name", oAccountNumberManage.SubSystem);

                var varProvince = UnitOfWork.ProvinceRepository.Get().ToList();
                ViewBag.Province = new System.Web.Mvc.SelectList(varProvince, "Id", "Name", oAccountNumberManage.Province);

                var varAccountNumber = UnitOfWork.AccountNumberRepository.Get().ToList();
                ViewBag.AccountNumber = new System.Web.Mvc.SelectList(varAccountNumber, "Id", "Name", oAccountNumberManage.AccountNumber);
                #endregion

                return View();
                //return RedirectToAction("create", "accountnumbermanage");
            }

            // **************************************************
            // **************************************************
            if (ModelState.IsValid)
            {
                Models.AccountNumberManage oAccountNumberManage = new Models.AccountNumberManage();
                {
                    oAccountNumberManage.SubSystemId = accountnumbermanage.SubSystem;
                    oAccountNumberManage.IsActived = true;
                    oAccountNumberManage.IsDeleted = false;
                    oAccountNumberManage.IsSystem = false;
                    oAccountNumberManage.IsVerified = true;
                    oAccountNumberManage.ProvinceId = accountnumbermanage.Province;
                    oAccountNumberManage.AccountNumberId = accountnumbermanage.AccountNumber;
                    oAccountNumberManage.InsertDateTime = DateTime.Now;
                    oAccountNumberManage.UpdateDateTime = DateTime.Now;

                    UnitOfWork.AccountNumberManageRepository.Insert(oAccountNumberManage);
                    UnitOfWork.Save();

                    #region DropDownList
                    var varSubSystem = UnitOfWork.SubSystemRepository.Get().ToList();
                    ViewBag.SubSystem = new System.Web.Mvc.SelectList(varSubSystem, "Id", "Name", oAccountNumberManage.SubSystem);

                    var varProvince = UnitOfWork.ProvinceRepository.Get().ToList();
                    ViewBag.Province = new System.Web.Mvc.SelectList(varProvince, "Id", "Name", oAccountNumberManage.Province);

                    var varAccountNumber = UnitOfWork.AccountNumberRepository.Get().ToList();
                    ViewBag.AccountNumber = new System.Web.Mvc.SelectList(varAccountNumber, "Id", "Name", oAccountNumberManage.AccountNumber);
                    #endregion


                    ViewBag.PageMessages += "حساب درخواستی شما با موفقیت ثبت گردید  ";
                }
            }

            //return View(accountnumbermanage);

            return RedirectToAction("Index", "accountnumbermanage");

            //return View();
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

            ViewModels.Areas.Administrator.AccountNumberManage.DetailViewModel oAccountNumberManage
                = UnitOfWork.AccountNumberManageRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.AccountNumberManage.DetailViewModel()
                {
                    Id = current.Id,
                    SubSystem = current.SubSystem.Name,
                    Province = current.Province.Name,
                    AccountNumber = current.AccountNumber.Account,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult FindAccountNumber(System.Guid accountnumber)
        {
            var oAccountNumberManage
                = UnitOfWork.AccountNumberManageRepository.Get()
                .Where(current => current.AccountNumberId == accountnumber)
                .FirstOrDefault()
                ;

            return Json
                (data: oAccountNumberManage.AccountNumber.Account,
                behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel oAccountNumberManage
                = UnitOfWork.AccountNumberManageRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel()
                {
                    Id = current.Id,
                    SubSystem = current.SubSystemId,
                    Province = current.ProvinceId,
                    AccountNumber = current.AccountNumberId,
                    BaseAccountNumber = current.AccountNumber.Account
                })
                .FirstOrDefault()
                ;

            #region DropDownList
            var varSubSystem = UnitOfWork.SubSystemRepository.Get().ToList();
            ViewBag.SubSystem = new System.Web.Mvc.SelectList(varSubSystem, "Id", "Name", oAccountNumberManage.SubSystem);

            var varProvince = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewBag.Province = new System.Web.Mvc.SelectList(varProvince, "Id", "Name", oAccountNumberManage.Province);

            var varAccountNumber = UnitOfWork.AccountNumberRepository.Get().ToList();
            ViewBag.AccountNumber = new System.Web.Mvc.SelectList(varAccountNumber, "Id", "Name", oAccountNumberManage.AccountNumber);
            #endregion

            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            if (oAccountNumberManage == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oAccountNumberManage));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.AccountNumberManage.EditViewModel accountnumbermanage)
        {
            ViewBag.PageMessages = null;

            try
            {
                Models.AccountNumberManage oFindedAccountNumberManage;

                var OlderAccount =
                    UnitOfWork.AccountNumberManageRepository
                    .Get()
                    .Where(current => current.Id == accountnumbermanage.Id)
                    .FirstOrDefault()
                    ;

                var oFindedOther =
                    UnitOfWork.AccountNumberManageRepository
                    .Get()
                    .Where(current => current.SubSystemId == accountnumbermanage.SubSystem)
                    .Where(current => current.ProvinceId == accountnumbermanage.Province)
                    .Where(current => current.AccountNumberId == accountnumbermanage.AccountNumber)
                    .Where(current => current.Id != accountnumbermanage.Id)
                    .FirstOrDefault()
                    ;

                oFindedAccountNumberManage =
                    UnitOfWork.AccountNumberManageRepository
                    .Get()
                    .Where(current => current.Id == accountnumbermanage.Id)
                    .FirstOrDefault()
                    ;

                #region DropDownList
                var varSubSystem = UnitOfWork.SubSystemRepository.Get().ToList();
                ViewBag.SubSystem = new System.Web.Mvc.SelectList(varSubSystem, "Id", "Name", accountnumbermanage.SubSystem);

                var varProvince = UnitOfWork.ProvinceRepository.Get().ToList();
                ViewBag.Province = new System.Web.Mvc.SelectList(varProvince, "Id", "Name", accountnumbermanage.Province);

                var varAccountNumber = UnitOfWork.AccountNumberRepository.Get().ToList();
                ViewBag.AccountNumber = new System.Web.Mvc.SelectList(varAccountNumber, "Id", "Name", accountnumbermanage.AccountNumber);
                #endregion

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "حساب با این مشخصات در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View();
                }

                // **************************************************
                // **************************************************
                Models.AccountNumberManageLog oNewAccount = new AccountNumberManageLog();
                {
                    oNewAccount.AccountNumberId = OlderAccount.AccountNumberId;
                    oNewAccount.OldId = OlderAccount.Id;
                    oNewAccount.InsertDateTime = OlderAccount.InsertDateTime;
                    oNewAccount.IPAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    oNewAccount.IsActived = OlderAccount.IsActived;
                    oNewAccount.IsDeleted = OlderAccount.IsDeleted;
                    oNewAccount.IsSystem = OlderAccount.IsSystem;
                    oNewAccount.IsVerified = OlderAccount.IsVerified;
                    oNewAccount.ProvinceId = OlderAccount.ProvinceId;
                    oNewAccount.SubSystemId = OlderAccount.SubSystemId;
                    oNewAccount.UpdateDateTime = OlderAccount.UpdateDateTime;
                    oNewAccount.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                }

                UnitOfWork.AccountNumberManageLogRepository.Insert(oNewAccount);
                UnitOfWork.Save();

                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedAccountNumberManage.SubSystemId = accountnumbermanage.SubSystem;
                    oFindedAccountNumberManage.ProvinceId = accountnumbermanage.Province;
                    oFindedAccountNumberManage.AccountNumberId = accountnumbermanage.AccountNumber;
                    oFindedAccountNumberManage.UpdateDateTime = DateTime.Now;

                    UnitOfWork.AccountNumberManageRepository.Update(oFindedAccountNumberManage);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "زیرسیستم درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(accountnumbermanage);
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

            ViewModels.Areas.Administrator.AccountNumberManage.DetailViewModel oAccountNumberManage
                = UnitOfWork.AccountNumberManageRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.AccountNumberManage.DetailViewModel()
                {
                    Id = current.Id,
                    SubSystem = current.SubSystem.Name,
                    Province = current.Province.Name,
                    AccountNumber = current.AccountNumber.Account,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    UpdateDateTime = Infrastructure.Utility.DisplayDateTime(current.UpdateDateTime, true)
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
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.AccountNumberManageRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                // **************************************************
                // **************************************************
                Models.AccountNumberManageLog oNewAccount = new AccountNumberManageLog();
                {
                    oNewAccount.AccountNumberId = varAccountNumberManages.AccountNumberId;
                    oNewAccount.Id = varAccountNumberManages.Id;
                    oNewAccount.InsertDateTime = varAccountNumberManages.InsertDateTime;
                    oNewAccount.IPAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    oNewAccount.IsActived = varAccountNumberManages.IsActived;
                    oNewAccount.IsDeleted = varAccountNumberManages.IsDeleted;
                    oNewAccount.IsSystem = varAccountNumberManages.IsSystem;
                    oNewAccount.IsVerified = varAccountNumberManages.IsVerified;
                    oNewAccount.ProvinceId = varAccountNumberManages.ProvinceId;
                    oNewAccount.SubSystemId = varAccountNumberManages.SubSystemId;
                    oNewAccount.UpdateDateTime = varAccountNumberManages.UpdateDateTime;
                    oNewAccount.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                }

                UnitOfWork.AccountNumberManageLogRepository.Insert(oNewAccount);
                UnitOfWork.Save();

                // **************************************************
                // **************************************************
                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    UnitOfWork.AccountNumberManageRepository.Delete(varAccountNumberManages);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.AccountNumberManage.Index()));
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
