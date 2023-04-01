namespace DAL
{
    public interface IDetailOfFactorRepository : IRepository<Models.DetailOfFactor>
    {
        System.Linq.IQueryable<Models.DetailOfFactor> Get(System.Guid headoffactorid);
        System.Linq.IQueryable<Models.ServiceTariff> GetServiceTariff(System.Guid headoffactorid);
    }
}
