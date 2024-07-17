using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IInventoryamountRepository : IRepository<Models.Inventoryamount>
    {
        System.Linq.IQueryable<Models.Inventoryamount> GetInventoryamount();

        Models.Inventoryamount GetById(Guid Id);
        void Insertdata(Models.Inventoryamount Inventoryamount);
    }
}
