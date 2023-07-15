namespace DAL
{
    public interface ITonnageRepository : IRepository<Models.Tonnage>
	{

        System.Linq.IQueryable<Models.Tonnage> GetTonnages();

        Models.Tonnage GetByCode(string code);

        System.Linq.IQueryable<Models.Tonnage> GetByPackageTypeId(System.Guid PackageTypeId);
        void Insertdata(Models.Tonnage tonnage);
    }
}
