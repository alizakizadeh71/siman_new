using Models;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class SiteSettingController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/SiteSetting
        [HttpGet]
        public virtual ActionResult Index()
        {
            var setting = UnitOfWork.SiteSettingRepository.GetSetting();

            if (setting == null)
            {
                setting = new SiteSetting { IsPurchaseEnabled = true };
                UnitOfWork.SiteSettingRepository.Insert(setting);
                UnitOfWork.Save();
            }

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Index(SiteSetting model)
        {
            if (ModelState.IsValid)
            {
                var setting = UnitOfWork.SiteSettingRepository.GetSetting();

                if (setting != null)
                {
                    // آپدیت کردن فیلد مورد نظر
                    setting.IsPurchaseEnabled = model.IsPurchaseEnabled;

                    UnitOfWork.SiteSettingRepository.Update(setting);
                    UnitOfWork.Save();

                    ViewBag.SuccessMessage = "تنظیمات سایت با موفقیت بروزرسانی شد.";
                }
            }

            return View(model);
        }
    }
}