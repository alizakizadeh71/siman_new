using System.Linq;

namespace DAL
{
    public class UnitRepository : Repository<Models.Unit>, IUnitRepository
    {
        public UnitRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.Unit> GetUnits()
        {
            IQueryable<Models.Unit> list = null;
            list = Get();
            return list;
        }
    }
}
