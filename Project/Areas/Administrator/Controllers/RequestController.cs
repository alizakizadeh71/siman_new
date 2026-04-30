using Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OPS.Controllers;
using OPS.ir.shaparak.sadad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Cement;
using ViewModels.Areas.Administrator.Request;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class RequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private MerchantUtility oMerchantUtility = new MerchantUtility();

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", null).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(Guid.Empty).ToList(); /// نوع کالا
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", null).OrderByDescending(x => x.Text); /// تیپ یک

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(Guid.Empty).ToList(); /// کیسه
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", null).OrderByDescending(x => x.Text);

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(Guid.Empty).ToList(); /// سیمان
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", null).OrderBy(x => x.Text);

            var stringFinalApprove = Infrastructure.Utility.EnumList(Enums.EnumTypes.FinalApprove);
            base.ViewData["stringFinalApprove"] = new System.Web.Mvc.SelectList(stringFinalApprove, "Id", "Name", 1);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(Guid.Empty).ToList();
            base.ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            // ایجاد لیست مقادیر برای دراپ‌داون وضعیت درخواست بر اساس Enum
            var requestStateList = new System.Collections.Generic.List<System.Web.Mvc.SelectListItem>
            {
                new System.Web.Mvc.SelectListItem { Value = "1", Text = "تایید مالی نشده" },
                new System.Web.Mvc.SelectListItem { Value = "2", Text = "تایید مالی" },
                new System.Web.Mvc.SelectListItem { Value = "3", Text = "بارگیری شده" },
                new System.Web.Mvc.SelectListItem { Value = "4", Text = "به مقصد رسیده" },
                new System.Web.Mvc.SelectListItem { Value = "5", Text = "تحویل داده شده" }
            };

            base.ViewData["RequestState"] = new System.Web.Mvc.SelectList(requestStateList, "Value", "Text", null);
            // استفاده از ViewModel جدید
            var viewModel = new ViewModels.Areas.Administrator.Request.IndexViewModel();
            return View(viewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (System.Web.Mvc.JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Request.IndexViewModel viewModel)
        {
            var varRequest = UnitOfWork.FactorCementRepository.Get();

            if (viewModel == null)
            {
                varRequest = varRequest.Where(x => x.FinalApprove == true);
            }
            else
            {
                #region Condition

                if (!string.IsNullOrEmpty(viewModel.BuyerMobile))
                {
                    viewModel.BuyerMobile = Utilities.Text.Utility.FixText(viewModel.BuyerMobile);
                    varRequest = varRequest.Where(x => x.BuyerMobile == viewModel.BuyerMobile);
                }

                if (viewModel.InvoiceNumber != null && viewModel.InvoiceNumber != 0)
                {
                    varRequest = varRequest.Where(x => x.InvoiceNumber == viewModel.InvoiceNumber);
                }

                if (viewModel.ProductName != null && viewModel.ProductName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProductNameId == viewModel.ProductName);

                if (viewModel.ProductType != null && viewModel.ProductType != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProductTypeId == viewModel.ProductType);

                if (viewModel.PackageType != null && viewModel.PackageType != Guid.Empty)
                    varRequest = varRequest.Where(x => x.PackageTypeId == viewModel.PackageType);

                if (viewModel.FactoryName != null && viewModel.FactoryName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.FactoryNameId == viewModel.FactoryName);

                if (viewModel.stringFinalApprove == "1")
                {
                    viewModel.FinalApprove = true;
                    varRequest = varRequest.Where(x => x.FinalApprove == viewModel.FinalApprove);
                }
                else if (viewModel.stringFinalApprove == "0")
                {
                    viewModel.FinalApprove = false;
                    varRequest = varRequest.Where(x => x.FinalApprove == viewModel.FinalApprove);
                }

                if (viewModel.Province != null && viewModel.Province != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);

                if (viewModel.City != null && viewModel.City != Guid.Empty)
                    varRequest = varRequest.Where(x => x.CityId == viewModel.City);

                if (viewModel.FromAmount != null && viewModel.ToAmount != null && viewModel.FromAmount > 0 && viewModel.ToAmount > 0 && viewModel.FromAmount <= viewModel.ToAmount)
                    varRequest = varRequest.Where(current => current.AmountPaid >= viewModel.FromAmount && current.AmountPaid <= viewModel.ToAmount);

                if (viewModel.StartDate != null)
                    varRequest = varRequest.Where(current => current.InsertDateTime >= viewModel.StartDate);

                if (viewModel.EndDate != null)
                {
                    var EndDate2 = viewModel.EndDate.Value.AddDays(1);
                    varRequest = varRequest.Where(current => current.InsertDateTime < EndDate2);
                }

                if (viewModel.PayStartDate != null)
                    varRequest = varRequest.Where(current => current.AmountPaidDate >= viewModel.PayStartDate);

                if (viewModel.PayEndDate != null)
                {
                    var PayEndDate2 = viewModel.PayEndDate.Value.AddDays(1);
                    varRequest = varRequest.Where(current => current.AmountPaidDate < PayEndDate2);
                }

                #endregion
            }

            try
            {
                var mappedRequests = varRequest
                    // اعمال مرتب‌سازی: ابتدا تایید نشده‌ها (false)، سپس تایید شده‌ها (true)، و در نهایت شماره فاکتور نزولی
                    .OrderBy(current => current.RequestState == (int) RequestState.PendingFinancialApproval)
                    .ThenByDescending(current => current.InvoiceNumber)
                    .ToList()
                    .Select(current => new ViewModels.Areas.Administrator.Request.IndexViewModel()
                    {
                        Id = current.Id,
                        InvoiceNumber = current.InvoiceNumber,
                        RequestState = current.RequestState,
                        StringProductName = current.ProductName?.Name,
                        StringProductType = current.ProductType?.Name,
                        StringPackageType = current.PackageType?.Name,
                        StringFactoryName = current.FactoryName?.Name,
                        StringTonnage = current.Tonnagedouble.ToString(),
                        Address = current.Address,
                        BuyerMobile = current.BuyerMobile,
                        Description = current.Description,
                        stringFinalApprove = current.FinalApprove == true ? "نهایی شده" : "نهایی نشده",
                        AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? (current.DestinationAmountPaid ?? 0) : 0,
                        MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                        StringInsertDateTime = current.InsertDateTime != null ? new Infrastructure.Calander(current.InsertDateTime).Persion() : "",
                    })
                    .AsQueryable();

                var varResult = Utilities.Kendo.HtmlHelpers.ParseGridData<ViewModels.Areas.Administrator.Request.IndexViewModel>(mappedRequests);

                return Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Data = new System.Collections.Generic.List<string>(), Total = 0 }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        public virtual ActionResult ShowFactor(int? invoicenumber, bool isPdf = false)
        {
            if (!invoicenumber.HasValue)
            {
                return HttpNotFound("شماره فاکتور مشخص نشده است.");
            }
            var oFactorCement = UnitOfWork.FactorCementRepository.GetByinvoicenumber(invoicenumber.Value).FirstOrDefault();
            var cementViewModel = ZarinpalController.ConvertCementViewModel(oFactorCement);

            if (isPdf)
            {
                return new Rotativa.ViewAsPdf("ShowFactor", cementViewModel)
                {
                    FileName = $"Factor_{invoicenumber}.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait
                };
            }
            else
            {
                return View(cementViewModel);
            }
        }

        private void ViewData(ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel)
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", cementViewModel.ProductName).OrderByDescending(x => x.Text);

            var ProductType = UnitOfWork.ProductTypeRepository.GetByProductNameId(cementViewModel.ProductName).ToList();
            base.ViewData["ProductType"] = new System.Web.Mvc.SelectList(ProductType, "Id", "Name", cementViewModel.ProductType).OrderByDescending(x => x.Text);

            var PackageType = UnitOfWork.PackageTypeRepository.GetByProductTypeId(cementViewModel.ProductType).ToList();
            base.ViewData["PackageType"] = new System.Web.Mvc.SelectList(PackageType, "Id", "Name", cementViewModel.PackageType).OrderByDescending(x => x.Text);

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(cementViewModel.ProductName).ToList();
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", cementViewModel.FactoryName).OrderBy(x => x.Text);

            var Tonnage = UnitOfWork.tonnageRepository.GetByPackageTypeId(cementViewModel.PackageType).ToList();
            base.ViewData["Tonnage"] = new System.Web.Mvc.SelectList(Tonnage, "Id", "Name", cementViewModel.Tonnage).OrderBy(x => x.Text);

            var Province = UnitOfWork.ProvinceRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(Province, "Id", "Name", cementViewModel.Province).OrderBy(x => x.Text);

            var City = UnitOfWork.CityRepository.GetByProvinceId(cementViewModel.Province ?? Guid.Empty).ToList();
            base.ViewData["City"] = new System.Web.Mvc.SelectList(City, "Id", "Name", cementViewModel.City).OrderBy(x => x.Text);
        }

        private string GetCity(Guid? cityId)
        {
            if (cityId != null)
                return UnitOfWork.CityRepository.GetById(cityId.Value).Name;
            return string.Empty;
        }

        private string GetServiceTariff(Guid? serviceTariffId)
        {
            if (serviceTariffId != null)
            {
                var ser = UnitOfWork.ServiceTariffRepository.GetById(serviceTariffId.Value);
                return ser.Name + ser.Amount;
            }
            return string.Empty;
        }

        //-----------------------------------------

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DetailsInvoiceNumber(int id)
        {
            try
            {
                if (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri != Infrastructure.WebServiceSetting_Sadad.ImportReffererUrl)
                {
                    return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest));
                }

                var oRequest = UnitOfWork.RequestRepository.Get().FirstOrDefault(current => current.InvoiceNumber == id);

                if (oRequest == null)
                {
                    return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound));
                }

                return View(oRequest);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Display(Guid id)
        {
            try
            {
                ViewBag.MessageList = UnitOfWork.MessageRepository.MetMessageByRequestId(id);
                ViewBag.PageMessages = null;

                var oRequest = UnitOfWork.FactorCementRepository.GetByUser(Infrastructure.Sessions.AuthenticatedUser.User)
                 .Where(current => current.Id == id)
                 .Select(current => new ViewModels.Areas.Administrator.Cement.CementViewModel()
                 {
                     Id = current.Id,
                     InvoiceNumber = current.InvoiceNumber,
                     StringProductName = current.ProductName.Name,
                     StringProductType = current.ProductType.Name,
                     StringPackageType = current.PackageType.Name,
                     StringFactoryName = current.FactoryName.Name,
                     StringTonnage = current.Tonnage.Name,
                     BuyerMobile = current.BuyerMobile,
                     AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? (current.DestinationAmountPaid ?? 0) : 0,
                     MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                     StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                     Description = current.Description,
                 })
                 .FirstOrDefault();

                if (oRequest == null)
                {
                    return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound));
                }

                return View(oRequest);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Edit(Guid id)
        {
            var cementViewModel = UnitOfWork.FactorCementRepository.Get()
            .Where(current => current.Id == id)
            .Select(current => new CementViewModel()
            {
                Id = current.Id,
                InvoiceNumber = current.InvoiceNumber,
                StringProductName = current.ProductName.Name,
                StringProductType = current.ProductType.Name,
                StringPackageType = current.PackageType.Name,
                StringFactoryName = current.FactoryName.Name,
                StringTonnage = current.Tonnage.Name,
                StringProvince = current.Province.Name,
                StringCity = current.City.Name,
                BuyerMobile = current.BuyerMobile,
                AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? (current.DestinationAmountPaid ?? 0) : 0,
                MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                Description = current.Description,
                RemittanceNumber = current.RemittanceNumber
            })
            .FirstOrDefault();

            ViewBag.PageMessages = null;

            return View(cementViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(CementViewModel cementViewModel)
        {
            ViewBag.PageMessages = null;

            try
            {
                var olderAccount = UnitOfWork.FactorCementRepository.Get().FirstOrDefault(current => current.Id == cementViewModel.Id);

                if (olderAccount != null)
                {
                    olderAccount.InvoiceNumber = cementViewModel.InvoiceNumber;
                    olderAccount.ProductName.Name = cementViewModel.StringProductName;
                    olderAccount.ProductType.Name = cementViewModel.StringProductType;
                    olderAccount.PackageType.Name = cementViewModel.StringPackageType;
                    olderAccount.FactoryName.Name = cementViewModel.StringFactoryName;
                    olderAccount.Tonnage.Name = cementViewModel.StringTonnage;
                    olderAccount.Province.Name = cementViewModel.StringProvince;
                    olderAccount.City.Name = cementViewModel.StringCity;
                    olderAccount.BuyerMobile = cementViewModel.BuyerMobile;
                    olderAccount.AmountPaid = cementViewModel.AmountPaid;
                    olderAccount.MahalTahvil = cementViewModel.MahalTahvil;
                    olderAccount.RemittanceNumber = cementViewModel.RemittanceNumber;
                    olderAccount.UpdateDateTime = DateTime.Now;

                    UnitOfWork.FactorCementRepository.Update(olderAccount);
                    UnitOfWork.Save();

                    ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ویرایش گردید";
                }

                return View(cementViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound));
            }
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult ViewFiles(string Id)
        {
            try
            {
                Guid requestId = new Guid(Id);

                var oFile =
                 UnitOfWork.FileRepository.Get()
                 .Where(current => current.RequestId == requestId)
                 .ToList()
                 ;

                if (oFile.Count > 0)
                {
                    List<Models.File> oFinalFile = new List<Models.File>();
                    foreach (var file in oFile)
                    {
                        Models.File newFile = new Models.File();
                        newFile.Id = file.Id;
                        newFile.FileAddress = file.FileAddress;
                        newFile.InsertDateTime = file.InsertDateTime;
                        newFile.IsActived = file.IsActived;
                        newFile.IsDeleted = file.IsDeleted;
                        newFile.IsSystem = file.IsSystem;
                        newFile.IsVerified = file.IsVerified;
                        newFile.Name = file.Name;
                        newFile.RequestId = file.RequestId;
                        newFile.UpdateDateTime = file.UpdateDateTime;
                        oFinalFile.Add(newFile);
                    }
                    return Json(data: oFinalFile, behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: oFile, behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }


        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert01)]
        //public virtual FileContentResult Download()
        //{
        //    var fileDownloadName = String.Format("FileName.xlsx");
        //    const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    //var varRequest =
        //    //    UnitOfWork.RequestRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
        //    //    ;

        //    DateTime nowDateTime = DateTime.Now.AddDays(-3);
        //    int PaymentConfirmation = (int)RequestState.Delivered;

        //    //var varRequestTypes = UnitOfWork.SubSystemRepository.Get().OrderBy(current => current.Name).ToList();
        //    //ViewData["SubSystem"] = new System.Web.Mvc.SelectList(varRequestTypes, "Id", "Name", null);

        //    var newDataSource = Infrastructure.Sessions.SearchDataSource as List<ViewModels.Areas.Administrator.Request.IndexViewModel>;

        //    // Pass your ef data to method
        //    //ExcelPackage package = GenerateExcelFile(newDataSource);

        //    var fsr = new FileContentResult(package.GetAsByteArray(), contentType);
        //    fsr.FileDownloadName = fileDownloadName;

        //    return fsr;
        //}

        //private static ExcelPackage GenerateExcelFile(List<ViewModels.Areas.Administrator.Request.IndexViewModel> datasource)
        //{
        //    //*****************************************************//
        //    //*****************************************************//
        //    //http://www.codeproject.com/Articles/680421/Create-Read-Edit-Advance-Excel-Report-in
        //    //https://epplus.codeplex.com/wikipage?title=FAQ
        //    //*****************************************************//
        //    //*****************************************************//

        //    ExcelPackage excelPackage = new ExcelPackage();
        //    excelPackage.Workbook.Properties.Application = "OPS.IVO.IR";
        //    excelPackage.Workbook.Properties.Author = Infrastructure.Sessions.AuthenticatedUser.UserName;
        //    excelPackage.Workbook.Properties.Comments = (new Infrastructure.Calander(DateTime.Now)).Persion();
        //    excelPackage.Workbook.Properties.Subject = "گزارشات مالی";
        //    excelPackage.Workbook.Properties.Title = "گزارشات مالی";
        //    //excelPackage.Workbook.Properties.SetCustomPropertyValue("IsRightToLeft",true);
        //    //var dfsbgfg1 = excelPackage.Workbook.Properties.GetCustomPropertyValue("IsRightToLeft");

        //    //Create the worksheet 
        //    ExcelWorksheet WorkSheet = excelPackage.Workbook.Worksheets.Add("گزارشات مالی");
        //    WorkSheet.Cells.AutoFitColumns(100, 400);
        //    WorkSheet.Cells[1, 1].Value = "گزارشات مالی سیستم پرداخت آنلاین سازمان دامپزشکی کل کشور";
        //    WorkSheet.Cells[1, 1, 1, 12].Merge = true;

        //    // Sets Headers
        //    WorkSheet.Cells[2, 1].Value = Resources.Model.Request.SubSystem;
        //    WorkSheet.Cells[2, 2].Value = Resources.Model.Request.CompanyName;
        //    WorkSheet.Cells[2, 3].Value = Resources.Model.Request.ServiceTariff;
        //    WorkSheet.Cells[2, 4].Value = Resources.Model.Request.CompanyNationalCode;
        //    WorkSheet.Cells[2, 5].Value = Resources.Model.Request.InvoiceNumber;
        //    WorkSheet.Cells[2, 6].Value = Resources.Model.Request.InvoiceDate;
        //    WorkSheet.Cells[2, 7].Value = Resources.Model.Request.RecordNumber;
        //    WorkSheet.Cells[2, 8].Value = Resources.Model.Request.RecordDate;
        //    WorkSheet.Cells[2, 9].Value = Resources.Model.Request.RequestState;
        //    WorkSheet.Cells[2, 10].Value = Resources.Model.Request.Province;
        //    WorkSheet.Cells[2, 11].Value = Resources.Model.Request.AmountPaid;
        //    WorkSheet.Cells[2, 12].Value = Resources.Model.Request.City;
        //    WorkSheet.Cells[2, 13].Value = Resources.Model.Request.DepositNumber;
        //    WorkSheet.Cells[2, 14].Value = Resources.Model.Request.BaseCurrencyValue;
        //    WorkSheet.Cells[2, 15].Value = Resources.Model.Request.CurrencyUnit;
        //    WorkSheet.Cells[2, 16].Value = Resources.Model.Request.Bank_TraceNo;
        //    WorkSheet.Cells[2, 17].Value = " کاربر سازنده ";
        //    WorkSheet.Cells[2, 18].Value = Resources.Model.Request.AmountPaidDate;
        //    #region Cell Borders
        //    WorkSheet.Cells[2, 1].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 1].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 1].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 2].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 2].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 2].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 3].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 3].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 3].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 4].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 4].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 4].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 5].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 5].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 5].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 6].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 6].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 6].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 7].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 7].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 7].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 8].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 8].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 8].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 9].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 9].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 9].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 10].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 10].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 10].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 11].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 11].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 11].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


        //    WorkSheet.Cells[2, 12].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 12].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 12].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 13].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 13].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 13].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 14].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 14].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 14].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 15].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 15].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 15].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 16].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 16].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 16].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 17].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 17].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 17].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //    WorkSheet.Cells[2, 18].Style.Border.Top.Style
        //        = WorkSheet.Cells[2, 18].Style.Border.Bottom.Style
        //        = WorkSheet.Cells[2, 18].Style.Border.Right.Style
        //        = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //    #endregion


        //    // Inserts Data
        //    for (int i = 0; i < datasource.Count(); i++)
        //    {
        //        #region Row Value
        //        WorkSheet.Cells[i + 3, 1].Value = datasource.ElementAt(i).SubSystem;
        //        WorkSheet.Cells[i + 3, 2].Value = datasource.ElementAt(i).CompanyName;
        //        WorkSheet.Cells[i + 3, 3].Value = datasource.ElementAt(i).ServiceTariff;
        //        WorkSheet.Cells[i + 3, 4].Value = datasource.ElementAt(i).CompanyNationalCode;
        //        WorkSheet.Cells[i + 3, 5].Value = datasource.ElementAt(i).InvoiceNumber;
        //        WorkSheet.Cells[i + 3, 6].Value = datasource.ElementAt(i).InvoiceDate;
        //        WorkSheet.Cells[i + 3, 7].Value = datasource.ElementAt(i).RecordNumber;
        //        WorkSheet.Cells[i + 3, 8].Value = datasource.ElementAt(i).RecordDate;
        //        WorkSheet.Cells[i + 3, 9].Value = datasource.ElementAt(i).RequestState;
        //        WorkSheet.Cells[i + 3, 10].Value = datasource.ElementAt(i).Province;
        //        WorkSheet.Cells[i + 3, 11].Value = datasource.ElementAt(i).AmountPaid;
        //        WorkSheet.Cells[i + 3, 12].Value = datasource.ElementAt(i).City;
        //        WorkSheet.Cells[i + 3, 13].Value = datasource.ElementAt(i).DepositNumber;
        //        WorkSheet.Cells[i + 3, 14].Value = datasource.ElementAt(i).BaseCurrencyValue;
        //        WorkSheet.Cells[i + 3, 15].Value = datasource.ElementAt(i).CurrencyUnit;
        //        WorkSheet.Cells[i + 3, 16].Value = datasource.ElementAt(i).Bank_TraceNo;
        //        WorkSheet.Cells[i + 3, 17].Value = datasource.ElementAt(i).CeratedBy;
        //        WorkSheet.Cells[i + 3, 18].Value = datasource.ElementAt(i).AmountPaidDate;

        //        #endregion

        //        #region Cell Borders
        //        WorkSheet.Cells[i + 3, 1].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 1].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 1].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 2].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 2].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 2].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 3].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 3].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 3].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 4].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 4].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 4].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 5].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 5].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 5].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 6].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 6].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 6].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 7].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 7].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 7].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 8].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 8].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 8].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 9].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 9].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 9].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 10].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 10].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 10].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 11].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 11].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 11].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 12].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 12].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 12].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 13].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 13].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 13].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 14].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 14].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 14].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 15].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 15].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 15].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 16].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 16].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 16].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 17].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 17].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 17].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        //        WorkSheet.Cells[i + 3, 18].Style.Border.Top.Style
        //            = WorkSheet.Cells[i + 3, 18].Style.Border.Bottom.Style
        //            = WorkSheet.Cells[i + 3, 18].Style.Border.Right.Style
        //            = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //        #endregion
        //    }


        //    WorkSheet.Column(1).AutoFit(25);
        //    WorkSheet.Column(2).AutoFit(30);
        //    WorkSheet.Column(3).AutoFit(30);
        //    WorkSheet.Column(4).AutoFit(15);
        //    WorkSheet.Column(5).AutoFit(15);
        //    WorkSheet.Column(6).AutoFit(15);
        //    WorkSheet.Column(7).AutoFit(15);
        //    WorkSheet.Column(8).AutoFit(15);
        //    WorkSheet.Column(9).AutoFit(20);
        //    WorkSheet.Column(10).AutoFit(20);
        //    WorkSheet.Column(11).AutoFit(20);
        //    WorkSheet.Column(12).AutoFit(20);
        //    WorkSheet.Column(13).AutoFit(20);
        //    WorkSheet.Column(14).AutoFit(20);
        //    WorkSheet.Column(15).AutoFit(20);
        //    WorkSheet.Column(16).AutoFit(20);
        //    WorkSheet.Column(17).AutoFit(20);
        //    WorkSheet.Column(18).AutoFit(20);

        //    // Format Header of Table
        //    using (ExcelRange rng = WorkSheet.Cells["A1:I1"])
        //    {
        //        rng.Style.Font.Bold = true;
        //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
        //        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke); //Set color to DarkGray 
        //        rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
        //    }

        //    using (ExcelRange rng = WorkSheet.Cells["A2:Q2"])

        //    {
        //        rng.Style.Font.Bold = true;
        //        //WorkSheet.Cells["A2:L2"].AutoFilter = true;
        //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
        //        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); //Set color to DarkGray 
        //        rng.Style.Font.Color.SetColor(System.Drawing.Color.Black);
        //    }
        //    return excelPackage;
        //}

        [HttpPost]
        // در صورت نیاز اتریبیوت دسترسی (SyncPermission) مربوط به مدیر مالی را اینجا اضافه کنید
        public virtual System.Web.Mvc.ActionResult ApproveFinancial(System.Guid id, string description)
        {
            try
            {
                // دریافت سفارش از دیتابیس
                var order = UnitOfWork.FactorCementRepository.GetById(id);

                if (order == null)
                {
                    return HttpNotFound("سفارش یافت نشد.");
                }

                // تغییر وضعیت به تایید مالی
                order.RequestState = (int)Enums.RequestState.FinanciallyApproved;

                // ذخیره توضیحات تایید مالی (نام این پروپرتی را با مدل دیتابیس خود جایگزین کنید)
                order.Description = description; // یا مثلا order.FinancialDescription = description;

                // ذخیره تغییرات
                UnitOfWork.FactorCementRepository.Update(order);
                UnitOfWork.Save();

                TempData["SuccessMessage"] = "تایید مالی با موفقیت انجام شد و سفارش به کارتابل باربری منتقل گردید.";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "خطایی در تایید مالی رخ داد: " + ex.Message;
            }

            // بازگشت به صفحه لیست سفارشات ادمین
            return RedirectToAction("Index");
        }




        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetCities(System.Guid provinceId)
        {
            try
            {
                var cities =
                 UnitOfWork.CityRepository.GetByProvinceId(provinceId)
                 .Select(x => new
                 {
                     Name = x.Name,
                     Id = x.Id
                 })
                 .ToList()
                 ;

                return Json
                    (data: cities,
                    behavior: System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintNewFactor(int invoiceNumber)
        {
            try
            {
                var factorCement =
                UnitOfWork.FactorCementRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();

                ViewModels.Areas.Administrator.Cement.CementViewModel cementViewModel = new ViewModels.Areas.Administrator.Cement.CementViewModel();
                cementViewModel.Id = factorCement.Id;
                cementViewModel.InvoiceNumber = factorCement.InvoiceNumber;
                cementViewModel.StringInsertDateTime = new Infrastructure.Calander(factorCement.InsertDateTime).Persion();
                cementViewModel.BuyerMobile = factorCement.BuyerMobile;
                cementViewModel.BuyerName = factorCement.User.FullName;
                cementViewModel.BuyerNationalCode = factorCement.User.NationalCode;
                cementViewModel.StringProvince = factorCement.Province.Name;
                cementViewModel.StringCity = factorCement.City.Name;
                cementViewModel.StringProductName = factorCement.ProductName.Name;
                cementViewModel.StringProductType = factorCement.ProductType.Name;
                cementViewModel.StringPackageType = factorCement.PackageType.Name;
                cementViewModel.StringFactoryName = factorCement.FactoryName.Name;
                cementViewModel.StringTonnage = factorCement.Tonnage.Name;
                cementViewModel.AmountPaid = factorCement.AmountPaid;
                cementViewModel.Address = factorCement.Address;
                cementViewModel.RemittanceNumber = factorCement.RemittanceNumber;
                cementViewModel.DestinationAmountPaid = factorCement.DestinationAmountPaid != null ? factorCement.DestinationAmountPaid.Value : 0;
                cementViewModel.MahalTahvil = factorCement.MahalTahvil == "Karkhane" ? "درب کارخانه" : factorCement.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ";
                cementViewModel.ref_id = factorCement.ref_id.ToString();
                cementViewModel.card_pan = factorCement.card_pan;

                var File = new Rotativa.MVC.ViewAsPdf("PrintNewFactor", cementViewModel)
                {
                    FileName = factorCement.InvoiceNumber + ".pdf"
                };
                return File;
            }

            catch (Exception ex)
            {
                return null;
            }
        }


        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintNewFactorWallet(int invoiceNumber)
        {
            try
            {
                var factorCement =
                UnitOfWork.walletFactorRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();

                WalletViewModel factor = new WalletViewModel();
                factor.Id = factorCement.Id;
                factor.InvoiceNumber = factorCement.InvoiceNumber;
                factor.StringInsertDateTime = factorCement.InsertDateTime.ToString();
                factor.BuyerName = factorCement.User.FullName;
                factor.BuyerMobile = factorCement.BuyerMobile;
                factor.AmountPaid = factorCement.Chargeamount.ToString();
                factor.ref_id = factorCement.ref_id.ToString();
                factor.card_pan = factorCement.card_pan;

                var File = new Rotativa.MVC.ViewAsPdf("PrintNewFactorWallet", factor)
                {
                    FileName = factorCement.InvoiceNumber + ".pdf"
                };
                return File;
            }

            catch (Exception ex)
            {
                return null;
            }
        }



        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult PrintDepositNumber(int invoiceNumber)
        {
            Models.Request oRequest = null;
            try
            {
                List<Models.DetailOfFactor> list = null;

                oRequest =
                UnitOfWork.RequestRepository.Get()
                .Where(current => current.InvoiceNumber == invoiceNumber)
                .FirstOrDefault();


                if (oRequest?.HeadOfFactor != null)
                {
                    var headoffactorid =
                        oRequest.HeadOfFactor.Id;

                    var headRow =
                         UnitOfWork.HeadOfFactorRepository.Get()
                         .Where(current => current.Id == headoffactorid)
                         .FirstOrDefault()
                         ;
                    ViewBag.headRow = headRow;


                    list =
                        UnitOfWork.DetailOfFactorRepository.Get(headoffactorid)
                        .ToList();

                    var File = new Rotativa.MVC.ViewAsPdf("PrintFactor", list)
                    {
                        FileName = list.FirstOrDefault().HeadOfFactor.CompanyName + "-" + list.FirstOrDefault().HeadOfFactor.InvoiceNumber + ".pdf"
                    };
                    return File;
                }
                else
                {
                    oRequest =
                        UnitOfWork.RequestRepository.Get()
                        .Where(current => current.InvoiceNumber == invoiceNumber)
                        .FirstOrDefault();

                    //Use ViewAsPdf Class to generate pdf using GeneratePDF.cshtml view
                    var File = new Rotativa.MVC.ViewAsPdf("PrintDepositNumber", oRequest) { FileName = "PrintDepositNumber.pdf" };
                    return File;
                }
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }


        //[HttpPost]
        //public virtual JsonResult AddPayment(PCPOS_Res data, Guid Id)
        //{
        //    var pcPosPaymentResult = new PcPosPaymentResult();

        //    foreach (var item in data.resp_tlv.children)
        //    {
        //        switch (item.Tag)
        //        {
        //            case "TM":
        //                pcPosPaymentResult.PosNumber = item.Value;
        //                break;
        //            case "PN":
        //                pcPosPaymentResult.CartNumber = item.Value;
        //                break;
        //            case "SR":
        //                pcPosPaymentResult.Serial = item.Value + "/" + pcPosPaymentResult.Serial;
        //                break;
        //            case "TR":
        //                pcPosPaymentResult.Serial = item.Value;
        //                pcPosPaymentResult.Bank_TraceNo = item.Value;
        //                break;
        //            case "TI":
        //                pcPosPaymentResult.Date = item.Value;
        //                break;
        //            case "RN":
        //                pcPosPaymentResult.Bank_BankReciptNumber = item.Value;
        //                break;
        //        }
        //    }

        //    var request = UnitOfWork.RequestRepository.GetById(Id);
        //    request.Bank_BankReciptNumber = pcPosPaymentResult.Bank_BankReciptNumber;
        //    request.Bank_ShamsiDate = (pcPosPaymentResult.Date).ToString().Substring(0, 15);
        //    request.Bank_CardHolderAccNumber = pcPosPaymentResult.CartNumber;
        //    request.Bank_CustomerCardNumber = pcPosPaymentResult.CartNumber;
        //    request.Bank_Terminal = pcPosPaymentResult.PosNumber;
        //    request.AmountPaidDate = DateTime.Now;
        //    // request.Bank_BankReciptNumber = pcPosPaymentResult.Serial;
        //    //      request.Bank_TraceNo = Convert.ToInt64(Convert.ToDecimal(pcPosPaymentResult.Serial));
        //    request.Bank_TraceNo = Convert.ToInt64(Convert.ToDecimal(pcPosPaymentResult.Bank_TraceNo));
        //    request.RequestState = (int)Enums.RequestStates.PaymentConfirmation;
        //    UnitOfWork.RequestRepository.Update(request);
        //    UnitOfWork.Save();
        //    try
        //    {
        //        #region Insert New Message
        //        Models.Message oMessage = new Models.Message();
        //        oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
        //        oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
        //        oMessage.MessageText =
        //            Resources.Message.Request.PaymentConfirmation;
        //        oMessage.NewState = (int)Enums.RequestStates.PaymentConfirmation;
        //        oMessage.RequestId = request.Id;
        //        UnitOfWork.MessageRepository.Insert(oMessage);
        //        UnitOfWork.Save();
        //        #endregion
        //    }
        //    catch
        //    {
        //    }


        //    return new JsonResult { Data = new { success = "true" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
    }
}