using System;

namespace DAL
{
    public interface IInventoryamountRepository : IRepository<Models.Inventoryamount>
    {
        System.Linq.IQueryable<Models.Inventoryamount> GetInventoryamount();

        Models.Inventoryamount GetById(Guid Id);
        void Insertdata(Models.Inventoryamount Inventoryamount);
    }
}
