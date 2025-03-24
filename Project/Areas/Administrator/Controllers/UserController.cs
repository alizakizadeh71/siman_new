using ClosedXML.Excel;
using OPS.GetPostinfoServices;
using OPS.PersonInfoServices;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Models;
using OPS.Controllers;
using Utilities.PersianDate;
using ViewModels.Areas.Administrator.User;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class UserController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Index()
        {
            var varRoles
                = UnitOfWork.RoleRepository.Get()
                .OrderBy(current => current.Name).ToList();

            ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
            ViewModels.Areas.Administrator.User.SearchViewModel searchViewModel = new ViewModels.Areas.Administrator.User.SearchViewModel();
            ViewBag.PageMessages = null;
            return View(searchViewModel);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.User.SearchViewModel viewModel)
        {
            System.Globalization.PersianCalendar opersian = new System.Globalization.PersianCalendar();

            var varUser =
                UnitOfWork.UserRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                ;

            #region Condition
            viewModel.FullName = Utilities.Text.Utility.FixText(viewModel.FullName);
            viewModel.UserName = Utilities.Text.Utility.FixText(viewModel.UserName);

            if (viewModel.FullName != string.Empty)
            {
                varUser =
                    varUser
                    .Where(current => current.FullName.Contains(viewModel.FullName))
                    ;
            }

            if (viewModel.UserName != string.Empty)
            {
                varUser =
                    varUser
                    .Where(current => current.UserName.Contains(viewModel.UserName))
                    ;
            }

            if (viewModel.Province != null && viewModel.Province != new Guid())
            {
                varUser = varUser.Where(current => current.ProvinceId == viewModel.Province);
            }

            if (viewModel.City != null && viewModel.City != new Guid())
            {
                varUser = varUser.Where(current => current.CityId == viewModel.City);
            }

            if (viewModel.Role != null && viewModel.Role != new Guid())
            {
                varUser = varUser.Where(current => current.RoleId == viewModel.Role);
            }
            #endregion

            var varRoles
                = UnitOfWork.RoleRepository.Get()
                .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                .OrderBy(current => current.Name).ToList();
            ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            var ViewModelsUser
                 = varUser.OrderByDescending(current => current.Role.Code)
                 .OrderBy(current => current.Province != null ? current.Province.Name : current.UserName)
                 .ThenBy(current => current.UserName)
                 .ToList()
                 .Select(current =>
                     new ViewModels.Areas.Administrator.User.IndexViewModel()
                     {
                         Id = current.Id,
                         FullName = current.FullName,
                         UserName = current.UserName,
                         Role = current.Role.Name,
                         Province = current.Province != null ? current.Province.Name : "[نا مشخص]",
                         City = current.City != null ? current.City.Name : "[نا مشخص]",
                         IsActive = current.IsActived,
                         Address = current.Address,
                         BuyerMobile = current.BuyerMobile,
                         InitialCredit = current.InitialCredit,
                         isSendSmS = current.isSendSms,
                         //IsApprovallicense = current.IsApprovallicense,
                         Authenticate = current.Authenticate,
                     })
                     .ToList()
                     .Select(current =>
                     new ViewModels.Areas.Administrator.User.IndexViewModel()
                     {
                         Id = current.Id,
                         FullName = current.FullName,
                         UserName = current.UserName,
                         Role = current.Role,
                         Province = current.Province,
                         City = current.City,
                         IsActive = current.IsActive,
                         BuyerMobile = current.BuyerMobile,
                         Address = current.Address,
                         //IsApprovallicense = current.IsApprovallicense,
                         Authenticate = current.Authenticate,
                     })
                     .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.User.IndexViewModel>(ViewModelsUser);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetUsers()
        {
            try
            {
                var varUser =
                    UnitOfWork.UserRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                    ;

                var varRoles
                    = UnitOfWork.RoleRepository.Get()
                    .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                    .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

                var ViewModelsUser
                     = varUser.OrderByDescending(current => current.Role.Code)
                     .OrderBy(current => current.Province != null ? current.Province.Name : current.UserName)
                     //.ThenBy(current => current.City != null ? current.City.Name : current.UserName)
                     .ThenBy(current => current.UserName)
                     .ToList()
                     .Select(current =>
                         new ViewModels.Areas.Administrator.User.IndexViewModel()
                         {
                             Id = current.Id,
                             FullName = current.FullName,
                             UserName = current.UserName,
                             Role = current.Role.Name,
                             Province = current.Province != null ? current.Province.Name : "[نا مشخص]",
                             City = current.City != null ? current.City.Name : "[نا مشخص]",
                             IsActive = current.IsActived,
                             creditAmount = current.creditAmount.ToString("N0"),
                             BuyerMobile = current.BuyerMobile,
                             Address = current.Address,
                             InitialCredit = current.InitialCredit,
                             isSendSmS = current.isSendSms,
                             //IsApprovallicense = current.IsApprovallicense,
                             Authenticate = current.Authenticate,
                         })
                         .ToList()
                         .Select(current =>
                         new ViewModels.Areas.Administrator.User.IndexViewModel()
                         {
                             Id = current.Id,
                             FullName = current.FullName,
                             UserName = current.UserName,
                             Role = current.Role,
                             Province = current.Province,
                             City = current.City,
                             IsActive = current.IsActive,
                             creditAmount = current.creditAmount,
                             BuyerMobile = current.BuyerMobile,
                             Address = current.Address,
                             InitialCredit = current.InitialCredit,
                             isSendSmS = current.isSendSmS,
                             //IsApprovallicense = current.IsApprovallicense,
                             Authenticate = current.Authenticate,
                         })
                         .AsQueryable();

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                    .ParseGridData<ViewModels.Areas.Administrator.User.IndexViewModel>(ViewModelsUser);
                ViewBag.PageMessages = null;
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

            var varRoles
                = UnitOfWork.RoleRepository.Get()
                .OrderBy(current => current.Name).ToList();
            ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            ViewModels.Areas.Administrator.User.CreateViewModel createViewModel = new ViewModels.Areas.Administrator.User.CreateViewModel();
            return View(createViewModel);
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

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.User.CreateViewModel user)
        {
            ViewBag.PageMessages = null;

            Models.User oFindedUser = new Models.User();

            oFindedUser =
                UnitOfWork.UserRepository
                .Get()
                .Where(current => current.UserName == user.UserName)
                .FirstOrDefault()
                ;

            if (oFindedUser != null)
            {
                var varRoles
                    = UnitOfWork.RoleRepository.Get()
                    .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                    .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
                ViewBag.PageMessages += "کاربر مشابه با همین ویژگی ها در سیستم ثبت شده است.";
                ViewBag.PageMessages += "<br/>";
                return View(user);
            }
            if (user.FullName == string.Empty || user.FullName == null)
            {
                var varRoles
                = UnitOfWork.RoleRepository.Get()
                .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
                ViewBag.PageMessages += "احراز هویت انجام نشده است";
                ViewBag.PageMessages += "<br/>";
                return View(user);
            }
            if (user.Password == string.Empty || user.Password == null)
            {
                var varRoles
                = UnitOfWork.RoleRepository.Get()
                .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
                ViewBag.PageMessages += "گذرواژه وارد نشده است";
                ViewBag.PageMessages += "<br/>";
                return View(user);
            }

            if (ModelState.IsValid)
            {
                Models.User oUser = new Models.User();
                {
                    oUser.FullName = user.FullName;
                    oUser.IsActived = true;
                    oUser.IsDeleted = false;
                    oUser.IsSystem = false;
                    oUser.IsVerified = true;
                    oUser.IsApprovallicense = user.IsApprovallicense;
                    oUser.ProvinceId = user.Province;
                    oUser.CityId = user.City;
                    oUser.LoginCount = 0;
                    oUser.InsertDateTime = DateTime.Now;
                    oUser.UpdateDateTime = DateTime.Now;
                    oUser.Password = Utilities.Security.Hashing.GetSha1(user.Password);
                    oUser.RoleId = user.Role;
                    oUser.UserName = user.UserName;
                    oUser.NationalCode = user.NationalCode;
                    oUser.creditAmount = user.creditAmount;
                    oUser.BirthDay = user.BirthDay;
                    oUser.Authenticate = true;
                    oUser.InitialCredit = user.InitialCredit;
                    oUser.isSendSms = user.isSendSmS;
                    UnitOfWork.UserRepository.Insert(oUser);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "حساب درخواستی شما با موفقیت ثبت گردید  ";
                }

                var varRoles
                    = UnitOfWork.RoleRepository.Get()
                    .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                    .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", oUser.RoleId);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", oUser.ProvinceId);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
            }
            else
            {
                var varRoles
                = UnitOfWork.RoleRepository.Get()
                .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", null);
                ViewBag.PageMessages += "خطا دیتا - دیتا های وارد شده را دوباره بررسی نمایید";
                ViewBag.PageMessages += "<br/>";
                return View(user);
            }

            return View(user);
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

            ViewModels.Areas.Administrator.User.DisplayViewModel oUser
                = UnitOfWork.UserRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.User.DisplayViewModel()
                {
                    Id = current.Id,
                    FullName = current.FullName,
                    Province = current.Province != null ? current.Province.Name : "[مشخص نشده]",
                    City = current.City != null ? current.City.Name : "[مشخص نشده]",
                    Role = current.Role.Name,
                    UserName = current.UserName,
                    creditAmount = current.creditAmount,
                    Active = current.IsActived == true ? "فعال" : "غیر فعال",
                    Approvallicense = current.IsApprovallicense == true ? "دارد" : "ندارد",
                    Authenticate = current.Authenticate == true ? "احراز هویت شده" : "احراز هویت نشده",
                    NationalCode = current.NationalCode,
                    BirthDay = current.BirthDay,
                    Address = current.Address,
                    Image = current.Image,
                    isSendSmS = current.isSendSms == true ? "فعال" : "غیر فعال",
                    InitialCredit = current.InitialCredit

                })
                .FirstOrDefault()
                ;

            if (oUser == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oUser));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            ViewModels.Areas.Administrator.User.EditViewModel oUser
                = UnitOfWork.UserRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.User.EditViewModel()
                {
                    Id = current.Id,
                    FullName = current.FullName,
                    Province = current.ProvinceId.Value,
                    City = current.CityId != null ? current.CityId.Value : new Guid(),
                    Role = current.RoleId,
                    creditAmount = current.creditAmount,
                    UserName = current.UserName,
                    Address = current.Address,
                    BuyerMobile = current.BuyerMobile,
                    IsActive = current.IsActived,
                    IsApprovallicense = current.IsApprovallicense,
                    Authenticate = current.Authenticate,
                    isSendSmS = current.isSendSms,
                    Image = current.Image,
                    
                    InitialCredit = current.InitialCredit
                })
                .FirstOrDefault()
                ;

            #region DropDownList

            var varRoles
                = UnitOfWork.RoleRepository.Get()
                //.Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                .OrderBy(current => current.Name).ToList();
            ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", oUser.Role);

            var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", oUser.Province);


            var varCities1 =
                 UnitOfWork.CityRepository.GetByProvinceId(oUser.Province)
                 .Select(x => new
                 {
                     Name = x.Name,
                     Id = x.Id
                 })
                 .ToList()
                 ;


            //var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities1, "Id", "Name", oUser.City);

            #endregion

            ViewBag.PageMessages = null;

            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            if (oUser == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oUser));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.User.EditViewModel user)
        {
            ViewBag.PageMessages = null;

            try
            {
                Models.User oFindedUser;

                var OlderAccount =
                    UnitOfWork.UserRepository
                    .Get()
                    .Where(current => current.Id == user.Id)
                    .FirstOrDefault()
                    ;

                var oFindedOther =
                    UnitOfWork.UserRepository
                    .Get()
                    .Where(current => current.UserName == user.UserName)
                    .Where(current => current.Id != user.Id)
                    .FirstOrDefault()
                    ;

                #region DropDownList

                var varRoles
                    = UnitOfWork.RoleRepository.Get()
                    .Where(current => current.Code < Infrastructure.Sessions.AuthenticatedUser.RoleCode)
                    .OrderBy(current => current.Name).ToList();
                ViewData["Role"] = new System.Web.Mvc.SelectList(varRoles, "Id", "Name", user.Role);

                var varProvinces = UnitOfWork.ProvinceRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User).ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", user.Province);


                var varCities1 =
                     UnitOfWork.CityRepository.GetByProvinceId(user.Province)
                     .Select(x => new
                     {
                         Name = x.Name,
                         Id = x.Id
                     })
                     .ToList()
                     ;


                //var varCities = UnitOfWork.CityRepository.GetByProvinceId(new Guid()).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities1, "Id", "Name", user.City);

                #endregion

                if (oFindedOther != null)
                {
                    ViewBag.PageMessages += "حساب با این مشخصات در سیستم ثبت شده است.";
                    ViewBag.PageMessages += "<br/>";
                    return View();
                }

                else if (ModelState.IsValid)
                {
                    OlderAccount.ProvinceId = user.Province;
                    OlderAccount.CityId = user.City;
                    OlderAccount.RoleId = user.Role;
                    OlderAccount.FullName = user.FullName;
                    OlderAccount.UserName = user.UserName;
                    OlderAccount.IsActived = user.IsActive;
                    OlderAccount.IsApprovallicense = user.IsApprovallicense;
                    OlderAccount.Address = user.Address;
                    OlderAccount.BuyerMobile = user.BuyerMobile;
                    OlderAccount.creditAmount = user.creditAmount;
                    OlderAccount.InitialCredit = user.InitialCredit;
                    OlderAccount.isSendSms = user.isSendSmS;
                    if (user.Authenticate == false)
                    {
                        OlderAccount.Authenticate = user.Authenticate;
                    }

                    UnitOfWork.UserRepository.Update(OlderAccount);
                    UnitOfWork.Save();

                    ViewBag.PageMessages += "کاربر درخواستی شما با موفقیت ویرایش گردید  ";
                }

                return View(user);
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

            ViewModels.Areas.Administrator.User.DisplayViewModel oUser
                = UnitOfWork.UserRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.User.DisplayViewModel()
                {
                    Id = current.Id,
                    Role = current.Role.Name,
                    Province = current.Province != null ? current.Province.Name : "[مشخص نشده]",
                    City = current.City != null ? current.City.Name : "[مشخص نشده]",
                    UserName = current.UserName,
                    FullName = current.FullName,
                    IsApprovallicense = current.IsApprovallicense,
                    Authenticate = current.Authenticate == true ? "احراز هویت شده" : "احراز هویت نشده",
                })
                .FirstOrDefault()
                ;

            if (oUser == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oUser));
        }


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varUser =
                    UnitOfWork.UserRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                ViewBag.PageMessages = string.Empty;

                if (varUser != null)
                {
                    //UnitOfWork.UserRepository.Delete(varUser);
                    //UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.User.Index()));
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult ChangePassword(System.Guid id)
        {
            ViewModels.Areas.Administrator.User.ChangePasswordViewModel oUser
                = new ViewModels.Areas.Administrator.User.ChangePasswordViewModel();

            oUser.Id = id;
            return View(oUser);
        }


        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.ActionResult ChangePassword
            ([System.Web.Mvc.Bind(Include = "Id,NewPassword,ConfirmNewPassword")]
            ViewModels.Areas.Administrator.User.ChangePasswordViewModel viewModel)
        {
            Models.User oUser
                = UnitOfWork.UserRepository.GetById(viewModel.Id);

            if (User == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            //string strHashOfCurrentPassword = Utilities.Security.Hashing.GetSha1(viewModel.NewPassword);

            //if (string.Compare(oUser.Password, strHashOfCurrentPassword, ignoreCase: true) != 0)
            //{
            //    ModelState.AddModelError("CurrentPassword", Resources.Message.User.Error008);
            //    return (View());
            //}

            string strHashOfNewPassword = Utilities.Security.Hashing.GetSha1(viewModel.NewPassword);
            oUser.Password = strHashOfNewPassword;

            UnitOfWork.Save();

            PageMessages.Add(new Infrastructure.PageMessage
                (Enums.PageMessageTypes.Information, Resources.Message.User.Information005));

            return (View(viewModel));
        }


        [System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Programmer)]
        [Infrastructure.SyncPermission(isPublic: false)]
        public virtual System.Web.Mvc.ActionResult AccessManagement()
        {
            try
            {
                SyncAreasControllersActionsWithSourceCode wewe = new SyncAreasControllersActionsWithSourceCode();
                ViewBag.Message = "بروزرسانی با موفقیت انجام شد";
                return View();
            }

            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }


        public virtual ActionResult CheckPersonValidation(string NationalCode, string birthDate)
        {
            try
            {
                try
                {
                    var Isuser = Infrastructure.Sessions.AuthenticatedUser.User.Id;
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, CodeMessage = "ابتدا وارد سیستم شوید" }, JsonRequestBehavior.AllowGet);
                }

                var OlderAccount =
                    UnitOfWork.UserRepository
                    .Get()
                    .Where(current => current.Id == Infrastructure.Sessions.AuthenticatedUser.User.Id)
                    .FirstOrDefault()
                    ;
                if (OlderAccount == null)
                {
                    return Json(new { success = false, CodeMessage = "نام کاربری در سیستم یافت نشد" }, JsonRequestBehavior.AllowGet);
                }

                NationalCode = changePersianNumbersToEnglish(NationalCode);
                birthDate = changePersianNumbersToEnglish(birthDate);

                if (NationalCode == string.Empty || birthDate == string.Empty)
                {
                    return Json(new { success = false, CodeMessage = "کد ملی و تاریخ تولد را وارد نمایید" }, JsonRequestBehavior.AllowGet);
                }

                GetingPersonByNationalIdAndBirthDateSoapClient person = new GetingPersonByNationalIdAndBirthDateSoapClient();

                var personInfo = person.GetPersonInfo("payDam", "nRm491HdB", NationalCode, birthDate);
                if (personInfo.Err == 0)
                {
                    personInfo.firstName = personInfo.firstName + " " + personInfo.lastName;
                    return Json(new { success = true, personInfo = personInfo }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, CodeMessage = personInfo.Msg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, CodeMessage = "خطا در ارتباط با سرویس دریافت اطلاعات اشخاص - لطفا دقایقی بعد دوباره تلاش نمایید" }, JsonRequestBehavior.AllowGet);
            }

        }


        public virtual ActionResult CheckPostalCodeValidation(string PostalCode)
        {
            try
            {
                PostalCode = changePersianNumbersToEnglish(PostalCode);
                if (PostalCode == string.Empty)
                {
                    return Json(new { success = false, CodeMessage = "کد پستی را وارد نمایید" }, JsonRequestBehavior.AllowGet);
                }

                postServicesSoapClient postServicesSoapClient = new postServicesSoapClient();
                var postInfo = postServicesSoapClient.GetAddressByPostalCode("payDam", "nRm491HdB", PostalCode);
                //personInfo.Zone = personInfo.Province + " " + personInfo.lastName;
                //var aaa = postInfo.Province + " - " + postInfo.TownShip + " - " + postInfo.SubLocality
                //     + " - " + postInfo.Street + " - " + postInfo.Street2 + " - طبقه " + postInfo.Floor
                //      + " - پلاک " + postInfo.HouseNumber;
                if (postInfo.ErrorCode == 0)
                {
                    return Json(new { success = true, postInfo }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, CodeMessage = postInfo.ErrorMessage }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, CodeMessage = "خطا در ارتباط با سرویس دریافت اطلاعات کد پستی - لطفا دقایقی بعد دوباره تلاش نمایید" }, JsonRequestBehavior.AllowGet);
            }

        }
        private string changePersianNumbersToEnglish(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: true, role: Enums.Roles.None)]
        public virtual ActionResult Authenticate(ViewModels.Areas.Administrator.User.AuthenticateViewModel authenticateViewModel)
        {
            var OlderAccount =
                    UnitOfWork.UserRepository
                    .Get()
                    .Where(current => current.Id == Infrastructure.Sessions.AuthenticatedUser.User.Id)
                    .FirstOrDefault()
                    ;

            OlderAccount.NationalCode = authenticateViewModel.NationalCode2;
            OlderAccount.BirthDay = authenticateViewModel.BirthDay2;
            OlderAccount.IdentityCertificateSerial = authenticateViewModel.IdentityCertificateSerial2;
            OlderAccount.PostalCode = authenticateViewModel.PostalCode2;
            OlderAccount.FullName = authenticateViewModel.FullName;
            OlderAccount.Address = authenticateViewModel.Address;
            OlderAccount.Image = authenticateViewModel.Image;
            OlderAccount.UpdateDateTime = DateTime.Now;
            OlderAccount.Authenticate = true;
            UnitOfWork.UserRepository.Update(OlderAccount);
            UnitOfWork.Save();

            return (RedirectToAction(MVC.HomeMain.Main()));
        }

        public virtual ActionResult DownloadUserExcel()
        {
            var users = UnitOfWork.UserRepository.Get()
                .ToList();

            // مرتب‌سازی کاربران
            var sortedUsers = users.OrderBy(u => u.FullName, StringComparer.Create(new CultureInfo("fa-IR"), false)).ToList();

            using (var workbook = new XLWorkbook())
            {
                // ایجاد شیت
                var worksheet = workbook.Worksheets.Add("فایل موجودی");
                worksheet.Cell(1, 1).Value = "نام و نام خانوادگی";
                worksheet.Cell(1, 2).Value = "قدرت خرید";
                worksheet.Cell(1, 3).Value = "مبلغ موجودی اولیه";
                worksheet.Cell(1, 4).Value = "مبلغ موجودی باقی مانده";

                for (int i = 0; i < sortedUsers.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = sortedUsers[i].FullName;
                    worksheet.Cell(i + 2, 2).Value = sortedUsers[i].creditAmount;
                    worksheet.Cell(i + 2, 3).Value = sortedUsers[i].InitialCredit;
                    worksheet.Cell(i + 2, 4).Value = sortedUsers[i].InitialCredit - sortedUsers[i].creditAmount;
                }
                // گرفتن تاریخ روز به فرمت شمسی
                var persianCalendar = new System.Globalization.PersianCalendar();
                var now = DateTime.Now;
                var persianDate = $"{persianCalendar.GetYear(now)}/{persianCalendar.GetMonth(now):00}/{persianCalendar.GetDayOfMonth(now):00}";

                // ساخت نام فایل
                var fileName = $"فایل موجودی تاریخ {persianDate}.xlsx";

                // استفاده از MemoryStream برای نگهداری داده‌های Excel
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream); // فایل Excel در استریم ذخیره می‌شود
                    stream.Seek(0, SeekOrigin.Begin); // بازگشت به ابتدای استریم

                    // بازگشت فایل به کاربر برای دانلود
                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName
                    );
                }
            }
        }
        [HttpGet]
        public virtual ActionResult DownloadUserExcelGetByid(System.Guid id)
        {
            // بازیابی نام کاربر از دیتابیس
            var user = UnitOfWork.UserRepository.GetById(id);
            var userName = user?.FullName ?? "User"; // فرض اینکه نام کامل کاربر در `FullName` ذخیره شده است

            // تاریخ روز به صورت فرمت شده
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // بازیابی اطلاعات واریزی و برداشت
            var usersdeposit = UnitOfWork.walletFactorRepository.Get()
                .Where(u => u.UserId == id && u.FinalApprove == true)
                .OrderByDescending(x => x.InsertDateTime)
                .ToList();

            var userswithdrawal = UnitOfWork.FactorCementRepository.Get()
                .Where(u => u.UserId == id && u.FinalApprove == true)
                .OrderByDescending(x => x.InsertDateTime)
                .ToList();

            var depositList = usersdeposit.Select(d => new
            {
                Amount = d.Chargeamount,
                Type = "Deposit",
                Date = d.InsertDateTime,
                Description = d.Description
            }).ToList();

            var withdrawalList = userswithdrawal.Select(w => new
            {
                Amount = (int)w.AmountPaid,
                Type = "Withdrawal",
                Date = w.InsertDateTime,
                Description = w.Description
            }).ToList();

            var transactions = depositList;
            transactions.AddRange(withdrawalList);
            transactions = transactions.OrderByDescending(t => t.Date).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions");

                // تنظیم هدر‌ها
                worksheet.Cell(1, 1).Value = "تاریخ";
                worksheet.Cell(1, 2).Value = "نوع تراکنش";
                worksheet.Cell(1, 3).Value = "مبلغ";
                worksheet.Cell(1, 4).Value = "توضیحات";
                // اضافه کردن اطلاعات تراکنش‌ها
                for (int i = 0; i < transactions.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = transactions[i].Date.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cell(row, 2).Value = transactions[i].Type == "Deposit" ? "واریزی" : "برداشت";
                    worksheet.Cell(row, 3).Value = transactions[i].Amount;
                    worksheet.Cell(row, 4).Value = transactions[i].Description;
                    // رنگ‌بندی پس‌زمینه براساس نوع تراکنش
                    var rowRange = worksheet.Range(row, 1, row, 4); // کل ردیف
                    if (transactions[i].Type == "Deposit")
                    {
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightGreen; // سبز برای واریزی‌ها
                    }
                    else
                    {
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightCoral; // قرمز برای برداشت‌ها
                    }
                }

                // ساختن نام فایل
                var fileName = $"{userName}_{currentDate}.xlsx";

                // ذخیره به استریم و بازگشت فایل
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                fileName);
                }
            }


        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Paymentwallet(System.Guid id)
        {
            ViewBag.PageMessages = null;
            RechargewalletUser model = new RechargewalletUser();
            model.PhoneNumber = UnitOfWork.UserRepository.GetById(id).BuyerMobile;
            return View(model);
        }
        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Paymentwallet(RechargewalletUser rechargewalletUser)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = UnitOfWork.UserRepository.GetByPhoneNumebr(rechargewalletUser.PhoneNumber);
                    if (user != null)
                    {
                        walletFactor OwalletFactor = new walletFactor()
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            Chargeamount = rechargewalletUser.ChargeAmount,
                            BuyerMobile = user.BuyerMobile,
                            FinalApprove = true,
                            Authority = "",
                            InvoiceNumber = 0,
                            Bankcode = 100,
                            card_pan = "",
                            ref_id = 0,
                            AmountPaidDate = DateTime.Now,
                            IsActived = true,
                            IsVerified = true,
                            IsDeleted = false,
                            IsSystem = false,
                            InsertDateTime = DateTime.Now,
                            Description = rechargewalletUser.Description
                        };
                        UnitOfWork.walletFactorRepository.Insertdata(OwalletFactor);
                        user.creditAmount += rechargewalletUser.ChargeAmount;
                        UnitOfWork.UserRepository.Update(user);
                        UnitOfWork.Save();
                        //ارسال پیامک
                        ZarinpalController pyment = new ZarinpalController();
                        pyment.PaymentSMSWallet(user.BuyerMobile, OwalletFactor);
                        ViewBag.PageMessages= "افزایش موجودی با موفقیت افزایش یافت";
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.PageMessages= "خطا دیتا - دیتا های وارد شده را دوباره بررسی نمایید";
                    return View();
                }
                
            }
            ViewBag.PageMessages = "خطا دیتا - دیتا های وارد شده را دوباره بررسی نمایید";
            return View();
        }
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult SendSMSdebtor()
        {
            try
            {
                var phoneNumebrs = UnitOfWork.UserRepository.Get()
                    .Where(u => u.InitialCredit - u.creditAmount > 0 && u.BuyerMobile == "09926932699")
                    .Select(u => u.BuyerMobile)
                    .ToArray();

                ZarinpalController pyment = new ZarinpalController();
                var isSendSMS = pyment.SendSMSdebtor(phoneNumebrs);
                if (isSendSMS == true)
                {
                    ViewBag.PageMessages += "حساب درخواستی شما با موفقیت ثبت گردید  ";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Index");
        }
    }

}