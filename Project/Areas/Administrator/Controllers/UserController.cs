using ClosedXML.Excel;
using OPS.GetPostinfoServices;
using OPS.PersonInfoServices;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using OPS.Controllers;
using Utilities.PersianDate;
using ViewModels.Areas.Administrator.User;
using System.Transactions;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class UserController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Index(int? page)
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
            ViewBag.CurrentPage = page ?? 1;
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
                 .OrderBy(user => user.FullName)
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
                         MarketingCode = current.MarketingCode,
                         ReferredByCode = current.ReferredByCode,
                         //IsApprovallicense = current.IsApprovallicense,
                         Authenticate = current.Authenticate,
                     })
                     .ToList()
                     .OrderBy(u => u.FullName)
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
                         IsMarketer = current.IsMarketer,
                         MarketingCode = current.MarketingCode,
                         ReferredByCode = current.ReferredByCode,
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
                // گرفتن همه‌ی کاربران
                var varUser = UnitOfWork.UserRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User);

                var data = varUser
                    .OrderBy(u => u.FullName)
                    .Select(u => new
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        UserName = u.UserName,
                        Role = u.Role.Name,
                        Province = u.Province != null ? u.Province.Name : "[نا مشخص]",
                        City = u.City != null ? u.City.Name : "[نا مشخص]",
                        IsActive = u.IsActived,
                        creditAmount = u.creditAmount,
                        BuyerMobile = u.BuyerMobile,
                        Address = u.Address,
                        InitialCredit = u.InitialCredit,
                        MarketingCode = u.MarketingCode,
                        ReferredByCode = u.ReferredByCode,
                        Discount = u.Discount
                    }).ToList();

                var draw = Request.Form["draw"] ?? "1";

                return Json(new
                {
                    draw = int.Parse(draw),
                    recordsTotal = data.Count,
                    recordsFiltered = data.Count,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
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

            if (!string.IsNullOrWhiteSpace(user.MarketingCode) &&
                UnitOfWork.UserRepository.IsMarketingCodeAvailable(user.MarketingCode))
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
                ViewBag.PageMessages += "کد بازاریابی تکراری است";
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
                    oUser.BuyerMobile = user.PhoneNumebr;
                    oUser.Authenticate = true;
                    oUser.MarketingCode = user.MarketingCode;
                    oUser.ReferredByCode = user.ReferredByCode;
                    oUser.InitialCredit = user.InitialCredit;
                    oUser.isSendSms = user.isSendSmS;
                    oUser.IsMarketer = user.IsMarketer;
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
                    MarketingCode = current.MarketingCode,
                    ReferredByCode = current.ReferredByCode,
                    InitialCredit = current.InitialCredit,
                    Discount = current.Discount,
                    IsMarketer = current.IsMarketer
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
                    OlderAccount.Discount = user.Discount;
                    if (user.Role != new Guid("805f9b24-c5e0-4227-9efa-7b5eb5646394"))
                    {
                        OlderAccount.ReferredByCode = user.ReferredByCode;
                    }
                    else
                    {
                        OlderAccount.MarketingCode = user.MarketingCode;
                    }
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
                    UnitOfWork.UserRepository.Delete(varUser);
                    UnitOfWork.Save();
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
                worksheet.Cell(1, 3).Value = "میزان اعتبار داده شده";
                worksheet.Cell(1, 4).Value = "میزان بدهی";

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
        public virtual ActionResult DownloadUserExcelGetByid(Guid id)
        {
            // --- بازیابی اطلاعات کاربر
            var user = UnitOfWork.UserRepository.GetById(id);
            var userName = user?.FullName ?? "User";
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // --- دریافت واریزی‌ها
            var deposits = UnitOfWork.walletFactorRepository.Get()
                .Where(u => u.UserId == id && u.FinalApprove == true)
                .OrderByDescending(x => x.InsertDateTime)
                .ToList();

            // --- دریافت برداشت‌ها
            var withdrawals = UnitOfWork.FactorCementRepository.Get()
                .Where(u => u.UserId == id && u.FinalApprove == true)
                .OrderByDescending(x => x.InsertDateTime)
                .ToList();

            // --- تبدیل واریزی‌ها
            var depositList = deposits.Select(d => new
            {
                Amount = d.Chargeamount,
                Type = "Deposit",
                Date = d.InsertDateTime,
                Description = d.Description
            }).ToList();

            // --- تبدیل برداشت‌ها
            var withdrawalList = withdrawals.Select(w =>
            {
                string tonnageValue = "نامشخص";

                if (w.TonnageId.HasValue)
                {
                    var tonnage = UnitOfWork.tonnageRepository.GetById(w.TonnageId.Value);
                    if (!string.IsNullOrWhiteSpace(tonnage?.Name))
                        tonnageValue = tonnage.Name;
                }
                else if (w.Tonnagedouble > 0)
                {
                    tonnageValue = w.Tonnagedouble + " تن";
                }
                else if (w.Tonnage?.Name != "0")
                {
                    tonnageValue = w.Tonnage?.Name + " تن";
                }

                var productName = w.ProductName?.Name ?? "نامشخص";
                var productType = w.ProductType?.Name ?? "نامشخص";
                var packageType = w.PackageType?.Name ?? "نامشخص";
                var factoryName = w.FactoryName?.Name ?? "نامشخص";

                return new
                {
                    Amount = (long)w.AmountPaid,
                    Type = "Withdrawal",
                    Date = w.InsertDateTime,
                    Description = $"محصول: {productName}\n" +
                                  $"نوع محصول: {productType}\n" +
                                  $"نوع بسته‌بندی: {packageType}\n" +
                                  $"کارخانه: {factoryName}\n" +
                                  $"تناژ: {tonnageValue}"
                };
            }).ToList();

            // --- ادغام و مرتب‌سازی تراکنش‌ها از قدیمی‌ترین به جدیدترین
            var transactions = depositList;
            transactions.AddRange(withdrawalList);
            transactions = transactions.OrderBy(t => t.Date).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions");

                // --- هدرها
                worksheet.Cell(1, 1).Value = "تاریخ (شمسی - ساعت)";
                worksheet.Cell(1, 2).Value = "نوع تراکنش";
                worksheet.Cell(1, 3).Value = "مبلغ";
                worksheet.Cell(1, 4).Value = "توضیحات";
                worksheet.Cell(1, 5).Value = "موجودی تجمعی";

                long cumulative = 0;
                var pc = new PersianCalendar();

                for (int i = 0; i < transactions.Count; i++)
                {
                    var t = transactions[i];
                    var row = i + 2;

                    // --- محاسبه موجودی تجمعی
                    if (t.Type == "Deposit")
                        cumulative += t.Amount;
                    else
                        cumulative -= t.Amount;

                    // --- تبدیل تاریخ به رشته شمسی با ساعت (فرمت گزینه A)
                    var d = t.Date;
                    string persianDateTime =
                        $"{pc.GetYear(d):0000}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00} - {d:HH:mm}";

                    // --- درج اطلاعات در اکسل
                    worksheet.Cell(row, 1).Value = persianDateTime;
                    worksheet.Cell(row, 2).Value = t.Type == "Deposit" ? "واریزی" : "برداشت";
                    worksheet.Cell(row, 3).Value = t.Amount;
                    worksheet.Cell(row, 4).Value = t.Description;
                    worksheet.Cell(row, 5).Value = cumulative;

                    // --- رنگ‌بندی ردیف‌ها
                    var rowRange = worksheet.Range(row, 1, row, 5);
                    if (t.Type == "Deposit")
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    else
                        rowRange.Style.Fill.BackgroundColor = XLColor.LightCoral;
                }

                // --- تنظیم عرض ستون‌ها
                worksheet.Columns().AdjustToContents();

                // --- نام فایل خروجی
                var fileName = $"{userName}_{currentDate}.xlsx";

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
        public virtual ActionResult Paymentwallet(Guid id, int? page)
        {
            ViewBag.PageMessages = null;

            // پیدا کردن کاربر
            var user = UnitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                ViewBag.PageMessages = "کاربر یافت نشد";
                return RedirectToAction("Index", "User", new { area = "Administrator", page = page ?? 1 });
            }

            // ساخت مدل برای ارسال به View
            RechargewalletUser model = new RechargewalletUser
            {
                PhoneNumber = user.BuyerMobile,
                PageNumber = page ?? 1,
                TransactionId = Guid.NewGuid() // شناسه یکتا برای جلوگیری از شارژ دوباره
            };

            return View(model);
        }


        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual ActionResult Paymentwallet(RechargewalletUser rechargewalletUser)
        {
            int page = rechargewalletUser.PageNumber;
            if (!ModelState.IsValid)
            {
                ViewBag.PageMessages = "خطا دیتا - دیتا های وارد شده را دوباره بررسی کنید";
                return View(rechargewalletUser);
            }

            try
            {
                walletFactor walletFactor = null;

                using (var scope = new TransactionScope())
                {
                    var user = UnitOfWork.UserRepository.GetByPhoneNumebr(rechargewalletUser.PhoneNumber);
                    if (user == null)
                    {
                        ViewBag.PageMessages = "کاربر یافت نشد";
                        return View(rechargewalletUser);
                    }

                    // بررسی تراکنش تکراری
                    bool exists = UnitOfWork.walletFactorRepository
                        .Get(w => w.TransactionId == rechargewalletUser.TransactionId)
                        .Any();

                    if (exists)
                    {
                        TempData["PageMessages"] = "این تراکنش قبلا ثبت شده است";
                        return RedirectToAction("Index", "User", new { area = "Administrator", page = page });
                    }

                    // بررسی اینکه اگر مبلغ منفی بود، موجودی کافی باشه
                    if (rechargewalletUser.ChargeAmount < 0 && user.creditAmount < Math.Abs(rechargewalletUser.ChargeAmount))
                    {
                        ViewBag.PageMessages = "موجودی کاربر کافی نیست";
                        return View(rechargewalletUser);
                    }

                    // ایجاد فاکتور جدید
                    walletFactor = new walletFactor
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
                        Description = rechargewalletUser.Description,
                        TransactionId = rechargewalletUser.TransactionId
                    };

                    UnitOfWork.walletFactorRepository.Insertdata(walletFactor);

                    // افزایش یا کاهش موجودی بر اساس علامت مبلغ
                    user.creditAmount += rechargewalletUser.ChargeAmount;
                    UnitOfWork.UserRepository.Update(user);

                    UnitOfWork.Save();
                    scope.Complete();
                }

                TempData["PageMessages"] = "تراکنش با موفقیت انجام شد";
                return RedirectToAction("Index", "User", new { area = "Administrator", page = page });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا در Paymentwallet: {ex.Message}");
                ViewBag.PageMessages = "خطا دیتا - دیتا های وارد شده را دوباره بررسی نمایید";
                return View(rechargewalletUser);
            }
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

        //[System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        //public virtual ActionResult SendSMSPrice()
        //{
        //    try
        //    {
        //        var phoneNumebrs = UnitOfWork.UserRepository.Get()
        //            .Where(u =>  u.BuyerMobile == "09926932699")
        //            .Select(u => u.BuyerMobile)
        //            .ToArray();

        //        ZarinpalController pyment = new ZarinpalController();
        //        var isSendSMS = pyment.SendSMSPrice(phoneNumebrs);
        //        if (isSendSMS == true)
        //        {
        //            ViewBag.PageMessages += "حساب درخواستی شما با موفقیت ثبت گردید  ";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //    return RedirectToAction("Index");
        //}
    }

}