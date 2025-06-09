using Models;
using System;
using System.Collections.Generic;
using ViewModels.Areas.Administrator.Inventoryamount;

namespace DAL
{
    public interface IInventoryamountRepository : IRepository<Models.Inventoryamount>
    {
        System.Linq.IQueryable<Models.Inventoryamount> GetInventoryamount();

        Models.Inventoryamount GetById(Guid Id);
        void Insertdata(Models.Inventoryamount Inventoryamount);
       List<Inventoryamount> GetByProductId(List<InventoryViewModel> productIdList);
    }
}
