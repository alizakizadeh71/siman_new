using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class CityRepository : Repository<Models.City>, ICityRepository
	{
        public CityRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public Models.City GetByCode(string code , System.Guid ProvinceId)
        {
            Models.City oCity =
                Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .Where(current =>current.Code==code)
                .Where(current =>current.ProvinceId== ProvinceId)
                .FirstOrDefault();

            return (oCity);
        }

        public IQueryable<Models.City> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.City> retValue;

                if (user.Role.Code < (int)Enums.Roles.SoperAdmin)
                {
                    retValue
                        = Get()
                        .Where(x => x.IsActived && !x.IsDeleted)
                        .Where(current => current.ProvinceId == user.ProvinceId);
                }

                else
                {
                    retValue
                        = Get()
                        .Where(x => x.IsActived && !x.IsDeleted)
                        .OrderBy(current=>current.Name);
                }

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Models.City> GetByProvinceId(System.Guid provinceId)
        {
            try
            {
                IQueryable<Models.City> retValue;

                    retValue
                        = Get()
                        //.Where(x => x.IsActived && !x.IsDeleted)
                        .Where(x => !x.IsDeleted)
                        .Where(current => current.ProvinceId == provinceId);
               

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
	}
}
