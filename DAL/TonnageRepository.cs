using Models;
using System;
using System.Linq;

namespace DAL
{
    public class TonnageRepository : Repository<Models.Tonnage>, ITonnageRepository
    {
        public TonnageRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }
        public IQueryable<Models.Tonnage> GetTonnages()
        {
            IQueryable<Models.Tonnage> list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        public Models.Tonnage GetByCode(string code)
        {
            Models.Tonnage oTonnage =
                Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oTonnage;
        }


        public IQueryable<Models.Tonnage> GetByPackageTypeId(System.Guid PackageTypeId)
        {
            try
            {
                IQueryable<Models.Tonnage> retValue;

                retValue
                    = Get()
                    .Where(x => x.IsActived && !x.IsDeleted)
                    .Where(current => current.PackageTypeId == PackageTypeId);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(Tonnage tonnage)
        {
            try
            {
                DatabaseContext.Tonnages.Add(tonnage);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
