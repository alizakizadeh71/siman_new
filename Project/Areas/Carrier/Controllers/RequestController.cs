using DAL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Carrier.Controllers
{
    public class RequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Carrier/Request
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            // دراپ‌داون‌های مربوط به کالا و کارخانه (مبدأ)
            var ProductName = UnitOfWork.ProductNameRepository.Get().ToList();
            base.ViewData["ProductName"] = new System.Web.Mvc.SelectList(ProductName, "Id", "Name", null).OrderByDescending(x => x.Text);

            var FactoryName = UnitOfWork.FactoryNameRepository.GetByProductNameId(Guid.Empty).ToList();
            base.ViewData["FactoryName"] = new System.Web.Mvc.SelectList(FactoryName, "Id", "Name", null).OrderBy(x => x.Text);

            // دراپ‌داون‌های آدرس (مقصد)
            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(Guid.Empty).ToList();
            base.ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            // لیست وضعیت‌های مرتبط با باربری (گزینه تایید مالی نشده حذف یا نادیده گرفته می‌شود)
            var requestStateList = new System.Collections.Generic.List<System.Web.Mvc.SelectListItem>
            {
                new System.Web.Mvc.SelectListItem { Value = "2", Text = "آماده بارگیری (تایید مالی شده)" },
                new System.Web.Mvc.SelectListItem { Value = "3", Text = "بارگیری شده" },
                new System.Web.Mvc.SelectListItem { Value = "4", Text = "به مقصد رسیده" },
                new System.Web.Mvc.SelectListItem { Value = "5", Text = "تحویل داده شده" }
            };

            base.ViewData["RequestState"] = new System.Web.Mvc.SelectList(requestStateList, "Value", "Text", null);

            // استفاده از ViewModel (دقت کنید آدرس ViewModel را در صورت نیاز به پوشه Carrier تغییر دهید)
            var viewModel = new ViewModels.Areas.Administrator.Request.IndexViewModel();
            return View(viewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (System.Web.Mvc.JsonResult)Search(null);

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Carrier)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.Request.IndexViewModel viewModel)
        {
            var varRequest = UnitOfWork.FactorCementRepository.Get();

            // 🌟 شرط بسیار مهم: باربری فقط باید درخواست‌هایی را ببیند که تایید مالی شده‌اند (RequestState >= 2)
            // فرض بر این است که 2 به معنای تایید مالی است
            varRequest = varRequest.Where(x => x.RequestState >= 2);

            if (viewModel != null)
            {
                #region Condition (بدون فیلترهای مالی)

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

                if (viewModel.FactoryName != null && viewModel.FactoryName != Guid.Empty)
                    varRequest = varRequest.Where(x => x.FactoryNameId == viewModel.FactoryName);

                if (viewModel.Province != null && viewModel.Province != Guid.Empty)
                    varRequest = varRequest.Where(x => x.ProvinceId == viewModel.Province);

                if (viewModel.City != null && viewModel.City != Guid.Empty)
                    varRequest = varRequest.Where(x => x.CityId == viewModel.City);

                // فیلتر وضعیت باربری
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
                        Description = current.Description,
                        MahalTahvil = current.MahalTahvil == "Karkhane" ? "درب کارخانه" : current.MahalTahvil == "Mahal" ? "مقصد خریدار" : " - ",
                        StringInsertDateTime = current.InsertDateTime != null ? new Infrastructure.Calander(current.InsertDateTime).Persion() : "",

                        // فیلدهای مالی (مثل AmountPaid و FinalApprove) حذف شدند چون باربری نیازی به آن‌ها ندارد
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
        public ActionResult LoadCargo(Guid id) // اگر Id شما از نوع int است، اینجا را تغییر دهید
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
                // ۱. واکشی درخواست مورد نظر از دیتابیس
                var request = UnitOfWork.FactorCementRepository.GetById(id);

                if (request == null)
                {
                    return Json(new { Success = false, Message = "درخواست مورد نظر یافت نشد." });
                }

                // ۲. ثبت اطلاعات راننده (دقت کنید که این فیلدها باید در کلاس/مدل FactorCement وجود داشته باشند)
                request.DriverName = driverName;
                request.DriverMobile = driverMobile;
                request.DriverLicensePlate = licensePlate;

                // تغییر وضعیت به بارگیری شده (با توجه به لیست وضعیت‌های شما کد ۳ در نظر گرفته شده است)
                request.RequestState = 3;

                // بروزرسانی و ذخیره تغییرات در دیتابیس
                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();

                // ۳. ارسال پیامک به موبایل خریدار
                if (!string.IsNullOrEmpty(request.BuyerMobile))
                {
                    // فراخوانی متد پیامک که قبلاً ساختید
                    Utilities.SMS.SmsUtility.SendLoadedNotification(request.BuyerMobile, driverName, driverMobile, licensePlate);
                }

                // بازگرداندن نتیجه موفقیت‌آمیز برای Ajax
                return Json(new { Success = true, Message = "اطلاعات با موفقیت ثبت و پیامک ارسال شد." });
            }
            catch (Exception ex)
            {
                // در صورت بروز هرگونه خطا، پیام خطا به کاربر برگردانده می‌شود
                return Json(new { Success = false, Message = "خطایی سمت سرور رخ داد: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SetAsArrived(Guid id)
        {
            try
            {
                var request = UnitOfWork.FactorCementRepository.GetById(id);
                if (request == null)
                    return Json(new { success = false, message = "درخواست یافت نشد." });

                // تغییر وضعیت به 4 (رسیده به مقصد)
                // اگر از Enum استفاده می‌کنید: request.RequestState = RequestState.ArrivedAtDestination;
                request.RequestState = 4;

                UnitOfWork.FactorCementRepository.Update(request);
                UnitOfWork.Save();
                return Json(new { success = true, message = "وضعیت بار به 'رسیده به مقصد' تغییر یافت." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا در انجام عملیات: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SetAsDelivered(Guid id)
        {
            try
            {
                var request = UnitOfWork.FactorCementRepository.GetById(id);
                if (request == null)
                    return Json(new { success = false, message = "درخواست یافت نشد." });

                // تغییر وضعیت به 5 (تحویل داده شده)
                // اگر از Enum استفاده می‌کنید: request.RequestState = RequestState.Delivered;
                request.RequestState = 5;

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