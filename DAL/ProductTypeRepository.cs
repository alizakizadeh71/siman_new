using Models;
using System;
using System.Linq;

namespace DAL
{
    public class ProductTypeRepository : Repository<Models.ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.ProductType> GetProductTypes()
        {
            IQueryable<Models.ProductType> list = null;
            list = Get();
            return list;
        }

        public Models.ProductType GetByCode(string code)
        {
            Models.ProductType oProductType =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oProductType;
        }

        public IQueryable<Models.ProductType> GetByProductNameId(System.Guid ProductNameId)
        {
            try
            {
                IQueryable<Models.ProductType> retValue;

                retValue
                    = Get()
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Where(current => current.ProductNameId == ProductNameId);


                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(ProductType productType)
        {
            try
            {
                DatabaseContext.ProductTypes.Add(productType);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
