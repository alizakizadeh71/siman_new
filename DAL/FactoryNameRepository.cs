using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class FactoryNameRepository : Repository<Models.FactoryName>, IFactoryNameRepository
    {
        public FactoryNameRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.FactoryName> GetFactoryNames()
        {
            IQueryable<Models.FactoryName> list = Get();
            return list;
        }

        public Models.FactoryName GetByCode(string code)
        {
            Models.FactoryName oFactoryName =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oFactoryName;
        }

        public IQueryable<Models.FactoryName> GetByProductNameId(System.Guid ProductNameId)
        {
            try
            {
                IQueryable<Models.FactoryName> retValue;

                retValue
                    = Get()
                    .Where(current => current.ProductNameId == ProductNameId);


                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
