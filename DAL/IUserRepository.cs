namespace DAL
{
	public interface IUserRepository : IRepository<Models.User>
	{
        Models.User GetByUserName(string username);

        System.Linq.IQueryable<Models.User> Get(Models.User user);
    }
}
