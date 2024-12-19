namespace DAL
{
    public interface ISubHeadLineRepository : IRepository<Models.SubHeadLine>
    {

        System.Linq.IQueryable<Models.SubHeadLine> GetSubHeadLines();

        Models.SubHeadLine GetByCode(string code);

        System.Linq.IQueryable<Models.SubHeadLine> GetByHeadLineId(System.Guid HeadLineId);
    }
}
