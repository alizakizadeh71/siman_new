using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
        public List<Inventoryamount> GetByProductId(List<Guid> productIdList)
        {
            try
            {
                var list = DatabaseContext.Inventoryamount
                    .Where(x => productIdList.Contains(x.ProductNameId)
                                && x.Inventorytonnage > 0 
                                && x.IsActived != false
                                && x.IsDeleted != true)
                    .ToList();

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
