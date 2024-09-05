using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using Models;

namespace DAL
{
    public class walletFactorRepository : Repository<Models.walletFactor>, IwalletFactorRepository
    {
        public walletFactorRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public int GetLastInvoiceNumber()
        {
            try
            {
                IQueryable<Models.walletFactor> retValue;

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

        public IQueryable<Models.walletFactor> GetByinvoicenumber(int invoicenumber)
        {
            try
            {
                IQueryable<Models.walletFactor> retValue = Get()
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
        public IQueryable<Models.walletFactor> GetByAuthority(string authority)
        {
            try
            {
                IQueryable<Models.walletFactor> retValue = Get()
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
        public IQueryable<Models.walletFactor> GetByUser(Models.User user)
        {
            try
            {
                IQueryable<Models.walletFactor> retValue;
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

        public void Insertdata(Models.walletFactor walletFactor)
        {
            try
            {
                DatabaseContext.walletFactor.Add(walletFactor);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
