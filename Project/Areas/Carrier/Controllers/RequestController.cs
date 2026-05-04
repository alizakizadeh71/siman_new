using DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Carrier.Controllers
{
    public partial class RequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Carrier/Request
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", null).OrderByDescending(x => x.Text);

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(Guid.Empty).ToList();
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", null).OrderBy(x => x.Text);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(Guid.Empty).ToList();
            base.ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            // لیست وضعیت‌های مرتبط با باربری
            var requestStateList = new System.Collections.Generic.List<System.Web.Mvc.SelectListItem>
            {
                new System.Web.Mvc.SelectListItem { Value = "1", Text = "تایید مالی نشده" },
                new System.Web.Mvc.SelectListItem { Value = "2", Text = "تایید مالی شده" },
                new System.Web.Mvc.SelectListItem { Value = "3", Text = "در انتظار بارگیری" },
                new System.Web.Mvc.SelectListItem { Value = "4", Text = "تحویل داده شده" }
            };

            base.ViewData["RequestState"] = new System.Web.Mvc.SelectList(requestStateList, "Value", "Text", null);

            var viewModel = new ViewModels.Areas.Administrator.Request.IndexViewModel();
            return View(viewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (System.Web.Mvc.JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Request.IndexViewModel viewModel, int gridTab = 0)
        {
            var currentUserId = Infrastructure.Sessions.AuthenticatedUser.Id;

            var varRequest = UnitOfWork.FactorCementRepository.Get()
                .Where(c => c.CarrierId == currentUserId);

            // 🌟 فیلتر اصلی بر اساس تب (جدول) درخواستی
            if (gridTab == 1)
            {
                // جدول اول: آماده تخصیص راننده (فقط وضعیت 2)
                varRequest = varRequest.Where(x => x.RequestState == 2);
            }
            else if (gridTab == 2)
            {
                // جدول دوم: در جریان ارسال (فقط وضعیت 3)
                varRequest = varRequest.Where(x => x.RequestState == 3);
            }
            else if (gridTab == 3)
            {
                // جدول سوم: بایگانی (فقط وضعیت 4)
                varRequest = varRequest.Where(x => x.RequestState == 4);
            }
            else
            {
                // پیش‌فرض: تمام مواردی که تایید مالی شده‌اند به بعد
                varRequest = varRequest.Where(x => x.RequestState >= 2);
            }

            if (viewModel != null)
            {
                #region Condition 
                if (!string.IsNullOrEmpty(viewModel.BuyerMobile))
                {
                    viewModel.BuyerMobile = Utilities.Text.Utility.FixText(viewModel.BuyerMobile);
                    varRequest = varRequest.Where(x => x.BuyerMobile == viewModel.BuyerMobile);
                }

                if (viewModel.InvoiceNumber != null && viewModel.InvoiceNumber != 0)
                    varRequest = varRequest.Where(x => x.InvoiceNumber == viewModel.InvoiceNumber);

                if (viewModel.ProductName != null && viewModel.ProductName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProductNameId == viewModel.ProductName);

                if (viewModel.FactoryName != null && viewModel.FactoryName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.FactoryNameId == viewModel.FactoryName);

                if (viewModel.Province != null && viewModel.Province != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);

                if (viewModel.City != null && viewModel.City != Guid.Empty)
                    varRequest = varRequest.Where(x => x.CityId == viewModel.City);

                if (viewModel.RequestState != null && viewModel.RequestState > 0)
                    varRequest = varRequest.Where(x => x.RequestState == viewModel.RequestState);

                if (viewModel.StartDate != null)
                    varRequest = varRequest.Where(current => current.InsertDateTime >= viewModel.StartDate);

                if (viewModel.EndDate != null)
                {
                    var EndDate2 = viewModel.EndDate.Value.AddDays(1);
                    varRequest = varRequest.Where(current => current.InsertDateTime < EndDate2);
                }
                #endregion
            }

            try
            {
                var mappedRequests = varRequest
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
                        BuyerMobile = current.BuyerMobile,
                        BuyerName = current.User.FullName,
                        Description = current.Description,
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

        [HttpGet]
        public virtual ActionResult LoadCargo(Guid id)
        {
            ViewBag.RequestId = id;
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.JsonResult SetAsLoaded(Guid id, string driverName, string driverMobile, string licensePlate)
        {
            try
            {
                var request = UnitOfWork.FactorCementRepository.GetById(id);
                if (request == null) return Json(new { Success = false, Message = "درخواست مورد نظر یافت نشد." });

                request.DriverName = driverName;
                request.DriverMobile = driverMobile;
                request.DriverLicensePlate = licensePlate;
                request.RequestState = 3; // تغییر به در انتظار بارگیری (در جریان ارسال)

                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();

                // بررسی وجود شماره موبایل برای ارسال پیامک
                if (!string.IsNullOrEmpty(request.BuyerMobile))
                {
                    string factorNumber = request.InvoiceNumber.ToString(); // شماره فاکتور
                    string fullProductName = $"{request.Tonnagedouble.ToString().Replace('.', '/')} تن {request.ProductName?.Name} {request.FactoryName?.Name} {request.PackageType?.Name}".Trim();

                    // ارسال اطلاعات کامل به متد پیامک
                    Utilities.SMS.SmsUtility.SendLoadedNotification(
                        request.BuyerMobile,
                        factorNumber,
                        fullProductName,
                        driverName,
                        driverMobile,
                        licensePlate
                    );
                }

                return Json(new { Success = true, Message = "اطلاعات با موفقیت ثبت و پیامک ارسال شد." });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "خطایی سمت سرور رخ داد: " + ex.Message });
            }
        }


        // 🌟 متد SetAsArrived به دلیل تکراری بودن حذف شد. 🌟

        [HttpPost]
        public virtual ActionResult SetAsDelivered(Guid id)
        {
            try
            {
                var request = UnitOfWork.FactorCementRepository.GetById(id);
                if (request == null)
                    return Json(new { success = false, message = "درخواست یافت نشد." });

                // تغییر وضعیت به 4 (تحویل داده شده / بایگانی)
                request.RequestState = 4;

                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();

                return Json(new { success = true, message = "وضعیت بار به 'تحویل داده شده' تغییر یافت." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا در انجام عملیات: " + ex.Message });
            }
        }
    }
}
