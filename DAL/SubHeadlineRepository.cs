using System.Linq;

namespace DAL
{
    public class SubHeadLineRepository : Repository<Models.SubHeadLine>, ISubHeadLineRepository
    {
        public SubHeadLineRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.SubHeadLine> GetSubHeadLines()
        {
            IQueryable<Models.SubHeadLine> list = null;
            list = Get();
            return list;
        }

        public Models.SubHeadLine GetByCode(string code)
        {
            Models.SubHeadLine oSubHeadLine =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oSubHeadLine;
        }

        public IQueryable<Models.SubHeadLine> GetByHeadLineId(System.Guid HeadLineId)
        {
            try
            {
                IQueryable<Models.SubHeadLine> retValue;

                retValue
                    = Get()
                    .Where(current => current.HeadLineId == HeadLineId);


                return retValue;
            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
