using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class AccountNumberManageLogRepository : Repository<Models.AccountNumberManageLog>, IAccountNumberManageLogRepository
	{
		public AccountNumberManageLogRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
	}
}
