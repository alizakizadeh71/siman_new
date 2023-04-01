using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class AccountNumberRepository : Repository<Models.AccountNumber>, IAccountNumberRepository
	{
		public AccountNumberRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
	}
}
