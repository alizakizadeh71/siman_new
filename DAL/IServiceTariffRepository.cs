namespace DAL
{
    public interface IServiceTariffRepository : IRepository<Models.ServiceTariff>
	{

        System.Linq.IQueryable<Models.ServiceTariff> GetServiceTariffs();
        System.Linq.IQueryable<Models.ServiceTariff> GetBySubSystemId(System.Guid subsystemId);
        Models.ServiceTariff GetByRCode(string code);
        Models.ServiceTariff GetByVCode(string code);
	}
}
