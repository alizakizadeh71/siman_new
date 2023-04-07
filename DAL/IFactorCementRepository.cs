using Models;

namespace DAL
{
    public interface IFactorCementRepository : IRepository<Models.FactorCement>
	{
        System.Linq.IQueryable<Models.FactorCement> Get(Models.User user);

        void Insertdata(Models.FactorCement factorCement);

    }
}
