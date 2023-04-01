using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class ServiceTariffRepository : Repository<Models.ServiceTariff>, IServiceTariffRepository
	{
        public ServiceTariffRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.ServiceTariff> GetServiceTariffs()
        {
            IQueryable<Models.ServiceTariff> list = null;
            list = Get();
            return list;
        }
        public IQueryable<Models.ServiceTariff> GetBySubSystemId(System.Guid subsystemId)
        {
            var listas =
                DatabaseContext.ServiceTariffInSubSystems
                .Where(x => !x.IsDeleted)
                .Where(x => x.IsActived)
                .Where(x => !x.ServiceTariff.IsDeleted)
                .Where(x => x.ServiceTariff.IsActived)
                .Where(x => x.SubSystemId == subsystemId)
                .Select(x => x.ServiceTariff)
                .ToList()
                ;

            IQueryable<Models.ServiceTariff> list = 
                DatabaseContext.ServiceTariffInSubSystems
                .Where(x=>!x.IsDeleted)
                .Where(x => x.IsActived)
                .Where(x => !x.ServiceTariff.IsDeleted)
                .Where(x=>x.ServiceTariff.IsActived)
                .Where(x => x.SubSystemId == subsystemId)
                .Select(x=>x.ServiceTariff)
                .ToList()
                .AsQueryable()
                ;

            return list;
        }

        public Models.ServiceTariff GetByRCode(string code)
        {
            Models.ServiceTariff oServiceTariff =
                Get()
                .Where(currenct => currenct.RCode == code)
                .FirstOrDefault();

            return oServiceTariff;
        }
        public Models.ServiceTariff GetByVCode(string code)
        {
            Models.ServiceTariff oServiceTariff =
                Get()
                .Where(currenct => currenct.VCode == code)
                .FirstOrDefault();

            return oServiceTariff;
        }
	}
}
