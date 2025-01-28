using System.Linq;

namespace DAL
{
    public class BankRepository : Repository<Models.Bank>, IBankRepository
    {
        public BankRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.Bank> GetBanks()
        {
            IQueryable<Models.Bank> list = null;
            list = Get();
            return list;
        }

        public Models.Bank GetByName(string Name)
        {
            Models.Bank oBank =
                Get()
                .Where(currenct => currenct.Name == Name)
                .FirstOrDefault();

            return oBank;
        }
    }
}
