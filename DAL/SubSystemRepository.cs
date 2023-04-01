using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class SubSystemRepository : Repository<Models.SubSystem>, ISubSystemRepository
	{
		public SubSystemRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public Models.SubSystem GetByCode(int code)
        {
            Models.SubSystem oSubSystem =
                Get()
                .Where(current => current.Code == code)
                .FirstOrDefault();

            return (oSubSystem);
        }
	}
}
