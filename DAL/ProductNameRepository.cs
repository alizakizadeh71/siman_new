using System.Linq;
using System.Data.Entity;
using Models;
using System;

namespace DAL
{
    public class ProductNameRepository : Repository<Models.ProductName>, IProductNameRepository
    {
        public ProductNameRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.ProductName> GetProductNames()
        {
            IQueryable<Models.ProductName> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        public Models.ProductName GetByCode(string code)
        {
            Models.ProductName oProductName =
                Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oProductName;
        }

        public void Insertdata(ProductName productName)
        {
            try
            {
                DatabaseContext.ProductNames.Add(productName);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void save()
        {
            //DatabaseContext.SaveChanges();
        }
    }
}
