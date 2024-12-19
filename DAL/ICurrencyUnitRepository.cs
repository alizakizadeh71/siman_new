namespace DAL
{
    public interface ICurrencyUnitRepository : IRepository<Models.CurrencyUnit>
    {

        System.Linq.IQueryable<Models.CurrencyUnit> GetCurrencyUnits(Models.User user);

        Models.CurrencyUnit GetByCode(int code);
    }
}
