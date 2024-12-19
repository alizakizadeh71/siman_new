using Models;
using System;
using System.Linq;

namespace DAL
{
    public class VillageRepository : Repository<Models.village>, IVillageRepository
    {
        public VillageRepository(Models.DatabaseContext databaseContext)
        : base(databaseContext)
        {
        }

        public village GetByCode(string code)
        {
            Models.village oVillage =
                Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .Where(currenct => currenct.Id.ToString() == code)
                .FirstOrDefault();

            return oVillage;
        }


        public IQueryable<village> GetVillageName()
        {
            IQueryable<Models.village> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        public IQueryable<Models.village> GetBycityId(System.Guid cityId)
        {
            try
            {
                IQueryable<Models.village> retValue;

                retValue
                    = Get()
                    //.Where(x => x.IsActived && !x.IsDeleted)
                    .Where(x => !x.IsDeleted)
                    .Where(current => current.Cityid == cityId);


                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(village village)
        {
            try
            {
                DatabaseContext.villages.Add(village);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
