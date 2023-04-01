using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class ServiceTariffController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            var varUnits = UnitOfWork.UnitRepository.Get().ToList();
            ViewData["Unit"] = new System.Web.Mvc.SelectList(varUnits, "Id", "Name", null);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.Get().ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null);

            var varBankAccounts = UnitOfWork.BankAccountRepository.Get().ToList();
            ViewData["BankAccount"] = new System.Web.Mvc.SelectList(varBankAccounts, "Id", "AccountTitel", null);

            var varServiceTariffs = UnitOfWork.ServiceTariffRepository.Get()
                .ToList()
                .Select(x => new ViewModels.ComboboxItemGuid
                {
                    Id = x.Id,
                    Name = x.NameString
                })
                .OrderBy(current => current.Name)
                .ToList();
            ViewBag.ServiceTariffs = varServiceTariffs;

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetServiceTariffs(ViewModels.Areas.Administrator.Request.SearchViewModel viewModel)
        {
            var varServiceTariffs =
                UnitOfWork.ServiceTariffRepository.Get()
                ;

            if (viewModel.ServiceTariffs.ToString() != "")
            {
                Guid serviceTarifIda = new Guid(viewModel.ServiceTariffs.ToString());
                if (serviceTarifIda != null)
                {
                    varServiceTariffs = varServiceTariffs.Where(e => e.Id == serviceTarifIda);
                }
            }

            var ViewModelsvarServiceTariffs
                = varServiceTariffs.OrderBy(current => current.Name)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.ServiceTariff.IndexViewModel()
                    {
                        Id = current.Id,
                        Name = current.Name,
                        RCode = current.RCode,
                        VCode = current.VCode,
                        Amount = current.Amount,
                        Unit = current.Unit.Name,
                        SubHeadLine = current.SubHeadLine.Name,
                        BankAccount = current.BankAccount.AccountTitel,
                        InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                    })
                    .AsQueryable();

            object dataSource;

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.ServiceTariff.IndexViewModel>(ViewModelsvarServiceTariffs,true, out dataSource);

            Infrastructure.Sessions.SearchDataSource = dataSource;
            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;

            var varUnits = UnitOfWork.UnitRepository.Get().ToList();
            ViewData["Unit"] = new System.Web.Mvc.SelectList(varUnits, "Id", "Name", null);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.Get().ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null);

            var varBankAccounts = UnitOfWork.BankAccountRepository.Get().ToList();
            ViewData["BankAccount"] = new System.Web.Mvc.SelectList(varBankAccounts, "Id", "AccountTitel", null);

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.ServiceTariff.CreateViewModel servicetariff)
        {
            ViewBag.PageMessages = null;
            var varUnits = UnitOfWork.UnitRepository.Get().ToList();
            ViewData["Unit"] = new System.Web.Mvc.SelectList(varUnits, "Id", "Name", null);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.Get().ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null);

            var varBankAccounts = UnitOfWork.BankAccountRepository.Get().ToList();
            ViewData["BankAccount"] = new System.Web.Mvc.SelectList(varBankAccounts, "Id", "AccountTitel", null);

            Models.ServiceTariff oServiceTariff = new Models.ServiceTariff();

            var oFindServiceTariff =
                 UnitOfWork.ServiceTariffRepository
                 .Get()
                 .Where(current => current.RCode == servicetariff.RCode)
                 .FirstOrDefault()
                 ;

            if (oFindServiceTariff != null)
            {
                
                ViewBag.PageMessages += "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View();
            }

            if (ModelState.IsValid)
            {
                oServiceTariff.SubHeadLineId = servicetariff.SubHeadLine;
                oServiceTariff.Name = servicetariff.Name;
                oServiceTariff.IsActived = true;
                oServiceTariff.IsDeleted = false;
                oServiceTariff.IsSystem = false;
                oServiceTariff.IsVerified = true;
                oServiceTariff.RCode = servicetariff.RCode;
                oServiceTariff.VCode = servicetariff.VCode;
                oServiceTariff.Amount = servicetariff.Amount;
                oServiceTariff.UnitId = servicetariff.Unit;
                oServiceTariff.BankAccountId = servicetariff.BankAccount;
                oServiceTariff.InsertDateTime = DateTime.Now;
                oServiceTariff.UpdateDateTime = DateTime.Now;

                UnitOfWork.ServiceTariffRepository.Insert(oServiceTariff);
                UnitOfWork.Save();

                ViewBag.PageMessages += "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }

            return RedirectToAction("Index");
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

            ViewModels.Areas.Administrator.ServiceTariff.DetailViewModel oServiceTariff
                = UnitOfWork.ServiceTariffRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ServiceTariff.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    RCode = current.RCode,
                    VCode = current.VCode,
                    Amount = current.Amount,
                    Unit = current.Unit.Name,
                    SubHeadLine = current.SubHeadLine.Name,
                    BankAccount = current.BankAccount.AccountTitel,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true)
                })
                .FirstOrDefault()
                ;

            if (oServiceTariff == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oServiceTariff));
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

            // دسترسی ویرایش فقط به غلامی و pap
            //if (Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("BA4C5D09-258F-4287-B093-9C8CF2865EBA")  // غلامی
            if (Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("BA4C5D09-258F-4287-B093-9C8CF2865EBB")  // کمرای
              ||Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("84879792-772E-11EA-9132-0050568D5B96")  // بزمونه
              ||Infrastructure.Sessions.AuthenticatedUser.User.Id == new Guid("F2C863BA-D829-4B4F-AA35-0036287CE8FD")  // pap-ict.ir
                )
            {

                ViewModels.Areas.Administrator.ServiceTariff.EditViewModel oServiceTariff
                = UnitOfWork.ServiceTariffRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ServiceTariff.EditViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    RCode = current.RCode,
                    VCode = current.VCode,
                    Amount = current.Amount,
                    Unit = current.UnitId,
                    SubHeadLine = current.SubHeadLineId,
                    BankAccount = current.BankAccountId.Value,
                })
                .FirstOrDefault()
                ;

            if (oServiceTariff == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
            var varUnits = UnitOfWork.UnitRepository.Get().ToList();
            ViewData["Unit"] = new System.Web.Mvc.SelectList(varUnits, "Id", "Name", oServiceTariff.Unit);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.Get().ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", oServiceTariff.SubHeadLine);

            var varBankAccounts = UnitOfWork.BankAccountRepository.Get().ToList();
            ViewData["BankAccount"] = new System.Web.Mvc.SelectList(varBankAccounts, "Id", "AccountTitel", oServiceTariff.BankAccount);

            return (View(oServiceTariff));
            }
            else
            {
                return (RedirectToAction
                (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.ServiceTariff.EditViewModel servicetariff)
        {
            ViewBag.PageMessages = null;

            try
            {
                var OldValue =
                    UnitOfWork.ServiceTariffRepository
                    .Get()
                    .Where(current => current.Id == servicetariff.Id)
                    .FirstOrDefault()
                    ;

                Models.ServiceTariff oFindedOther;
                Models.ServiceTariff oFindedServiceTariff;

                oFindedOther =
                    UnitOfWork.ServiceTariffRepository
                    .Get()
                    .Where(current => current.RCode == servicetariff.RCode)
                    .Where(current => current.Id != servicetariff.Id)
                    .FirstOrDefault()
                    ;

                oFindedServiceTariff =
                    UnitOfWork.ServiceTariffRepository
                    .Get()
                    .Where(current => current.Id == servicetariff.Id)
                    .FirstOrDefault()
                    ;

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "خدماتی با نام  یا کد مشابه در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";

                    var varUnits = UnitOfWork.UnitRepository.Get().ToList();
                    ViewData["Unit"] = new System.Web.Mvc.SelectList(varUnits, "Id", "Name", OldValue.UnitId);

                    var varSubHeadLines = UnitOfWork.SubHeadLineRepository.Get().ToList();
                    ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", OldValue.SubHeadLineId);

                    var varBankAccounts = UnitOfWork.BankAccountRepository.Get().ToList();
                    ViewData["BankAccount"] = new System.Web.Mvc.SelectList(varBankAccounts, "Id", "AccountTitel", OldValue.BankAccountId);

                    return View(servicetariff);
                }


                // **************************************************
                // **************************************************
                if (ModelState.IsValid)
                {
                    oFindedServiceTariff.UpdateDateTime = DateTime.Now;
                    oFindedServiceTariff.Name = servicetariff.Name;
                    oFindedServiceTariff.RCode = servicetariff.RCode;
                    oFindedServiceTariff.VCode = servicetariff.VCode;
                    oFindedServiceTariff.UnitId = servicetariff.Unit;
                    oFindedServiceTariff.SubHeadLineId = servicetariff.SubHeadLine;
                    oFindedServiceTariff.BankAccountId = servicetariff.BankAccount;
                    oFindedServiceTariff.Amount = servicetariff.Amount;
                    UnitOfWork.ServiceTariffRepository.Update(oFindedServiceTariff);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "خدمات درخواستی شما با موفقیت ثبت گردید  ";
                }

                return RedirectToAction("Index");
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

            ViewModels.Areas.Administrator.ServiceTariff.DetailViewModel oServiceTariff
                = UnitOfWork.ServiceTariffRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.ServiceTariff.DetailViewModel()
                {
                    Id = current.Id,
                    Name = current.Name,
                    RCode = current.RCode,
                    VCode = current.VCode,
                    Amount = current.Amount,
                    Unit = current.Unit.Name,
                    SubHeadLine = current.SubHeadLine.Name,
                    BankAccount = current.BankAccount.AccountTitel,
                    InsertDateTime = Infrastructure.Utility.DisplayDateTime(current.InsertDateTime, true),
                })
                .FirstOrDefault()
                ;

            if (oServiceTariff == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oServiceTariff));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varServiceTariffs =
                    UnitOfWork.ServiceTariffRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varServiceTariffs != null)
                {
                    UnitOfWork.ServiceTariffRepository.Delete(varServiceTariffs);
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


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        public virtual FileContentResult Download()
        {
            var fileDownloadName = String.Format("FileName.xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            
            var newDataSource = Infrastructure.Sessions.SearchDataSource 
                as List<ViewModels.Areas.Administrator.ServiceTariff.IndexViewModel>;

            // Pass your ef data to method
            ExcelPackage package = GenerateExcelFile(newDataSource);

            var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;
        }


        private static ExcelPackage GenerateExcelFile(List<ViewModels.Areas.Administrator.ServiceTariff.IndexViewModel> datasource)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Application = "OPS.IVO.IR";
            excelPackage.Workbook.Properties.Author = Infrastructure.Sessions.AuthenticatedUser.UserName;
            excelPackage.Workbook.Properties.Comments = (new Infrastructure.Calander(DateTime.Now)).Persion();
            excelPackage.Workbook.Properties.Subject = "گزارشات تعرفه";
            excelPackage.Workbook.Properties.Title = "گزارشات تعرفه";
            //excelPackage.Workbook.Properties.SetCustomPropertyValue("IsRightToLeft",true);
            //var dfsbgfg1 = excelPackage.Workbook.Properties.GetCustomPropertyValue("IsRightToLeft");

            //Create the worksheet 
            ExcelWorksheet WorkSheet = excelPackage.Workbook.Worksheets.Add("گزارشات تعرفه");
            WorkSheet.Cells.AutoFitColumns(100, 400);
            WorkSheet.Cells[1, 1].Value = "گزارشات تعرفه سیستم پرداخت آنلاین سازمان دامپزشکی کل کشور";
            WorkSheet.Cells[1, 1, 1, 13].Merge = true;

            // Sets Headers
            WorkSheet.Cells[2, 1].Value = Resources.Model.ServiceTariff.Name;
            WorkSheet.Cells[2, 2].Value = Resources.Model.ServiceTariff.RCode;
            WorkSheet.Cells[2, 3].Value = Resources.Model.ServiceTariff.VCode;
            WorkSheet.Cells[2, 4].Value = Resources.Model.ServiceTariff.Amount;
            WorkSheet.Cells[2, 5].Value = Resources.Model.ServiceTariff.Unit;
            WorkSheet.Cells[2, 6].Value = Resources.Model.ServiceTariff.SubHeadLine;
            WorkSheet.Cells[2, 7].Value = Resources.Model.ServiceTariff.BankAccount;
            WorkSheet.Cells[2, 8].Value = Resources.Model.ServiceTariff.InsertDateTime;

            #region Cell Borders
            WorkSheet.Cells[2, 1].Style.Border.Top.Style
                = WorkSheet.Cells[2, 1].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 1].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 2].Style.Border.Top.Style
                = WorkSheet.Cells[2, 2].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 2].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 3].Style.Border.Top.Style
                = WorkSheet.Cells[2, 3].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 3].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 4].Style.Border.Top.Style
                = WorkSheet.Cells[2, 4].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 4].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 5].Style.Border.Top.Style
                = WorkSheet.Cells[2, 5].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 5].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 6].Style.Border.Top.Style
                = WorkSheet.Cells[2, 6].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 6].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 7].Style.Border.Top.Style
                = WorkSheet.Cells[2, 7].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 7].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            WorkSheet.Cells[2, 8].Style.Border.Top.Style
                = WorkSheet.Cells[2, 8].Style.Border.Bottom.Style
                = WorkSheet.Cells[2, 8].Style.Border.Right.Style
                = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            
            #endregion


            // Inserts Data
            for (int i = 0; i < datasource.Count(); i++)
            {
                #region Row Value
                WorkSheet.Cells[i + 3, 1].Value = datasource.ElementAt(i).Name;
                WorkSheet.Cells[i + 3, 2].Value = datasource.ElementAt(i).RCode;
                WorkSheet.Cells[i + 3, 3].Value = datasource.ElementAt(i).VCode;
                WorkSheet.Cells[i + 3, 4].Value = datasource.ElementAt(i).Amount;
                WorkSheet.Cells[i + 3, 5].Value = datasource.ElementAt(i).Unit;
                WorkSheet.Cells[i + 3, 6].Value = datasource.ElementAt(i).SubHeadLine;
                WorkSheet.Cells[i + 3, 7].Value = datasource.ElementAt(i).BankAccount;
                WorkSheet.Cells[i + 3, 8].Value = datasource.ElementAt(i).InsertDateTime;
                #endregion

                #region Cell Borders
                WorkSheet.Cells[i + 3, 1].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 1].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 1].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 2].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 2].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 2].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 3].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 3].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 3].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 4].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 4].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 4].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 5].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 5].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 5].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 6].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 6].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 6].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 7].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 7].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 7].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                WorkSheet.Cells[i + 3, 8].Style.Border.Top.Style
                    = WorkSheet.Cells[i + 3, 8].Style.Border.Bottom.Style
                    = WorkSheet.Cells[i + 3, 8].Style.Border.Right.Style
                    = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                #endregion
            }


            WorkSheet.Column(1).AutoFit(25);
            WorkSheet.Column(2).AutoFit(30);
            WorkSheet.Column(3).AutoFit(30);
            WorkSheet.Column(4).AutoFit(15);
            WorkSheet.Column(5).AutoFit(15);
            WorkSheet.Column(6).AutoFit(15);
            WorkSheet.Column(7).AutoFit(15);
            WorkSheet.Column(8).AutoFit(15);


            // Format Header of Table
            using (ExcelRange rng = WorkSheet.Cells["A1:H1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }

            using (ExcelRange rng = WorkSheet.Cells["A2:H2"])
            {
                rng.Style.Font.Bold = true;
                //WorkSheet.Cells["A2:L2"].AutoFilter = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
            }
            return excelPackage;
        }


    }
}
