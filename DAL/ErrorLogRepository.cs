using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class ErrorLogRepository : Repository<Models.ErrorLog>, IErrorLogRepository
	{
        public ErrorLogRepository(Models.DatabaseContext databaseContext)
			: base(databaseContext)
		{
		}

        public void InsertErrorLog(string userName,string errorMessage,string description1,string description2)
        {
            try
            {
                Models.ErrorLog oErrorLog = new Models.ErrorLog();
                oErrorLog.UserName = userName;
                oErrorLog.ErrorMessage = errorMessage;
                oErrorLog.Description1 = description1;
                oErrorLog.Description2 = description2;
                Insert(oErrorLog);

            }

            catch (System.Exception ex)
            {
                throw ex;
            }
        }
	}
}
