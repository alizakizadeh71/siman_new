using System.Linq;

namespace DAL
{
    public class CertainRepository : Repository<Models.Certain>, ICertainRepository
    {
        public CertainRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.Certain> GetCertains()
        {
            IQueryable<Models.Certain> list = null;
            list = Get();
            return list;
        }

        public Models.Certain GetByCode(string code)
        {
            Models.Certain oCertain =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oCertain;
        }
    }
}
