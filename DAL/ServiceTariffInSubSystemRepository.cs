using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class ServiceTariffInSubSystemRepository : Repository<Models.ServiceTariffInSubSystem>, IServiceTariffInSubSystemRepository
	{
        public ServiceTariffInSubSystemRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

	}
}
