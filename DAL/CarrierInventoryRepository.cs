using Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class CarrierInventoryRepository : Repository<Models.CarrierInventory>, ICarrierInventoryRepository
    {
        public CarrierInventoryRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<CarrierInventory> GetCarrierInventories()
        {
            return Get()
                .Where(x => !x.IsDeleted && x.IsActived)
                .Include(x => x.ProductName)
                .Include(x => x.ProductType)
                .Include(x => x.PackageType)
                .Include(x => x.FactoryName)
                .Include(x => x.Carrier);
        }

        public IQueryable<CarrierInventory> GetByProduct(Guid productNameId, Guid productTypeId, Guid packageTypeId, Guid factoryNameId)
        {
            return GetCarrierInventories()
                .Where(x =>
                    x.ProductNameId == productNameId &&
                    x.ProductTypeId == productTypeId &&
                    x.PackageTypeId == packageTypeId &&
                    x.FactoryNameId == factoryNameId);
        }

        public CarrierInventory GetDefaultCarrier(Guid productNameId, Guid productTypeId, Guid packageTypeId, Guid factoryNameId)
        {
            return GetByProduct(productNameId, productTypeId, packageTypeId, factoryNameId)
                .FirstOrDefault(x => x.IsDefaultCarrier);
        }

        public void Insertdata(CarrierInventory carrierInventory)
        {
            try
            {
                DatabaseContext.CarrierInventories.Add(carrierInventory);
                DatabaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void save()
        {
            DatabaseContext.SaveChanges();
        }
    }
}