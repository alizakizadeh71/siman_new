using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ViewModels.Areas.Administrator.Inventoryamount;

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
        public List<Inventoryamount> GetByProductId(List<InventoryViewModel> productIdList)
        {
            try
            {
                var data = DatabaseContext.Inventoryamount
                    .Where(x => x.Inventorytonnage > 0 && x.IsActived != false && x.IsDeleted != true)
                    .ToList(); // دریافت اولیه از دیتابیس

                var filtered = data
                    .Where(x => productIdList.Any(p =>
                        p.ProductNameId == x.ProductNameId &&
                        p.ProductTypeId == x.ProductTypeId &&
                        p.PackageType == x.PackageTypeId &&
                        p.FactoryNameId == x.FactoryNameId))
                    .ToList();


                return filtered;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
