namespace DAL
{
    public interface ICarrierInventoryRepository : IRepository<Models.CarrierInventory>
    {
        System.Linq.IQueryable<Models.CarrierInventory> GetCarrierInventories();

        System.Linq.IQueryable<Models.CarrierInventory> GetByProduct(
            System.Guid productNameId,
            System.Guid productTypeId,
            System.Guid packageTypeId,
            System.Guid factoryNameId);

        Models.CarrierInventory GetDefaultCarrier(
            System.Guid productNameId,
            System.Guid productTypeId,
            System.Guid packageTypeId,
            System.Guid factoryNameId);

        void Insertdata(Models.CarrierInventory carrierInventory);

        void save();
    }
}