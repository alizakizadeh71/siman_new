using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class HeadOfFactorRepository : Repository<Models.HeadOfFactor>, IHeadOfFactorRepository
    {
        public HeadOfFactorRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.HeadOfFactor> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.HeadOfFactor> retValue;

                if (user.Role.Code <= (int)Enums.Roles.ProvinceExpert00)
                {
                    retValue = Get()
                        .Where(current => current.IsDeleted == false)
                        .Where(current => current.IsActived == true)
                        .Where(current => current.ProvinceId == user.ProvinceId);
                }
				if (user.Role.Code == (int)Enums.Roles.ExporterOFInvoice)
				{
					retValue = Get()
						.Where(current => current.IsDeleted == false)
						.Where(current => current.IsActived == true)
						.Where(current => current.UserId == user.Id);
				}

				else
                    retValue = Get()
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true);

                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
