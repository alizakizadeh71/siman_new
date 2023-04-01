using System.Linq;
using System.Data.Entity;
using System;

namespace DAL
{
	public class UserRepository : Repository<Models.User>, IUserRepository
	{
		public UserRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public Models.User GetByUserName(string username)
        {
            try
            {
                Models.User oUser =
                    Get()
                    .Where(current => string.Compare(current.UserName, username,
                        System.StringComparison.InvariantCultureIgnoreCase) == 0)
                    .FirstOrDefault();

                return (oUser);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public IQueryable<Models.User> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.User> retValue;

                if ((int)user.Role.Code == (int)Enums.Roles.ProvinceExpert00)
                {
                    retValue 
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum)
                        .Where(current => current.ProvinceId == user.ProvinceId);
                }

                else if ((int)user.Role.Code == (int)Enums.Roles.SoperAdmin)
                {
                    retValue 
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum);
                }

                else
                {
                    retValue
                        = Get()
                        .Where(current => current.Role.Code <= (int)user.Role.CodeEnum);
                }
                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
	}
}
