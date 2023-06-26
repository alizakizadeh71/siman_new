using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using Models;

namespace DAL
{
    public class DestinationManagementRepository : Repository<Models.DestinationManagement>, IDestinationManagementRepository
    {
        public DestinationManagementRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.DestinationManagement> GetByUser(Models.User user)
        {
            try
            {
                IQueryable<Models.DestinationManagement> retValue;
                if (user.Role.Code >= 1000) /// اگر ادمین بود تمام درخواست ها نمایش داده شود
                {
                    retValue = Get()
                     .Where(current => current.IsDeleted == false)
                     .Where(current => current.IsActived == true)
                     .Where(current => current.FinancialManagement.IsDeleted == false)
                     .Where(current => current.FinancialManagement.IsActived == true);
                }
                else
                {
                    retValue = Get()
                         //.Where(current => current.UserId == user.Id)
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true)
                         .Where(current => current.FinancialManagement.IsDeleted == false)
                         .Where(current => current.FinancialManagement.IsActived == true);
                }
                return retValue;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(Models.DestinationManagement DestinationManagement)
        {
            try
            {
                DatabaseContext.DestinationManagements.Add(DestinationManagement);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
