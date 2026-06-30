using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class CementController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            var varHeadLines = UnitOfWork.HeadLineRepository.Get().ToList();
            ViewData["HeadLine"] = new SelectList(varHeadLines, "Id", "Name", null);

            ViewData["SubHeadLine"] =
                new SelectList(new List<Models.SubHeadLine>(), "Id", "Name", null);

            var varProvinces = UnitOfWork.ProvinceRepository.Get().ToList();
            ViewData["Province"] = new SelectList(varProvinces, "Id", "Name", null);

            ViewData["City"] =
                new SelectList(new List<Models.City>(), "Id", "Name", null);

            return View();
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual JsonResult GetHeadOfFactors()
        {
            var varHeadOfFactors =
                UnitOfWork.HeadOfFactorRepository
                .Get(Infrastructure.Sessions.AuthenticatedUser.User);

            var ViewHeadOfFactors =
                varHeadOfFactors
                .OrderByDescending(current => current.InvoiceDate)
                .Take(20)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel()
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

                    InvoiceDate =
                        Infrastructure.Utility.DisplayDateTime
                        (current.InvoiceDate, true),

                    Description = current.Description,
                    CellPhoneNumber = current.CellPhoneNumber,
                    FinalApprove = current.FinalApprove
                })
                .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData
                <ViewModels.Areas.Administrator.HeadOfFactor.IndexViewModel>
                (ViewHeadOfFactors);

            return Json(varResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create(string Code)
        {
            ViewBag.PageMessages = null;

            var varHeadLines =
                UnitOfWork.HeadLineRepository.Get().ToList();

            if (Code == "5")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository.Get()
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
                    UnitOfWork.HeadLineRepository.Get()
                    .Where(x => x.Code == "10")
                    .ToList();

                TempData["Codee"] = Code;
            }

            if (Code == "3")
            {
                varHeadLines =
                    UnitOfWork.HeadLineRepository.Get()
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
                    UnitOfWork.HeadLineRepository.Get()
                    .Where(x => x.Code == "6")
                    .ToList();

                TempData["Codee"] = Code;
            }

            ViewData["HeadLine"] =
                new SelectList(varHeadLines, "Id", "Name", null);

            ViewData["SubHeadLine"] =
                new SelectList(new List<Models.SubHeadLine>(), "Id", "Name", null);

            var varProvinces =
                UnitOfWork.ProvinceRepository.Get().ToList();

            ViewData["Province"] =
                new SelectList(varProvinces, "Id", "Name", null);

            ViewData["City"] =
                new SelectList(new List<Models.City>(), "Id", "Name", null);

            return View();
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Create
        (
            ViewModels.Areas.Administrator.HeadOfFactor.CreateViewModel headoffactor
        )
        {
            ViewBag.PageMessages = null;

            if (ModelState.IsValid)
            {
                Models.HeadOfFactor oHeadOfFactor =
                    new Models.HeadOfFactor();

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
                UnitOfWork.HeadLineRepository.Get().ToList();

            ViewData["HeadLine"] =
                new SelectList(varHeadLines, "Id", "Name", null);

            var varSubHeadLines =
                UnitOfWork.SubHeadLineRepository
                .GetByHeadLineId(headoffactor.HeadLine)
                .ToList();

            ViewData["SubHeadLine"] =
                new SelectList(varSubHeadLines, "Id", "Name", null);

            var varProvinces =
                UnitOfWork.ProvinceRepository.Get().ToList();

            ViewData["Province"] =
                new SelectList(varProvinces, "Id", "Name", null);

            var varCities =
                UnitOfWork.CityRepository
                .GetByProvinceId(headoffactor.Province)
                .ToList();

            ViewData["City"] =
                new SelectList(varCities, "Id", "Name", null);

            return View(headoffactor);
        }

        [HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Details(Guid id)
        {
            var oHeadOfFactor =
                UnitOfWork.HeadOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.HeadOfFactor.DisplayViewModel()
                    {
                        Id = current.Id,
                        HeadLine = current.HeadLine.Name,
                        SubHeadLine = current.SubHeadLine.Name,
                        CompanyName = current.CompanyName,
                        Province = current.Province.Name,
                        City = current.City != null
                            ? current.City.Name
                            : "-",

                        CompanyNationalCode = current.CompanyNationalCode,
                        InvoiceNumber = current.InvoiceNumber,

                        InvoiceDate =
                            Infrastructure.Utility.DisplayDateTime
                            (current.InvoiceDate, true),

                        Description = current.Description,
                        CellPhoneNumber = current.CellPhoneNumber,
                    })
                .FirstOrDefault();

            if (oHeadOfFactor == null)
            {
                return HttpNotFound();
            }

            return View(oHeadOfFactor);
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetCities(Guid provinceId)
        {
            try
            {
                var cities =
                    UnitOfWork.CityRepository
                    .GetByProvinceId(provinceId)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                return Json(new
                {
                    success = false
                });
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetProductType(Guid ProductNameId)
        {
            try
            {
                var result =
                    UnitOfWork.ProductTypeRepository
                    .GetByProductNameId(ProductNameId)
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderByDescending(x => x.Name)
                    .ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                return Json(new
                {
                    success = false
                });
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetPackageType(Guid ProductTypeId)
        {
            try
            {
                var result =
                    UnitOfWork.PackageTypeRepository
                    .GetByProductTypeId(ProductTypeId)
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderByDescending(x => x.Name)
                    .ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                return Json(new
                {
                    success = false
                });
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetFactoryName(Guid ProductNameId)
        {
            try
            {
                var result =
                    UnitOfWork.FactoryNameRepository
                    .GetByProductNameId(ProductNameId)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderByDescending(x => x.Name)
                    .ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                return Json(new
                {
                    success = false
                });
            }
        }

        [HttpPost]
        [Infrastructure.SyncPermission(isPublic: true)]
        public virtual ActionResult GetTonnage(Guid PackageTypeId)
        {
            try
            {
                var result =
                    UnitOfWork.tonnageRepository
                    .GetByPackageTypeId(PackageTypeId)
                    .Select(x => new
                    {
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);

                return Json(new
                {
                    success = false
                });
            }
        }
    }
}