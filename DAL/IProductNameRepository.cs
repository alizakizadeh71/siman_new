namespace DAL
{
    public interface IProductNameRepository : IRepository<Models.ProductName>
	{

        System.Linq.IQueryable<Models.ProductName> GetProductNames();

        Models.ProductName GetByCode(string code);
	}
}
