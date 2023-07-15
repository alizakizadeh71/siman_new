using System.Linq;
using System.Data.Entity;
using Models;
using System;

namespace DAL
{
    public class ProvinceRepository : Repository<Models.Province>, IProvinceRepository
	{
		public ProvinceRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public Models.Province GetByCode(string code)
        {
            Models.Province oProvince =
                Get()
                .Where(current =>current.Code==code)
                .FirstOrDefault();

            return (oProvince);
        }

        public IQueryable<Models.Province> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.Province> retValue;

                if (user.Role.Code < (int)Enums.Roles.MaliExpert01)
                {
                    retValue
                        = Get()
                        .Where(current => current.Id == user.ProvinceId);
                }

                else
                {
                    retValue
                        = Get()
                        .OrderBy(current=>current.Name);
                }

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Updatedata(Province province)
        {
            try
            {
                DatabaseContext.Entry(province).State = EntityState.Modified;
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
