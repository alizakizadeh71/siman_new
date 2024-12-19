namespace DAL
{
    public interface IVillageRepository : IRepository<Models.village>
    {
        System.Linq.IQueryable<Models.village> GetVillageName();

        Models.village GetByCode(string code);
        System.Linq.IQueryable<Models.village> GetBycityId(System.Guid cityId);
        void Insertdata(Models.village village);
    }
}
