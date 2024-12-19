using System.Linq;

namespace DAL
{
    public class CurrencyUnitRepository : Repository<Models.CurrencyUnit>, ICurrencyUnitRepository
    {
        public CurrencyUnitRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.CurrencyUnit> GetCurrencyUnits(Models.User user)
        {
            IQueryable<Models.CurrencyUnit> CurrencyUnits = null;

            //if(user.Role.Code<(int)Enums.Roles.Programmer)
            //{
            CurrencyUnits = Get();
            CurrencyUnits = CurrencyUnits.Where(current => current.UserId == user.Id);
            //}

            return CurrencyUnits;
        }

        public Models.CurrencyUnit GetByCode(int code)
        {
            Models.CurrencyUnit oCurrencyUnit =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oCurrencyUnit;
        }
    }
}
