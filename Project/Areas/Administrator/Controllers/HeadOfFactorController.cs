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
    public partial class HeadOfFactorController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null);

            var varSubHeadLines = new List<Models.SubHeadLine>();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = new List<Models.City>();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.JsonResult GetHeadOfFactors()
        {
            var varHeadOfFactors =
                UnitOfWork.HeadOfFactorRepository.Get(Infrastructure.Sessions.AuthenticatedUser.User)
                ;

            var ViewHeadOfFactors
                = varHeadOfFactors.OrderByDescending(current => current.InvoiceDate).Take(20)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel()
                    {
                        Id = current.Id,
                        HeadLine = current.HeadLine.Name,
                        SubHeadLine = current.SubHeadLine?.Name,
                        CompanyName = current.CompanyName,
                        Province = current.City != null ? current.Province.Name + (" - " + current.City.Name) : "",
                        CompanyNationalCode = current.CompanyNationalCode,
                        InvoiceNumber = current.InvoiceNumber,
                        InvoiceDate = Infrastructure.Utility.DisplayDateTime(current.InvoiceDate, true),
                        Description = current.Description,
                        CellPhoneNumber = current.CellPhoneNumber,
                        FinalApprove = current.FinalApprove
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel>(ViewHeadOfFactors);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Create(string Code)
        {
            ViewBag.PageMessages = null;

            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            if (Code == "5")
            {
                varHeadLines = UnitOfWork.HeadLineRepository.Get().Where(x => x.Code == "1" || x.Code == "2" || x.Code == "3" || x.Code == "4" || x.Code == "5").ToList();
                TempData["Codee"] = Code;
            }
            if (Code == "1")
            {
                varHeadLines = UnitOfWork.HeadLineRepository.Get().Where(x => x.Code == "10").ToList();
                TempData["Codee"] = Code;
            }
            if (Code == "3")
            {
                varHeadLines = UnitOfWork.HeadLineRepository.Get().Where(x => x.Code == "7" || x.Code == "8" || x.Code == "9"|| x.Code == "11"|| x.Code == "12"|| x.Code == "13"|| x.Code == "14"|| x.Code == "15"|| x.Code == "16").ToList();
                TempData["Codee"] = Code;
            }
            if (Code == "23")
            {
                varHeadLines = UnitOfWork.HeadLineRepository.Get().Where(x => x.Code == "6").ToList();
                TempData["Codee"] = Code;
            }
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null).OrderBy(x => x.Text);


            var varSubHeadLines = new List<Models.SubHeadLine>();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null).OrderBy(x => x.Text);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null).OrderBy(x => x.Text);

            var varCities = new List<Models.City>();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null).OrderBy(x => x.Text);

            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.HeadOfFactor.CreateViewModel headoffactor)
        {
            ViewBag.PageMessages = null;

            if (ModelState.IsValid)
            {
                #region Insert HeadOfFactor
                Models.HeadOfFactor oHeadOfFactor = new Models.HeadOfFactor();
                oHeadOfFactor.HeadLineId = headoffactor.HeadLine;
                oHeadOfFactor.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                oHeadOfFactor.SubHeadLineId = headoffactor.SubHeadLine;
                oHeadOfFactor.CompanyName = headoffactor.CompanyName;
                oHeadOfFactor.ProvinceId = headoffactor.Province;
                oHeadOfFactor.CityId = headoffactor.City;
                oHeadOfFactor.CompanyNationalCode = headoffactor.CompanyNationalCode;
                oHeadOfFactor.Description = headoffactor.Description;
                oHeadOfFactor.CellPhoneNumber = headoffactor.CellPhoneNumber;
                oHeadOfFactor.IsActived = true;
                oHeadOfFactor.IsDeleted = false;
                oHeadOfFactor.IsSystem = false;
                oHeadOfFactor.IsVerified = true;
                oHeadOfFactor.InvoiceDate = DateTime.Now;
                oHeadOfFactor.InsertDateTime = DateTime.Now;
                oHeadOfFactor.UpdateDateTime = DateTime.Now;
                UnitOfWork.HeadOfFactorRepository.Insert(oHeadOfFactor);
                #endregion

                #region Insert New Message
                Models.FactorMessage oMessage = new Models.FactorMessage();
                oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                oMessage.MessageText = Resources.Message.Request.Message_InitialRequet;
                oMessage.HeadOfFactorId = oHeadOfFactor.Id;
                UnitOfWork.FactorMessageRepository.Insert(oMessage);
                #endregion

                UnitOfWork.Save();

                if (TempData["Codee"] != null)
                {
                    return RedirectToAction("Create", "DetailOfFactor", new { area = "Administrator", headoffactorid = oHeadOfFactor.Id });
                    //           return RedirectToAction("Index", "DetailOfFactor", new { area = "Administrator", headoffactorid = oHeadOfFactor.Id });
                }

                return (RedirectToAction(MVC.Administrator.HeadOfFactor.Index()));
            }


            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", null);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.GetByHeadLineId(headoffactor.HeadLine).ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", null);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(headoffactor.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", null);

            return View(headoffactor);
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Details(System.Guid id)
        {
            ViewBag.MessageList = UnitOfWork.FactorMessageRepository.MetMessageById(id);
            if (id == null)
            {
                return (RedirectToAction
                    (MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.HeadOfFactor.DisplayViewModel oHeadOfFactor
                = UnitOfWork.HeadOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadOfFactor.DisplayViewModel()
                {
                    Id = current.Id,
                    HeadLine = current.HeadLine.Name,
                    SubHeadLine = current.SubHeadLine.Name,
                    CompanyName = current.CompanyName,
                    Province = current.Province.Name,
                    City = current.City!=null?current.City.Name:"-",
                    CompanyNationalCode = current.CompanyNationalCode,
                    InvoiceNumber = current.InvoiceNumber,
                    InvoiceDate = Infrastructure.Utility.DisplayDateTime(current.InvoiceDate,true),
                    Description = current.Description,
                    CellPhoneNumber = current.CellPhoneNumber,
                })
                .FirstOrDefault()
                ;

            if (oHeadOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oHeadOfFactor));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewBag.MessageList = UnitOfWork.FactorMessageRepository.MetMessageById(id);

            ViewModels.Areas.Administrator.HeadOfFactor.EditViewModel oHeadOfFactor
                = UnitOfWork.HeadOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current=>new ViewModels.Areas.Administrator.HeadOfFactor.EditViewModel()
                {
                    Id = current.Id,
                    HeadLine = current.HeadLineId,
                    SubHeadLine = current.SubHeadLineId,
                    CompanyName = current.CompanyName,
                    Province = current.ProvinceId,
                    City = current.CityId,
                    CompanyNationalCode = current.CompanyNationalCode,
                    InvoiceNumber = current.InvoiceNumber,
                    InvoiceDate = Infrastructure.Utility.DisplayDateTime(current.InvoiceDate,true),
                    Description = current.Description,
                    CellPhoneNumber = current.CellPhoneNumber,
                })
                .FirstOrDefault()
                ;

            ViewBag.MessageList = UnitOfWork.FactorMessageRepository.MetMessageById(id);


            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", oHeadOfFactor.HeadLine);

            var varSubHeadLines = UnitOfWork.SubHeadLineRepository.GetByHeadLineId(oHeadOfFactor.HeadLine).ToList();
            ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", oHeadOfFactor.SubHeadLine);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", oHeadOfFactor.Province);

            var varCities = UnitOfWork.CityRepository.GetByProvinceId(oHeadOfFactor.Province).ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", oHeadOfFactor.City);

            if (oHeadOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oHeadOfFactor));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.HeadOfFactor.EditViewModel headoffactor)
        {
            try
            {
                var oHeadOfFactor =
                    UnitOfWork.HeadOfFactorRepository
                    .Get()
                    .Where(current => current.Id == headoffactor.Id)
                    .FirstOrDefault()
                    ;

                if (ModelState.IsValid)
                {

                    #region Insert HeadOfFactor
                    oHeadOfFactor.HeadLineId = headoffactor.HeadLine;
                    oHeadOfFactor.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oHeadOfFactor.SubHeadLineId = headoffactor.SubHeadLine;
                    oHeadOfFactor.CompanyName = headoffactor.CompanyName;
                    oHeadOfFactor.ProvinceId = headoffactor.Province;
                    oHeadOfFactor.CityId = headoffactor.City;
                    oHeadOfFactor.CompanyNationalCode = headoffactor.CompanyNationalCode;
                    oHeadOfFactor.Description = headoffactor.Description;
                    oHeadOfFactor.CellPhoneNumber = headoffactor.CellPhoneNumber;
                    oHeadOfFactor.IsActived = true;
                    oHeadOfFactor.IsDeleted = false;
                    oHeadOfFactor.IsSystem = false;
                    oHeadOfFactor.IsVerified = true;
                    oHeadOfFactor.UpdateDateTime = DateTime.Now;
                    UnitOfWork.HeadOfFactorRepository.Update(oHeadOfFactor);
                    #endregion

                    #region Insert New Message
                    Models.FactorMessage oMessage = new Models.FactorMessage();
                    oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oMessage.MessageText = Resources.Message.Request.Message_Update;
                    oMessage.HeadOfFactorId = oHeadOfFactor.Id;
                    UnitOfWork.FactorMessageRepository.Insert(oMessage);
                    #endregion

                    UnitOfWork.Save();

                    return (RedirectToAction(MVC.Administrator.HeadOfFactor.Index()));

                }


                var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
                ViewData["HeadLine"] = new System.Web.Mvc.SelectList(varHeadLines, "Id", "Name", headoffactor.HeadLine);

                var varSubHeadLines = UnitOfWork.SubHeadLineRepository.GetByHeadLineId(headoffactor.HeadLine).ToList();
                ViewData["SubHeadLine"] = new System.Web.Mvc.SelectList(varSubHeadLines, "Id", "Name", headoffactor.SubHeadLine);

                var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
                ViewData["Province"] = new System.Web.Mvc.SelectList(varProvinces, "Id", "Name", headoffactor.Province);

                var varCities = UnitOfWork.CityRepository.GetByProvinceId(headoffactor.Province).ToList();
                ViewData["City"] = new System.Web.Mvc.SelectList(varCities, "Id", "Name", headoffactor.City);

                return View(headoffactor);
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Delete(System.Guid id)
        {
            ViewBag.MessageList = UnitOfWork.FactorMessageRepository.MetMessageById(id);
            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.HeadOfFactor.DisplayViewModel oHeadOfFactor
                = UnitOfWork.HeadOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadOfFactor.DisplayViewModel()
                {
                    Id = current.Id,
                    HeadLine = current.HeadLine.Name,
                    SubHeadLine = current.SubHeadLine.Name,
                    CompanyName = current.CompanyName,
                    Province = current.Province.Name,
                    City = current.City != null ? current.City.Name : "-",
                    CompanyNationalCode = current.CompanyNationalCode,
                    InvoiceNumber = current.InvoiceNumber,
                    InvoiceDate = Infrastructure.Utility.DisplayDateTime(current.InvoiceDate, true),
                    Description = current.Description,
                    CellPhoneNumber = current.CellPhoneNumber,
                })
                .FirstOrDefault()
                ;

            if (oHeadOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oHeadOfFactor));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varHeadOfFactors =
                    UnitOfWork.HeadOfFactorRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                if (varHeadOfFactors != null)
                {
                    UnitOfWork.HeadOfFactorRepository.Delete(varHeadOfFactors);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.HeadOfFactor.Index()));
                }

                else
                    return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            catch (Exception ex)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }
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
                 .OrderBy(x => x.Name)
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
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetSubHeadLines(System.Guid HeadLineId)
        {
            try
            {
                var cities =
                 UnitOfWork.SubHeadLineRepository.GetByHeadLineId(HeadLineId)
                 .Select(x => new
                 {
                     Name = x.Name,
                     Id = x.Id
                 })
                 .OrderBy(x => x.Name)
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

    }
}
