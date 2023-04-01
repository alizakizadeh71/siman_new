namespace DAL
{
    public interface IPackageTypeRepository : IRepository<Models.PackageType>
	{

        System.Linq.IQueryable<Models.PackageType> GetSubHeadLines();

        Models.PackageType GetByCode(string code);

        System.Linq.IQueryable<Models.PackageType> GetByProductTypeId(System.Guid ProductTypeId);
	}
}
