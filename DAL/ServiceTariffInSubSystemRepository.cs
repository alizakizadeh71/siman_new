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
