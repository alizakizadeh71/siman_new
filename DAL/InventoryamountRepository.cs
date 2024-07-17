using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
        public class InventoryamountRepository : Repository<Models.Inventoryamount>, IInventoryamountRepository
        {
        public InventoryamountRepository(Models.DatabaseContext databaseContext)
: base(databaseContext)
        {
        }


        public IQueryable<Inventoryamount> GetInventoryamount()
        {
            IQueryable<Models.Inventoryamount> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        //public Inventoryamount GetById(Guid Id)
        //{
        //    Models.Inventoryamount oInventoryamoun =
        //        Get()
        //        .Where(x => x.IsActived && !x.IsDeleted)
        //        .Where(currenct => currenct.Id == Id)
        //        .FirstOrDefault();

        //    return oInventoryamoun;
        //}

        public void Insertdata(Inventoryamount Inventoryamount)
        {
            try
            {
                DatabaseContext.Inventoryamount.Add(Inventoryamount);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
