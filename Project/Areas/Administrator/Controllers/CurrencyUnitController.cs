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
    public partial class CurrencyUnitController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami | Enums.Roles.MaliExpert00)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami | Enums.Roles.MaliExpert00)]
        public virtual System.Web.Mvc.JsonResult GetCurrencyUnits()
        {
            var varCurrencyUnits =
                UnitOfWork.CurrencyUnitRepository.Get()
                .Where(x=>x.IsActived && !x.IsDeleted)
                ;

            var ViewModelsvarCurrencyUnits
                = varCurrencyUnits.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.CurrencyUnit.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Code = current.Code.ToString(),
                        Ratio = current.Ratio,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                        ExpireDateTime = Infrastructure.Utility.DisplayDateTime(current.ExpireDateTime, true)
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.CurrencyUnit.IndexViewModel>(ViewModelsvarCurrencyUnits);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami | Enums.Roles.MaliExpert00)]
        public virtual System.Web.Mvc.ActionResult Detail(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id==null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.CurrencyUnit.DetailViewModel oCurrencyUnit
                = UnitOfWork.CurrencyUnitRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.CurrencyUnit.DetailViewModel()
                {
                    Id=current.Id,
                    Name = current.Name,
                    Code = current.Code.ToString(),
                    Ratio = current.Ratio,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    ExpireDateTime = Infrastructure.Utility.DisplayDateTime(current.ExpireDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oCurrencyUnit == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCurrencyUnit));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewBag.PageMessages = null;

            if (id==null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            //if (Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("BA4C5D09-258F-4287-B093-9C8CF2865EBA") ||  // غلامی
            if (Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("BA4C5D09-258F-4287-B093-9C8CF2865EBB") ||  // کمرای
                Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("84879792-772E-11EA-9132-0050568D5B96") ||  // یزمونه
                Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("F2C863BA-D829-4B4F-AA35-0036287CE8FD")  // pap-ict.ir
                )
            {

                ViewModels.Areas.Administrator.CurrencyUnit.EditViewModel oCurrencyUnit
                = UnitOfWork.CurrencyUnitRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.CurrencyUnit.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Ratio = current.Ratio,
                    ExpireDateTime = current.ExpireDateTime
                })
                .FirstOrDefault()
                ;

            if (oCurrencyUnit == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCurrencyUnit));
            }
            else
            {
                return (RedirectToAction
                (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.CurrencyUnit.EditViewModel currencyunit)
        {
            ViewBag.PageMessages = null;

            if (currencyunit.ExpireDateTime < DateTime.Now)
            {
                ViewBag.PageMessages = "تاریخ اعتبار برای ارزش ریالی صحیح نمی باشد.";
                ViewBag.PageMessages += "<br/>";
                return View(currencyunit);
            }

            try
            {
                var OldValue =
                    UnitOfWork.CurrencyUnitRepository
                    .Get()
                    .Where(current => current.Id == currencyunit.Id)
                    .FirstOrDefault()
                    ;

                Models.CurrencyUnitLog oCurrencyUnitLog = new CurrencyUnitLog();
                {
                    oCurrencyUnitLog.CancellationDateTime = DateTime.Now;
                    oCurrencyUnitLog.Code = OldValue.Code;
                    oCurrencyUnitLog.ExpireDateTime = OldValue.ExpireDateTime;
                    oCurrencyUnitLog.OldId = OldValue.Id;
                    oCurrencyUnitLog.InsertDateTime = OldValue.InsertDateTime;
                    oCurrencyUnitLog.IsActived = OldValue.IsActived;
                    oCurrencyUnitLog.IsDeleted = OldValue.IsDeleted;
                    oCurrencyUnitLog.IsSystem = OldValue.IsSystem;
                    oCurrencyUnitLog.IsVerified = OldValue.IsVerified;
                    oCurrencyUnitLog.Name = OldValue.Name;
                    oCurrencyUnitLog.Ratio = OldValue.Ratio;
                    oCurrencyUnitLog.UpdateDateTime = OldValue.UpdateDateTime;
                    oCurrencyUnitLog.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
					oCurrencyUnitLog.UserIPAddress = Request.UserHostAddress;
					oCurrencyUnitLog.Browser = Request.Browser.Type; // مدل و ورژن مرورگر
				}
                UnitOfWork.CurrencyUnitLogRepository.Insert(oCurrencyUnitLog);
                UnitOfWork.Save();

                Models.CurrencyUnit oFindedOther;
                Models.CurrencyUnit oFindedCurrency;

                oFindedOther =
                    UnitOfWork.CurrencyUnitRepository
                    .Get()
                    .Where(current => current.Name == currencyunit.Name)
                    .Where(current => current.Id != currencyunit.Id)
                    .FirstOrDefault()
                    ;

                oFindedCurrency =
                    UnitOfWork.CurrencyUnitRepository
                    .Get()
                    .Where(current => current.Id == currencyunit.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "ارزی با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View(currencyunit);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedCurrency.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oFindedCurrency.Ratio = currencyunit.Ratio;
                    oFindedCurrency.ExpireDateTime = currencyunit.ExpireDateTime;
                    oFindedCurrency.UpdateDateTime = DateTime.Now;
					oFindedCurrency.UserIPAddress = Request.UserHostAddress;
					oFindedCurrency.Browser = Request.Browser.Type; // مدل و ورژن مرورگر
					UnitOfWork.CurrencyUnitRepository.Update(oFindedCurrency);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "ارز درخواستی شما با موفقیت ثبت گردید  ";
                }

                return View(currencyunit);
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

            ViewModels.Areas.Administrator.CurrencyUnit.DetailViewModel oCurrencyUnit
                = UnitOfWork.CurrencyUnitRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.CurrencyUnit.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    Code = Infrastructure.Utility.EnumValue(Enums.EnumTypes.CurrencyUnits, current.Code),
                    Ratio = current.Ratio,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    ExpireDateTime = Infrastructure.Utility.DisplayDateTime(current.ExpireDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oCurrencyUnit == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oCurrencyUnit));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varCurrencyUnits =
                    UnitOfWork.CurrencyUnitRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                Models.CurrencyUnitLog oCurrencyUnitLog = new CurrencyUnitLog();
                {
                    oCurrencyUnitLog.CancellationDateTime = DateTime.Now;
                    oCurrencyUnitLog.Code = varCurrencyUnits.Code;
                    oCurrencyUnitLog.ExpireDateTime = varCurrencyUnits.ExpireDateTime;
                    oCurrencyUnitLog.Id = varCurrencyUnits.Id;
                    oCurrencyUnitLog.InsertDateTime = varCurrencyUnits.InsertDateTime;
                    oCurrencyUnitLog.IsActived = varCurrencyUnits.IsActived;
                    oCurrencyUnitLog.IsDeleted = varCurrencyUnits.IsDeleted;
                    oCurrencyUnitLog.IsSystem = varCurrencyUnits.IsSystem;
                    oCurrencyUnitLog.IsVerified = varCurrencyUnits.IsVerified;
                    oCurrencyUnitLog.Name = varCurrencyUnits.Name;
                    oCurrencyUnitLog.Ratio = varCurrencyUnits.Ratio;
                    oCurrencyUnitLog.UpdateDateTime = varCurrencyUnits.UpdateDateTime;
                    oCurrencyUnitLog.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                }

                UnitOfWork.CurrencyUnitLogRepository.Insert(oCurrencyUnitLog);
                UnitOfWork.Save();

                ViewBag.PageMessages = string.Empty;

                if (varCurrencyUnits != null)
                {
                    UnitOfWork.CurrencyUnitRepository.Delete(varCurrencyUnits);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.CurrencyUnit.Index()));
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
