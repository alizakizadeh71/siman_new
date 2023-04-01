namespace DAL
{
    public interface IErrorLogRepository : IRepository<Models.ErrorLog>
	{
       void InsertErrorLog(string userName,string errorMessage,string description1,string description2);
	}
}
