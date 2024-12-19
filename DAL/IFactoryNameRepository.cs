namespace DAL
{
    public interface IFactoryNameRepository : IRepository<Models.FactoryName>
    {

        System.Linq.IQueryable<Models.FactoryName> GetFactoryNames();

        Models.FactoryName GetByCode(string code);
        System.Linq.IQueryable<Models.FactoryName> GetByProductNameId(System.Guid ProductNameId);
        void Insertdata(Models.FactoryName factoryName);
    }
}
