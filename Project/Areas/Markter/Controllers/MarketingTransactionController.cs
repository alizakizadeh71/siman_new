using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Models;
using ViewModels.Areas.Administrator.TotalCompanyInventory;

namespace OPS.Areas.Markter.Controllers
{
    public partial class MarketingTransactionController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Markter/MarketingTransaction
        [System.Web.Mvc.HttpGet]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Marketer)]
        public virtual ActionResult Index()
        {
            MarketerTransactions marketerTransactions = new MarketerTransactions();
            Viewdata(marketerTransactions);
            ViewBag.PageMessages = null;
            return View(marketerTransactions);
        }

        private void Viewdata(MarketerTransactions MarketerTransactions)
        {
            var Users = UnitOfWork.UserRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["Users"] = new SelectList(Users, "Id", "Title", MarketerTransactions.MarketingCode);

            var ProductName = UnitOfWork.ProductNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["ProductName"] = new SelectList(Users, "Id", "Title", MarketerTransactions.ProductNameId);

            var PackageType = UnitOfWork.PackageTypeRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["PackageType"] = new SelectList(Users, "Id", "Title", MarketerTransactions.PackageTypeId);

            var FactoryName = UnitOfWork.FactoryNameRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["FactoryName"] = new SelectList(Users, "Id", "Title", MarketerTransactions.FactoryNameId);
        }


        [System.Web.Mvc.HttpPost]
        //[Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.Marketer)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.TotalCompanyInventory.TotalCompanyInventoryViewModel viewModel)
        {
            bool Search = false;
            var marketingCode = Session["MarketingCode"] as int?;
            if (!marketingCode.HasValue)
            {
                return Json(new
                {
                    Success = false,
                    Message = "کاربر کد بازاریابی ندارد"
                }, JsonRequestBehavior.AllowGet);
            }


            var varRequest = UnitOfWork.MarketerTransactionsRepository.GetTransactions(marketingCode.Value);
            try
            {
                var ViewModelsvarBanks
                    = varRequest.OrderByDescending(current => current.InsertDateTime)
                        .ToList()
                        .Select(current =>
                            new ViewModels.Areas.Markter.MarketingTransaction.MarketingTransactionViewModel()
                            {
                                ProductNameString = current.ProductName.Name,
                                ProductTypeString = current.ProductType.Name,
                                PackageTypeString = current.PackageType.Name,
                                FactoryNameString = current.FactoryName.Name,
                                Tonnage = current.Tonnagedouble.ToString(),
                                CommissionAmount = current.CommissionAmount.ToString()
                            })
                        .AsQueryable();

                var varResult =
                    Utilities.Kendo.HtmlHelpers
                        .ParseGridData<ViewModels.Areas.Markter.MarketingTransaction.MarketingTransactionViewModel>(ViewModelsvarBanks);

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}