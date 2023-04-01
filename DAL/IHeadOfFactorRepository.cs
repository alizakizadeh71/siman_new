namespace DAL
{
    public interface IHeadOfFactorRepository : IRepository<Models.HeadOfFactor>
	{
        System.Linq.IQueryable<Models.HeadOfFactor> Get(Models.User user);
	}
}
