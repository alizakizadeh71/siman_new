namespace DAL
{
    public interface IUserLoginLogRepository : IRepository<Models.UserLoginLog>
	{
        System.Linq.IQueryable<Models.UserLoginLog> GetAuthenticatedUsers();

        System.Linq.IQueryable<Models.UserLoginLog> GetByUserId(System.Guid userId);

        Models.UserLoginLog GetBySessionIdAndUserId(string sessionId, System.Guid userId);

        System.Linq.IOrderedQueryable<Models.UserLoginLog> GetBySessionId(string sessionId);
	}
}
