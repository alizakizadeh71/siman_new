namespace DAL
{
    public interface IUnitRepository : IRepository<Models.Unit>
    {

        System.Linq.IQueryable<Models.Unit> GetUnits();

    }
}
