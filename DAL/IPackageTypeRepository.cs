namespace DAL
{
    public interface IPackageTypeRepository : IRepository<Models.PackageType>
	{

        System.Linq.IQueryable<Models.PackageType> GetPackageTypes();

        Models.PackageType GetByPackageTypeCode(string code);

        System.Linq.IQueryable<Models.PackageType> GetByProductTypeId(System.Guid ProductTypeId);
	}
}
