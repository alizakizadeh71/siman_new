using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class RoleRepository : Repository<Models.Role>, IRoleRepository
	{
		public RoleRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
	}
}
