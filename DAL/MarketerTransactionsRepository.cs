using Models;
using System;
using System.Linq;

namespace DAL
{
    public class MarketerTransactionsRepository : Repository<Models.MarketerTransactions>, IMarketerTransactionsRepository
    {
        public MarketerTransactionsRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }
        public IQueryable<Models.MarketerTransactions> GetTransactions(int MarktingCode)
        {
            IQueryable<Models.MarketerTransactions> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted && x.MarketingCode == MarktingCode);
            return list;
        }


        public void Insertdata(MarketerTransactions Transactions)
        {
            try
            {
                DatabaseContext.MarketerTransactions.Add(Transactions);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
