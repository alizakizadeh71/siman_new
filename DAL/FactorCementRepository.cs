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

        public int GetLastInvoiceNumber()
        {
            try
            {
                IQueryable<Models.FactorCement> retValue;

                int LastInvoiceNumber = Get()
                     .OrderByDescending(x => x.InvoiceNumber).Select(x => x.InvoiceNumber.Value).FirstOrDefault();

                if (LastInvoiceNumber == 0)
                {
                    return 1000;
                }

                return LastInvoiceNumber;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Models.FactorCement> GetByinvoicenumber(int invoicenumber)
        {
            try
            {
                IQueryable<Models.FactorCement> retValue = Get()
                         .Where(current => current.InvoiceNumber == invoicenumber)
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true);
                return retValue;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<Models.FactorCement> GetByAuthority(string authority)
        {
            try
            {
                IQueryable<Models.FactorCement> retValue = Get()
                         .Where(current => current.Authority == authority)
                         .Where(current => current.IsDeleted == false)
                         .Where(current => current.IsActived == true);
                return retValue;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<Models.FactorCement> GetByUser(Models.User user)
        {
            try
            {
                IQueryable<Models.FactorCement> retValue;
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
