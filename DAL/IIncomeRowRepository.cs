namespace DAL
{
    public interface IIncomeRowRepository : IRepository<Models.IncomeRow>
    {

        System.Linq.IQueryable<Models.IncomeRow> GetIncomeRows();

        Models.IncomeRow GetByCode(string code);
    }
}
