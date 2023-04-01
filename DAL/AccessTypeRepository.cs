using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class AccessTypeRepository : Repository<Models.AccessType>, IAccessTypeRepository
	{
		public AccessTypeRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}
	}
}
