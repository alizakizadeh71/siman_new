using System;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.Village;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class VillageController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Administrator/Village
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            ViewModels.Areas.Administrator.Village.IndexViewModel villageViewModel = new ViewModels.Areas.Administrator.Village.IndexViewModel();
            Viewdata(villageViewModel);
            return View(villageViewModel);
        }

        private void Viewdata(IndexViewModel villageViewModel)
        {
            var village = UnitOfWork.VillageRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["village"] = new System.Web.Mvc.SelectList(village, "Id", "Title", villageViewModel.Name).OrderByDescending(x => x.Text);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(villageViewModel.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", villageViewModel.City);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(IndexViewModel villageViewModel)
        {
            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.Get().ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
            bool Search = false;
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();
            var varRequest =
                UnitOfWork.VillageRepository.Get()
                .Where(x => x.IsActived && !x.IsDeleted);

            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                    .ToList()
                    .Select(current =>
                        new ViewModels.Areas.Administrator.Village.IndexViewModel()
                        {
                            Id = current.Id,
                            stringCity = current.City.Name,
                            Name = current.Name,
                            Code = current.Code,
                            StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
                        })
                        .AsQueryable();


                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.Village.IndexViewModel>(ViewModelsvarBanks);

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            ViewBag.PageMessages = null;
            ViewModels.Areas.Administrator.Village.IndexViewModel villageViewModel = new ViewModels.Areas.Administrator.Village.IndexViewModel();
            Viewdata(villageViewModel);
            return View(villageViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(IndexViewModel villageViewModel)
        {
            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", villageViewModel.Province);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(villageViewModel.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", villageViewModel.City);
            ViewBag.PageMessages = null;

            var ofindsubheadline =
                 UnitOfWork.VillageRepository.Get()
                 .Where(x => x.IsActived && !x.IsDeleted)
                 .Where(current => current.ProvinceId == villageViewModel.Province)
                 .Where(current => current.Cityid == villageViewModel.City)
                 .Where(cuttent => (cuttent.Name == villageViewModel.Name) || (cuttent.Code == villageViewModel.Code))
                 .FirstOrDefault();

            if (ofindsubheadline != null)
                ViewBag.PageMessages = "خدمات مشابه با همین ویژگی ها در سیستم ثبت شده است.";
            else
            {
                Models.village village = new Models.village();
                village.ProvinceId = villageViewModel.Province;
                village.Cityid = villageViewModel.City;
                village.Name = villageViewModel.Name;
                village.Code = villageViewModel.Code;

                UnitOfWork.VillageRepository.Insertdata(village);
                UnitOfWork.Save();

                ViewBag.PageMessages = "خدمات درخواستی شما با موفقیت ثبت گردید  ";
            }
            return View(villageViewModel);
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

            var oAccountNumberManage
                = UnitOfWork.VillageRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.Village.IndexViewModel
                {
                    stringCity = current.City.Name,
                    Name = current.Name,
                    Code = current.Code,
                    StringInsertDateTime = new Infrastructure.Calander(current.InsertDateTime).Persion(),
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        public virtual System.Web.Mvc.ActionResult Delete(IndexViewModel villageViewModel)
        {
            try
            {
                var varAccountNumberManages =
                    UnitOfWork.VillageRepository.Get()
                    .Where(current => current.Id == villageViewModel.Id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varAccountNumberManages != null)
                {
                    varAccountNumberManages.IsDeleted = true;
                    varAccountNumberManages.IsActived = false;
                    varAccountNumberManages.UpdateDateTime = DateTime.Now;
                    UnitOfWork.VillageRepository.Update(varAccountNumberManages);
                    UnitOfWork.Save();
                }
                return (RedirectToAction(MVC.Administrator.Village.Index()));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

    }
}