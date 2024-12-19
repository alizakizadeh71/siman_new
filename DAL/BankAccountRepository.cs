using System.Linq;

namespace DAL
{
    public class BankAccountRepository : Repository<Models.BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.BankAccount> GetBankAccounts()
        {
            IQueryable<Models.BankAccount> list = null;
            list = Get();
            return list;
        }

    }
}
