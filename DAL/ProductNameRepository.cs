using System.Linq;
using System.Data.Entity;

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
            list = Get();
            return list;
        }

        public Models.ProductName GetByCode(string code)
        {
            Models.ProductName oProductName =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oProductName;
        }
	}
}
