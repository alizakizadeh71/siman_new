using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class HeadOfFactorController :
        Infrastructure.BaseControllerWithUnitOfWork
    {
        // =========================================================
        // INDEX
        // =========================================================

        [HttpGet]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var varHeadLines =
                UnitOfWork.HeadLineRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["HeadLine"] =
                new SelectList(varHeadLines, "Id", "Name", null);

            var varSubHeadLines =
                new List<Models.SubHeadLine>();

            ViewData["SubHeadLine"] =
                new SelectList(varSubHeadLines, "Id", "Name", null);

            var varProvinces =
                UnitOfWork.ProvinceRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["Province"] =
                new SelectList(varProvinces, "Id", "Name", null);

            var varCities =
                new List<Models.City>();

            ViewData["City"] =
                new SelectList(varCities, "Id", "Name", null);

            return View();
        }

        // =========================================================
        // GRID
        // =========================================================

        [HttpPost]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult GetHeadOfFactors()
        {
            var varHeadOfFactors =
                UnitOfWork.HeadOfFactorRepository
                .Get(Infrastructure.Sessions.AuthenticatedUser.User)
                .Where(x => !x.IsDeleted);

            var ViewHeadOfFactors =
                varHeadOfFactors
                .OrderByDescending(current => current.InvoiceDate)
                .Take(20)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel()
                    {
                        Id = current.Id,
                        HeadLine = current.HeadLine.Name,
                        SubHeadLine = current.SubHeadLine != null
                            ? current.SubHeadLine.Name
                            : "",
                        CompanyName = current.CompanyName,
                        Province = current.City != null
                            ? current.Province.Name + " - " + current.City.Name
                            : "",
                        CompanyNationalCode = current.CompanyNationalCode,
                        InvoiceNumber = current.InvoiceNumber,
                        InvoiceDate = Infrastructure.Utility.DisplayDateTime(
                            current.InvoiceDate,
                            true),
                        Description = current.Description,
                        CellPhoneNumber = current.CellPhoneNumber,
                        FinalApprove = current.FinalApprove
                    })
                .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<
                    ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel>
                    (ViewHeadOfFactors);

            return Json(
                varResult,
                JsonRequestBehavior.AllowGet);
        }

        // =========================================================
        // CREATE GET
        // =========================================================

        [HttpGet]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create(string Code)
        {
            ViewBag.PageMessages = null;

            var varHeadLines =
                UnitOfWork.HeadLineRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            if (Code == "5")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(x =>
                        x.Code == "1" ||
                        x.Code == "2" ||
                        x.Code == "3" ||
                        x.Code == "4" ||
                        x.Code == "5")
                    .ToList();

                TempData["Codee"] = Code;
            }

            if (Code == "1")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(x => x.Code == "10")
                    .ToList();

                TempData["Codee"] = Code;
            }

            if (Code == "3")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(x =>
                        x.Code == "7" ||
                        x.Code == "8" ||
                        x.Code == "9" ||
                        x.Code == "11" ||
                        x.Code == "12" ||
                        x.Code == "13" ||
                        x.Code == "14" ||
                        x.Code == "15" ||
                        x.Code == "16")
                    .ToList();

                TempData["Codee"] = Code;
            }

            if (Code == "23")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository
                    .Get()
                    .Where(x => x.Code == "6")
                    .ToList();

                TempData["Codee"] = Code;
            }

            ViewData["HeadLine"] =
                new SelectList(varHeadLines, "Id", "Name", null);

            var varSubHeadLines =
                new List<Models.SubHeadLine>();

            ViewData["SubHeadLine"] =
                new SelectList(varSubHeadLines, "Id", "Name", null);

            var varProvinces =
                UnitOfWork.ProvinceRepository
                .Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .ToList();

            ViewData["Province"] =
                new SelectList(varProvinces, "Id", "Name", null);

            var varCities =
                new List<Models.City>();

            ViewData["City"] =
                new SelectList(varCities, "Id", "Name", null);

            return View();
        }

        // =========================================================
        // CREATE POST
        // =========================================================

        [HttpPost]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create(
            ViewModels.Areas.Administrator.HeadOfFactor.CreateViewModel
            headoffactor)
        {
            ViewBag.PageMessages = null;

            if (ModelState.IsValid)
            {
                Models.HeadOfFactor oHeadOfFactor =
                    new Models.HeadOfFactor();

                oHeadOfFactor.HeadLineId =
                    headoffactor.HeadLine;

                oHeadOfFactor.UserId =
                    Infrastructure.Sessions.AuthenticatedUser.Id;

                oHeadOfFactor.SubHeadLineId =
                    headoffactor.SubHeadLine;

                oHeadOfFactor.CompanyName =
                    headoffactor.CompanyName;

                oHeadOfFactor.ProvinceId =
                    headoffactor.Province;

                oHeadOfFactor.CityId =
                    headoffactor.City;

                oHeadOfFactor.CompanyNationalCode =
                    headoffactor.CompanyNationalCode;

                oHeadOfFactor.Description =
                    headoffactor.Description;

                oHeadOfFactor.CellPhoneNumber =
                    headoffactor.CellPhoneNumber;

                oHeadOfFactor.IsActived = true;
                oHeadOfFactor.IsDeleted = false;
                oHeadOfFactor.IsSystem = false;
                oHeadOfFactor.IsVerified = true;

                oHeadOfFactor.InvoiceDate =
                    DateTime.Now;

                oHeadOfFactor.InsertDateTime =
                    DateTime.Now;

                oHeadOfFactor.UpdateDateTime =
                    DateTime.Now;

                UnitOfWork.HeadOfFactorRepository
                    .Insert(oHeadOfFactor);

                UnitOfWork.Save();

                if (TempData["Codee"] != null)
                {
                    return RedirectToAction(
                        "Create",
                        "DetailOfFactor",
                        new
                        {
                            area = "Administrator",
                            headoffactorid = oHeadOfFactor.Id
                        });
                }

                return RedirectToAction("Index");
            }

            var varHeadLines =
                UnitOfWork.HeadLineRepository
                .Get()
                .ToList();

            ViewData["HeadLine"] =
                new SelectList(
                    varHeadLines,
                    "Id",
                    "Name",
                    headoffactor.HeadLine);

            var varSubHeadLines =
                UnitOfWork.SubHeadLineRepository
                .GetByHeadLineId(headoffactor.HeadLine)
                .ToList();

            ViewData["SubHeadLine"] =
                new SelectList(
                    varSubHeadLines,
                    "Id",
                    "Name",
                    headoffactor.SubHeadLine);

            var varProvinces =
                UnitOfWork.ProvinceRepository
                .Get()
                .ToList();

            ViewData["Province"] =
                new SelectList(
                    varProvinces,
                    "Id",
                    "Name",
                    headoffactor.Province);

            var varCities =
                UnitOfWork.CityRepository
                .GetByProvinceId(headoffactor.Province)
                .ToList();

            ViewData["City"] =
                new SelectList(
                    varCities,
                    "Id",
                    "Name",
                    headoffactor.City);

            return View(headoffactor);
        }

        // =========================================================
        // DETAILS
        // =========================================================

        [HttpGet]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Details(Guid id)
        {
            var oHeadOfFactor =
                UnitOfWork.HeadOfFactorRepository
                .Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator
                    .HeadOfFactor.DisplayViewModel()
                    {
                        Id = current.Id,
                        HeadLine = current.HeadLine.Name,
                        SubHeadLine = current.SubHeadLine != null
                            ? current.SubHeadLine.Name
                            : "-",
                        CompanyName = current.CompanyName,
                        Province = current.Province.Name,
                        City = current.City != null
                            ? current.City.Name
                            : "-",
                        CompanyNationalCode =
                            current.CompanyNationalCode,
                        InvoiceNumber = current.InvoiceNumber,
                        InvoiceDate =
                            Infrastructure.Utility.DisplayDateTime(
                                current.InvoiceDate,
                                true),
                        Description = current.Description,
                        CellPhoneNumber =
                            current.CellPhoneNumber,
                    })
                .FirstOrDefault();

            if (oHeadOfFactor == null)
            {
                return RedirectToAction(
                    "Display",
                    "Error",
                    new
                    {
                        statusCode =
                            System.Net.HttpStatusCode.NotFound
                    });
            }

            return View(oHeadOfFactor);
        }

        // =========================================================
        // DELETE
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(
            isPublic: false,
            role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var varHeadOfFactors =
                    UnitOfWork.HeadOfFactorRepository
                    .Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                if (varHeadOfFactors != null)
                {
                    varHeadOfFactors.IsDeleted = true;
                    varHeadOfFactors.IsActived = false;
                    varHeadOfFactors.UpdateDateTime =
                        DateTime.Now;

                    UnitOfWork.HeadOfFactorRepository
                        .Update(varHeadOfFactors);

                    UnitOfWork.Save();

                    return RedirectToAction("Index");
                }

                return RedirectToAction(
                    "Display",
                    "Error",
                    new
                    {
                        statusCode =
                            System.Net.HttpStatusCode.NotFound
                    });
            }
            catch
            {
                return RedirectToAction(
                    "Display",
                    "Error",
                    new
                    {
                        statusCode =
                            System.Net.HttpStatusCode.NotFound
                    });
            }
        }

        // =========================================================
        // AJAX
        // =========================================================

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetCities(Guid provinceId)
        {
            try
            {
                var cities =
                    UnitOfWork.CityRepository
                    .GetByProvinceId(provinceId)
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return Json(
                    cities,
                    JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(
                    new List<object>(),
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetSubHeadLines(Guid HeadLineId)
        {
            try
            {
                var subHeadLines =
                    UnitOfWork.SubHeadLineRepository
                    .GetByHeadLineId(HeadLineId)
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return Json(
                    subHeadLines,
                    JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(
                    new List<object>(),
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}