namespace DAL
{
    public interface ICertainRepository : IRepository<Models.Certain>
	{

        System.Linq.IQueryable<Models.Certain> GetCertains();

        Models.Certain GetByCode(string code);
	}
}
