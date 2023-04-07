using Models;

namespace DAL
{
    public interface IFactorCementRepository : IRepository<Models.FactorCement>
	{
        System.Linq.IQueryable<Models.FactorCement> Get(Models.User user);

        void Insert(Models.FactorCement factorCement);

        void Update(Models.FactorCement factorCement);

    }
}
