using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class HeadLineRepository : Repository<Models.HeadLine>, IHeadLineRepository
	{
        public HeadLineRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public IQueryable<Models.HeadLine> GetHeadLines()
        {
            IQueryable<Models.HeadLine> list = null;
            list = Get();
            return list;
        }

        public Models.HeadLine GetByCode(string code)
        {
            Models.HeadLine oHeadLine =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oHeadLine;
        }
	}
}
