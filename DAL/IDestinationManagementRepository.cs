using Models;

namespace DAL
{
    public interface IDestinationManagementRepository : IRepository<Models.DestinationManagement>
	{
        System.Linq.IQueryable<Models.DestinationManagement> GetByUser(Models.User user);

        void Insertdata(Models.DestinationManagement destinationManagement);

    }
}
