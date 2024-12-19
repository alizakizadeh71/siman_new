namespace DAL
{
    public interface ICityRepository : IRepository<Models.City>
    {
        Models.City GetByCode(string code, System.Guid Provinceid);
        System.Linq.IQueryable<Models.City> Get(Models.User user);
        System.Linq.IQueryable<Models.City> GetByProvinceId(System.Guid provinceId);
    }
}
