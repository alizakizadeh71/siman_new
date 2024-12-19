using Models;
using System;
using System.Linq;

namespace DAL
{
    public class PackageTypeRepository : Repository<Models.PackageType>, IPackageTypeRepository
    {
        public PackageTypeRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }
        public IQueryable<Models.PackageType> GetPackageTypes()
        {
            IQueryable<Models.PackageType> list = Get();
            return list;
        }

        public Models.PackageType GetByPackageTypeCode(string code)
        {
            Models.PackageType oPackageType =
                Get().Where(x => x.IsActived && !x.IsDeleted)
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oPackageType;
        }


        public IQueryable<Models.PackageType> GetByProductTypeId(System.Guid ProductTypeId)
        {
            try
            {
                IQueryable<Models.PackageType> retValue;

                retValue
                    = Get().Where(x => x.IsActived && !x.IsDeleted)
                    .Where(current => current.ProductTypeId == ProductTypeId);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(PackageType packageType)
        {
            try
            {
                DatabaseContext.PackageTypes.Add(packageType);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
