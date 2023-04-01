using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class IncomeRowRepository : Repository<Models.IncomeRow>, IIncomeRowRepository
	{
        public IncomeRowRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.IncomeRow> GetIncomeRows()
        {
            IQueryable<Models.IncomeRow> list = null;
            list = Get();
            return list;
        }

        public Models.IncomeRow GetByCode(string code)
        {
            Models.IncomeRow oIncomeRow =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oIncomeRow;
        }
	}
}
