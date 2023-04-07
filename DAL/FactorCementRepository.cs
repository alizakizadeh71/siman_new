using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using Models;

namespace DAL
{
    public class FactorCementRepository : Repository<Models.FactorCement>, IFactorCementRepository
    {
        public FactorCementRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.FactorCement> Get(Models.User user)
        {
            try
            {
                IQueryable<Models.FactorCement> retValue;

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

        public void Insertdata(Models.FactorCement factorCement)
        {
            try
            {
                DatabaseContext.FactorCements.Add(factorCement);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
