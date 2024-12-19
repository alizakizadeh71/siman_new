namespace DAL
{
    public class CurrencyUnitLogRepository : Repository<Models.CurrencyUnitLog>, ICurrencyUnitLogRepository
    {
        public CurrencyUnitLogRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

    }
}
