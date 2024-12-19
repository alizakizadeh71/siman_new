using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class DetailOfFactorController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Index(Guid headoffactorid)
        {
            var varServiceTariffs = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(headoffactorid).ToList();
            ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "NameString", null);
            ViewBag.HeadOfFactor = headoffactorid;

            var varHeadOfFactors =
                UnitOfWork.HeadOfFactorRepository.Get()
                .Where(x => x.IsActived)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Id == headoffactorid)
                .FirstOrDefault()
                ;

            ViewBag.BtnShow = varHeadOfFactors.FinalApprove;
            ViewModels.Areas.Administrator.DetailOfFactor.IndexViewModel onew =
                new ViewModels.Areas.Administrator.DetailOfFactor.IndexViewModel();

            onew.HeadLine = varHeadOfFactors.HeadLine.Name;
            onew.SubHeadLine = varHeadOfFactors.SubHeadLine.Name;
            onew.CompanyName = varHeadOfFactors.CompanyName;

            return View(onew);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.JsonResult GetDetailOfFactors(Guid headoffactorid)
        {
            var varDetailOfFactors =
                UnitOfWork.DetailOfFactorRepository.Get(headoffactorid)
                ;

            var ViewDetailOfFactors
                = varDetailOfFactors.OrderByDescending(current => current.InsertDateTime)
                .ToList()
                .Select(current =>
                    new ViewModels.Areas.Administrator.DetailOfFactor.IndexViewModel()
                    {
                        Id = current.Id,
                        HeadOfFactor = current.HeadOfFactor.Description,
                        ServiceTariff = current.ServiceTariff.NameString,
                        CommodityDescription = current.CommodityDescription,
                        CommodityCount = current.CommodityCount,
                        PricePerUnit = current.ServiceTariff.Amount * current.CurrencyRatio,
                        TotalPrice = current.CommodityCount * current.ServiceTariff.Amount * current.CurrencyRatio,
                        HeadLine = current.HeadOfFactor.HeadLine.Name,
                        SubHeadLine = current.HeadOfFactor.SubHeadLine.Name,
                        CompanyName = current.HeadOfFactor.CompanyName
                    })
                    .AsQueryable();

            var varResult =
                Utilities.Kendo.HtmlHelpers
                .ParseGridData<ViewModels.Areas.Administrator.DetailOfFactor.IndexViewModel>(ViewDetailOfFactors);

            return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));

        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Create(Guid headoffactorid)
        {
            ViewBag.PageMessages = null;

            var varServiceTariffs = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(headoffactorid).ToList()
                //ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "NameString", null).OrderBy(p => p.Text);
                .Select(x => new ViewModels.ComboboxItemGuid
                {
                    Id = x.Id,
                    Name = x.NameString
                })
                .OrderBy(current => current.Name)
                .ToList();
            ViewBag.ServiceTariff = varServiceTariffs;


            var varCurrencyUnit = UnitOfWork.CurrencyUnitRepository.Get().ToList()
                .Select(x => new ViewModels.ComboboxItemGuid
                {
                    Id = x.Id,
                    Name = x.NameString
                })
                    .OrderBy(current => current.Name)
                    .ToList();
            ViewBag.CurrencyUnit = varCurrencyUnit;


            ViewModels.Areas.Administrator.DetailOfFactor.CreateViewModel oCreate
                = new ViewModels.Areas.Administrator.DetailOfFactor.CreateViewModel();
            oCreate.HeadOfFactor = headoffactorid;

            return View(oCreate);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Create(ViewModels.Areas.Administrator.DetailOfFactor.CreateViewModel detailOfFactor)
        {
            ViewBag.PageMessages = null;
            decimal CurrencyRatio = 1;
            if (detailOfFactor.CurrencyUnit != null)
            {
                CurrencyRatio = UnitOfWork.CurrencyUnitRepository.Get()
               .Where(current => current.Id == detailOfFactor.CurrencyUnit)
               .FirstOrDefault().Ratio;
            }
            if (detailOfFactor.CommodityDescription?.Length > 99)
            {
                ViewBag.PageMessages = "تعداد حروف شرح خدمت باید کمتر از 100 کاراکتر باشد";

                var varServiceTariffs1 = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(detailOfFactor.HeadOfFactor).ToList();
                ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs1, "Id", "NameString", null);
                return View(detailOfFactor);
            }
            if (ModelState.IsValid)
            {
                #region Insert DetailOfFactor
                Models.DetailOfFactor oDetailOfFactor = new Models.DetailOfFactor();
                oDetailOfFactor.HeadOfFactorId = detailOfFactor.HeadOfFactor;
                oDetailOfFactor.ServiceTariffId = detailOfFactor.ServiceTariff;
                oDetailOfFactor.CommodityDescription = detailOfFactor.CommodityDescription;
                oDetailOfFactor.CommodityCount = detailOfFactor.CommodityCount;
                oDetailOfFactor.Description = detailOfFactor.Description;
                oDetailOfFactor.CurrencyUnitId = detailOfFactor.CurrencyUnit != null ? detailOfFactor.CurrencyUnit : null;
                oDetailOfFactor.CurrencyRatio = CurrencyRatio;
                oDetailOfFactor.IsActived = true;
                oDetailOfFactor.IsDeleted = false;
                oDetailOfFactor.IsSystem = false;
                oDetailOfFactor.IsVerified = true;
                oDetailOfFactor.InsertDateTime = DateTime.Now;
                oDetailOfFactor.UpdateDateTime = DateTime.Now;
                UnitOfWork.DetailOfFactorRepository.Insert(oDetailOfFactor);
                #endregion

                UnitOfWork.Save();

                return (RedirectToAction(MVC.Administrator.DetailOfFactor.Index(detailOfFactor.HeadOfFactor)));
            }

            var varServiceTariffs = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(detailOfFactor.HeadOfFactor).ToList();
            ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "NameString", null);

            return View(detailOfFactor);
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

            ViewModels.Areas.Administrator.DetailOfFactor.DisplayViewModel oDetailOfFactor
                = UnitOfWork.DetailOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.DetailOfFactor.DisplayViewModel()
                {
                    Id = current.Id,
                    HeadOfFactor = current.HeadOfFactor.Description,
                    ServiceTariff = current.ServiceTariff.Name,
                    CommodityDescription = current.CommodityDescription,
                    CommodityCount = current.CommodityCount,
                    Description = current.Description,
                })
                .FirstOrDefault()
                ;

            if (oDetailOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oDetailOfFactor));
        }

        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(System.Guid id)
        {
            if (id == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }

            ViewModels.Areas.Administrator.DetailOfFactor.EditViewModel oDetailOfFactor
                = UnitOfWork.DetailOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .Select(current => new ViewModels.Areas.Administrator.DetailOfFactor.EditViewModel()
                {
                    Id = current.Id,
                    HeadOfFactor = current.HeadOfFactorId,
                    ServiceTariff = current.ServiceTariffId,
                    CommodityDescription = current.CommodityDescription,
                    CommodityCount = current.CommodityCount,
                    Description = current.Description,
                })
                .FirstOrDefault()
                ;

            var varServiceTariffs = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(oDetailOfFactor.HeadOfFactor).ToList();
            ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "NameString", null);

            if (oDetailOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oDetailOfFactor));
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult Edit(ViewModels.Areas.Administrator.DetailOfFactor.EditViewModel detailOfFactor)
        {
            try
            {
                var oDetailOfFactor =
                    UnitOfWork.DetailOfFactorRepository
                    .Get()
                    .Where(current => current.Id == detailOfFactor.Id)
                    .FirstOrDefault()
                    ;

                if (ModelState.IsValid)
                {

                    #region Insert DetailOfFactor
                    oDetailOfFactor.HeadOfFactorId = detailOfFactor.HeadOfFactor;
                    oDetailOfFactor.ServiceTariffId = detailOfFactor.ServiceTariff;
                    oDetailOfFactor.CommodityDescription = detailOfFactor.CommodityDescription;
                    oDetailOfFactor.CommodityCount = detailOfFactor.CommodityCount;
                    oDetailOfFactor.Description = detailOfFactor.Description;
                    oDetailOfFactor.IsActived = true;
                    oDetailOfFactor.IsDeleted = false;
                    oDetailOfFactor.IsSystem = false;
                    oDetailOfFactor.IsVerified = true;
                    oDetailOfFactor.UpdateDateTime = DateTime.Now;
                    UnitOfWork.DetailOfFactorRepository.Update(oDetailOfFactor);
                    #endregion

                    UnitOfWork.Save();

                    return (RedirectToAction(MVC.Administrator.DetailOfFactor.Index(detailOfFactor.HeadOfFactor)));

                }

                var varServiceTariffs = UnitOfWork.DetailOfFactorRepository.GetServiceTariff(detailOfFactor.HeadOfFactor).ToList();
                ViewData["ServiceTariff"] = new System.Web.Mvc.SelectList(varServiceTariffs, "Id", "NameString", null);

                return View(detailOfFactor);
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

            ViewModels.Areas.Administrator.DetailOfFactor.DisplayViewModel oDetailOfFactor
                = UnitOfWork.DetailOfFactorRepository.Get()
                .Where(current => current.Id == id)
                .ToList()
                .Select(current => new ViewModels.Areas.Administrator.DetailOfFactor.DisplayViewModel()
                {
                    Id = current.Id,
                    HeadOfFactor = current.HeadOfFactor.Description,
                    ServiceTariff = current.ServiceTariff.Name,
                    CommodityDescription = current.CommodityDescription,
                    CommodityCount = current.CommodityCount,
                    Description = current.Description,
                })
                .FirstOrDefault()
                ;

            if (oDetailOfFactor == null)
            {
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.NotFound)));
            }

            return (View(oDetailOfFactor));
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual System.Web.Mvc.ActionResult DeleteConfirmed(System.Guid id)
        {
            try
            {
                var varDetailOfFactors =
                    UnitOfWork.DetailOfFactorRepository.Get()
                    .Where(current => current.Id == id)
                    .FirstOrDefault();

                if (varDetailOfFactors != null)
                {
                    UnitOfWork.DetailOfFactorRepository.Delete(varDetailOfFactors);
                    UnitOfWork.Save();
                    return (RedirectToAction(MVC.Administrator.DetailOfFactor.Index(varDetailOfFactors.HeadOfFactorId)));
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
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult PrintFactor(Guid headoffactorid)
        {
            List<Models.DetailOfFactor> list = null;
            try
            {
                var headRow =
                     UnitOfWork.HeadOfFactorRepository.Get()
                     .Where(current => current.Id == headoffactorid)
                     .FirstOrDefault()
                     ;

                ViewBag.headRow = headRow;
                var row =
                     UnitOfWork.DetailOfFactorRepository.Get()
                     .Where(current => current.HeadOfFactorId == headoffactorid)
                     .FirstOrDefault()
                     ;

                var amount =
                     UnitOfWork.DetailOfFactorRepository.Get()
                     .Where(current => current.HeadOfFactorId == headoffactorid)
                     .Sum(x => x.ServiceTariff.Amount * x.CommodityCount * x.CurrencyRatio)
                     ;

                var CurrencyRation = UnitOfWork.DetailOfFactorRepository.Get()
                     .Where(current => current.HeadOfFactorId == headoffactorid).FirstOrDefault().CurrencyRatio;


                var InvoiceNumber = row.HeadOfFactor.InvoiceNumber.ToString();

                Models.Request oRequest = UnitOfWork.RequestRepository.Get()
                    .Where(x => x.IsActived == true)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.RecordNumber == InvoiceNumber)
                    .Where(x => x.CompanyNationalCode == row.HeadOfFactor.CompanyNationalCode)
                    .FirstOrDefault()
                    ;

                if (oRequest == null)
                {
                    oRequest = new Models.Request();
                    oRequest.AmountPaid = Convert.ToInt64(amount);
                    oRequest.BaseCurrencyValue = oRequest.AmountPaid;
                    oRequest.CellPhoneNumber = row.HeadOfFactor.CellPhoneNumber;
                    oRequest.CityId = row.HeadOfFactor.CityId;
                    oRequest.CommodityType = row.CommodityDescription;
                    oRequest.CommodityUnit = row.CommodityDescription;
                    oRequest.CompanyName = row.HeadOfFactor.CompanyName;
                    oRequest.CompanyNationalCode = row.HeadOfFactor.CompanyNationalCode;
                    oRequest.CurrencyCode = Convert.ToInt32(Enums.CurrencyUnits.Rails);
                    //oRequest.Ratio = row.CurrencyUnit.Ratio; // نرخ ارز
                    oRequest.CurrencyRation = CurrencyRation;
                    oRequest.CurrencyValue = oRequest.AmountPaid;
                    oRequest.Description = row.HeadOfFactor.Description;
                    oRequest.HeadOfFactor = row.HeadOfFactor;
                    oRequest.ImportRecordNumber = row.HeadOfFactor.InvoiceNumber.ToString();
                    oRequest.InvoiceDate = DateTime.Now;
                    oRequest.InsertDateTime = DateTime.Now;
                    oRequest.IsActived = true;
                    oRequest.IsDeleted = false;
                    oRequest.IsSystem = true;
                    oRequest.IsVerified = true;
                    oRequest.PerformDate = DateTime.Now.Date.ToShortDateString();
                    oRequest.PerformNumber = row.HeadOfFactor.InvoiceNumber.ToString();
                    oRequest.Province = row.HeadOfFactor.Province;
                    oRequest.ProvinceId = row.HeadOfFactor.ProvinceId;
                    oRequest.RecordDate = Infrastructure.Utility.Persion(DateTime.Now);
                    oRequest.RecordNumber = row.HeadOfFactor.InvoiceNumber.ToString() + "000";
                    oRequest.RequestState = Convert.ToInt32(Enums.RequestStates.PaymentOrder);
                    oRequest.ServiceTariff = row.ServiceTariff;
                    oRequest.ServiceTariffId = row.ServiceTariffId;
                    //TODO: مشکل زیر سیستم داریم
                    var subSystem = new Models.SubSystem();
                    //               if (headRow.HeadLineId == Guid.Parse("00F8057F-08F6-11E9-92CE-0050568D5B96")) // واریز وجه مزایده
                    //               {
                    //                   subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000011")); // واریز وجه مزایده
                    //               }
                    //               else if(headRow.HeadLineId == Guid.Parse("8AEC4470-02AB-11E9-92CE-0050568D5B96")) // خانه های سازمانی
                    //               {
                    //                   subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000021")); // خانه های سازمانی
                    //               }
                    //else if (headRow.HeadLineId == Guid.Parse("1A8766A4-9FF7-4D58-85B2-C0BA280901CE")) // اعتبارات مصرف نشده
                    //               {
                    //                   subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("ed6ed479-6dbb-4e2b-9243-008a8000a573")); // اعتبارات مصرف نشده
                    //               }
                    //               else
                    //               {
                    //                   subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000001")); // تبصره 23
                    //               }

                    #region test
                    if (headRow.HeadLineId == Guid.Parse("BF70AE2A-5FBF-11E7-8319-C0F8DABA7555")) // 1 - تعرفه‌های مربوط به توليدات،‌ واردات و كنترل كيفيت دارو
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000777")); // ------خدمات دامپزشکی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("05AEFD36-5FC0-11E7-8319-C0F8DABA7555")) // 2 - تعرفه های مربوط به بازدید ، صدور ، تمدید ، تبدیل و نقل و انتقال پروانه های مختلف درمانی، آزمایشگاهی، داروخانه و مراکز مایه کوبی
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000777")); // ---------خدمات دامپزشکی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("403AA492-5FC0-11E7-8319-C0F8DABA7555")) // 3 - خدمات درمانگاهی دامپزشکی
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000777")); // -----خدمات دامپزشکی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("549B48DE-5FC0-11E7-8319-C0F8DABA7555")) // 4 - خدمات آزمایشگاهی دامپزشکی
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000777")); // --------خدمات دامپزشکی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("69E27263-5FC0-11E7-8319-C0F8DABA7555")) // 5 - خدمات بهداشتی دامپزشکی
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000777")); // ------خدمات دامپزشکی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("549B48DE-5FC0-11E7-8319-C0F8DABA7755")) // 6 - تبصره 23
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000001")); //---------تبصره 23
                    }
                    else if (headRow.HeadLineId == Guid.Parse("1A8766A4-9FF7-4D58-85B2-C0BA280901CE")) // 7 - اعتبارات مصرف نشده
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000333")); // -----سایر درآمدها
                    }
                    else if (headRow.HeadLineId == Guid.Parse("8AEC4470-02AB-11E9-92CE-0050568D5B96")) // 8 - وجوه خانه های سازمانی(اجاره)
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000333")); // -------سایر درآمدها
                    }
                    else if (headRow.HeadLineId == Guid.Parse("00F8057F-08F6-11E9-92CE-0050568D5B96")) // 9 - واریز وجوه مزایده
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000333")); // -----سایر درآمدها
                    }
                    else if (headRow.HeadLineId == Guid.Parse("549b48de-5fc0-11e7-8319-c0f8daba7723")) // 10 - درآمد حاصل از کشتار دام و طیور در کشتارگاهها موضوع ردیف 160125
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000555")); // -----ماده پنج نظارت شرعی
                    }
                    else if (headRow.HeadLineId == Guid.Parse("BAB2636A-FCB5-11EA-9F5E-0050568D5B96")) // 17 - پرداخت پروانه ها  - CERT
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("C3001C61-FC9B-11EA-9F5E-0050568D5B96")); // -----پروانه ها
                    }
                    else
                    {
                        subSystem = UnitOfWork.SubSystemRepository.GetById(Guid.Parse("00000000-0000-0000-0000-000000000333")); // سایر درآمدها
                    }
                    #endregion
                    oRequest.SubSystemId = subSystem.Id;
                    /////////////////////////////////////////////
                    oRequest.UserIPAddress = Request.UserHostAddress;
                    oRequest.Browser = Request.Browser.Type; // مدل و ورژن مرورگر
                    oRequest.TotalValue = 0;
                    oRequest.UpdateDateTime = DateTime.Now;
                    oRequest.URLAddress = UnitOfWork.SubSystemRepository.Get().FirstOrDefault().UrlTo;
                    oRequest.User = row.HeadOfFactor.User;
                    oRequest.UserId = row.HeadOfFactor.UserId;

                    UnitOfWork.RequestRepository.Insert(oRequest); // شناسه واریز
                }

                headRow.FinalApprove = true;
                UnitOfWork.Save();

                list =
                    UnitOfWork.DetailOfFactorRepository.Get(headoffactorid)
                    .ToList();

                var File = new Rotativa.MVC.ViewAsPdf("PrintFactor", list)
                {
                    FileName = list.FirstOrDefault().HeadOfFactor.CompanyName + "-" + list.FirstOrDefault().HeadOfFactor.InvoiceNumber + ".pdf"
                };

                try
                {
                    #region Insert New Message
                    Models.Message oMessage = new Models.Message();
                    oMessage.UserId = Infrastructure.Sessions.AuthenticatedUser.Id;
                    oMessage.LastState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.MessageText =
                        Resources.Message.Request.Message_InitialRequet;
                    oMessage.NewState = (int)Enums.RequestStates.InitialRequet;
                    oMessage.RequestId = oRequest.Id;
                    UnitOfWork.MessageRepository.Insert(oMessage);
                    UnitOfWork.Save();
                    #endregion
                }
                catch
                {
                }

                return File;
            }

            catch (Exception ex)
            {
                Utilities.Net.LogHandler.Report(GetType(), null, ex);
                return (RedirectToAction(MVC.Error.Display(System.Net.HttpStatusCode.BadRequest)));
            }
        }

    }
}
