using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class PackageTypeRepository : Repository<Models.PackageType>, IPackageTypeRepository
    {
        public PackageTypeRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
        public IQueryable<Models.PackageType> GetSubHeadLines()
        {
            IQueryable<Models.PackageType> list = null;
            list = Get();
            return list;
        }

        public Models.PackageType GetByCode(string code)
        {
            Models.PackageType oSubHeadLine =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oSubHeadLine;
        }


        public IQueryable<Models.PackageType> GetByProductTypeId(System.Guid ProductTypeId)
        {
            try
            {
                IQueryable<Models.PackageType> retValue;

                retValue
                    = Get()
                    .Where(current => current.ProductTypeId == ProductTypeId);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
	}
}
