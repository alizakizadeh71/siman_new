using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ViewModels.Areas.Administrator.TotalCompanyInventory;
using ViewModels.Areas.Administrator.Village;

namespace OPS.Areas.Administrator.Controllers
{
    public partial class TotalCompanyInventoryController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [System.Web.Mvc.HttpGet]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.ProvinceExpert00)]
        public virtual ActionResult Index()
        {
            TotalCompanyInventoryViewModel TotalCompanyInventoryViewModel = new TotalCompanyInventoryViewModel();
            Viewdata(TotalCompanyInventoryViewModel);
            return View(TotalCompanyInventoryViewModel);
        }

        private void Viewdata(TotalCompanyInventoryViewModel TotalCompanyInventoryViewModel)
        {
            var Users = UnitOfWork.UserRepository.Get().Where(x => x.IsActived && !x.IsDeleted).ToList();
            base.ViewData["Users"] = new System.Web.Mvc.SelectList(Users, "Id", "Title", TotalCompanyInventoryViewModel.UserId);

            var Banks = UnitOfWork.BankRepository.Get().ToList();
            base.ViewData["Province"] = new System.Web.Mvc.SelectList(Banks, "Id", "Name", null);

            var Inventorytonnage = UnitOfWork.InventoryamountRepository.Get().ToList();
            ViewData["City"] = new System.Web.Mvc.SelectList(Inventorytonnage, "Id", "Name", null);
        }

        [System.Web.Mvc.HttpPost]
        [Infrastructure.SyncPermission(isPublic: false, role: Enums.Roles.MaliAdminGholami)]
        public virtual System.Web.Mvc.JsonResult GetRequests() => (JsonResult)Search(null);

        public virtual System.Web.Mvc.ActionResult Search(ViewModels.Areas.Administrator.TotalCompanyInventory.TotalCompanyInventoryViewModel viewModel)
        {
            bool Search = false;

            var totalBalance = UnitOfWork.UserRepository.Get()
                .Where(u => u.IsActived && !u.IsDeleted)
                .Sum(u => (long)u.InitialCredit - (long)u.creditAmount);

            var totalValue = (from i in UnitOfWork.InventoryamountRepository.Get()
                              join f in UnitOfWork.FinancialManagementRepository.Get()
                                  on new { i.ProductNameId, i.ProductTypeId, i.PackageTypeId, i.FactoryNameId }
                                  equals new { f.ProductNameId, f.ProductTypeId, f.PackageTypeId, f.FactoryNameId }
                              where i.IsDeleted == false && f.IsDeleted == false && i.Inventorytonnage != 0
                              select i.Inventorytonnage * f.AmountPaid).Sum();


            var TotalBankAmount = UnitOfWork.BankRepository.Get()
                .Where(b => b.IsActived && !b.IsDeleted)
                .Sum(b => (long)b.Balance);
            try
            {
                var viewModelList = new List<TotalCompanyInventoryViewModel>
                {
                    new TotalCompanyInventoryViewModel()
                    {
                        InventoryUsersString = totalBalance.ToString("N0") + "ريال",
                        InventorytonnageString = totalValue.ToString("N0") + "ريال",
                        TotalBankamount = TotalBankAmount.ToString("N0") + "ريال",
                        PriceInventory = (totalBalance + totalBalance + totalBalance).ToString("N0") + "ریال"
                    }
                }.AsQueryable();


                var varResult =
                    Utilities.Kendo.HtmlHelpers
                        .ParseGridData<ViewModels.Areas.Administrator.TotalCompanyInventory.TotalCompanyInventoryViewModel>(viewModelList);

                return (Json(varResult, System.Web.Mvc.JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}