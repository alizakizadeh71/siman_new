using Models;
using System;
using System.Linq;

namespace DAL
{
    public class FinancialManagementRepository : Repository<Models.FinancialManagement>, IFinancialManagementRepository
    {
        public FinancialManagementRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.FinancialManagement> GetByUser(Models.User user)
        {
            try
            {
                IQueryable<Models.FinancialManagement> retValue;
                if (user.Role.Code >= 1000) /// اگر ادمین بود تمام درخواست ها نمایش داده شود
                {
                    retValue = Get()
                     .Where(current => current.IsDeleted == false)
                     .Where(current => current.IsActived == true);
                }
                else
                {
                    retValue = Get()
                         .Where(current => current.UserId == user.Id)
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true);
                }
                return retValue;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Insertdata(Models.FinancialManagement FinancialManagement)
        {
            try
            {
                DatabaseContext.FinancialManagements.Add(FinancialManagement);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
