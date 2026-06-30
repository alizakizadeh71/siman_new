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
                new System.Web.Mvc.SelectListItem { Value = "2", Text = "تایید مالی شده" },
                new System.Web.Mvc.SelectListItem { Value = "3", Text = "در انتظار بارگیری" },
                new System.Web.Mvc.SelectListItem { Value = "4", Text = "تحویل داده شده" }
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
            var varRequest = UnitOfWork.FactorCementRepository.Get().Where(x => x.FinalApprove);

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
                    // مرتب‌سازی بر اساس آخرین شماره فاکتور (نزولی)
                    .OrderByDescending(current => current.InvoiceNumber)
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
                        BuyerName =
    !string.IsNullOrEmpty(current.ManualBuyerName)
        ? current.ManualBuyerName
        : (current.User != null
            && !string.IsNullOrEmpty(current.User.FullName)
            && current.User.FullName != "کاربر مهمان"
                ? current.User.FullName
                : (!string.IsNullOrEmpty(current.BuyerFullName)
                    ? current.BuyerFullName
                    : "کاربر مهمان")),

                BuyerMobile =
                            current.User != null
                            && !string.IsNullOrEmpty(current.User.BuyerMobile)
                                ? current.User.BuyerMobile
                                : current.BuyerMobile,
                        Description = current.Description,
                        DriverName = current.DriverName,
                        DriverMobile = current.DriverMobile,
                        DriverLicensePlate = current.DriverLicensePlate,
                        stringFinalApprove = current.FinalApprove == true ? "نهایی شده" : "نهایی نشده",
                        AmountPaid = current.MahalTahvil == "Karkhane" ? current.AmountPaid : current.MahalTahvil == "Mahal" ? (current.DestinationAmountPaid ?? 0) : 0,
                        MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                        CarrierName = current.CarrierUser != null ? current.CarrierUser.FullName : "بدون باربری",
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
            var carrierRole = UnitOfWork.RoleRepository.Get()
                .FirstOrDefault(r => r.Code == (int)Enums.Roles.Carrier);

            if (carrierRole != null)
            {
                // ۲. حالا کاربرانی که این آی‌دی را دارند پیدا میکنیم
                var carrierUsers = UnitOfWork.UserRepository.Get()
                    .Where(u => u.RoleId == carrierRole.Id)
                    .Select(u => new
                    {
                        Id = u.Id,
                        FullName = u.FullName
                    }).ToList();

                ViewBag.CarrierList = new System.Web.Mvc.SelectList(carrierUsers, "Id", "FullName");
            }
            else
            {
                ViewBag.CarrierList = null;
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

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DriverInfo(Guid id) // اگر Id از نوع int است، نوع پارامتر را تغییر دهید
        {
            // ۱. دریافت اطلاعات از دیتابیس (نام کانتکست دیتابیس خود را قرار دهید)
            var request = UnitOfWork.FactorCementRepository.GetById(id);

            if (request == null)
            {
                return HttpNotFound("درخواست مورد نظر یافت نشد.");
            }

            // ۲. پاس دادن اطلاعات به View 
            // نکته: نام ویژگی‌ها (مثل DriverName) را با نام‌های کلاس دیتابیس خود جایگزین کنید
            ViewBag.DriverName = request.DriverName;
            ViewBag.DriverPhone = request.DriverMobile;
            ViewBag.LicensePlate = request.DriverLicensePlate;

            return View();
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

        [HttpPost]
        public virtual System.Web.Mvc.ActionResult ApproveFinancial(Guid id, string description, Guid CarrierId)
        {
            // 1. بررسی خالی نبودن شناسه باربری در سمت سرور
            if (CarrierId == Guid.Empty)
            {
                TempData["ErrorMessage"] = "خطا: انتخاب باربری الزامی است!";
                // کاربر را به همان صفحه قبلی برمی‌گردانیم
                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                return RedirectToAction("Index");
            }

            var request = UnitOfWork.FactorCementRepository.GetById(id);
            if (request != null)
            {
                // تغییر وضعیت به 2 (تایید مالی شده) 
                request.RequestState = 2;
                request.Description = description;

                // ذخیره شناسه باربری انتخاب شده
                request.CarrierId = CarrierId;

                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();

                TempData["SuccessMessage"] = "تایید مالی با موفقیت انجام شد و به باربری ارجاع داده شد.";
            }

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