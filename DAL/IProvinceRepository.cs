namespace DAL
{
    public interface IProvinceRepository : IRepository<Models.Province>
    {
        Models.Province GetByCode(string code);
        System.Linq.IQueryable<Models.Province> Get(Models.User user);
        void Updatedata(Models.Province province);
    }
}
