using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class AccountNumberManageRepository : Repository<Models.AccountNumberManage>, IAccountNumberManageRepository
	{
        public AccountNumberManageRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
	}
}
