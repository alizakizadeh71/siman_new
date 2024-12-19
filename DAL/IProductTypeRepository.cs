namespace DAL
{
    public interface IProductTypeRepository : IRepository<Models.ProductType>
    {

        System.Linq.IQueryable<Models.ProductType> GetProductTypes();

        Models.ProductType GetByCode(string code);
        System.Linq.IQueryable<Models.ProductType> GetByProductNameId(System.Guid ProductNameId);
        void Insertdata(Models.ProductType productType);
    }
}
