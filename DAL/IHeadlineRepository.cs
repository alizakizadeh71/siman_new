namespace DAL
{
    public interface IHeadLineRepository : IRepository<Models.HeadLine>
    {

        System.Linq.IQueryable<Models.HeadLine> GetHeadLines();

        Models.HeadLine GetByCode(string code);
    }
}
