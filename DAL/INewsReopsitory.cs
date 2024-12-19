namespace DAL
{
    public interface INewsReopsitory : IRepository<Models.newsweb>
    {
        System.Linq.IQueryable<Models.newsweb> GetProductNames();

        Models.newsweb GetByCode(string code);
        void Insertdata(Models.newsweb newsweb);
    }
}
