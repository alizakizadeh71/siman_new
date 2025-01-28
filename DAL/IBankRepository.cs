namespace DAL
{
    public interface IBankRepository : IRepository<Models.Bank>
    {

        System.Linq.IQueryable<Models.Bank> GetBanks();

        Models.Bank GetByName(string code);
    }
}
