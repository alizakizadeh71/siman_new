using Models;

namespace DAL
{
    public interface IFinancialManagementRepository : IRepository<Models.FinancialManagement>
	{
        System.Linq.IQueryable<Models.FinancialManagement> GetByUser(Models.User user);

        void Insertdata(Models.FinancialManagement financialManagement);

    }
}
