namespace DAL
{
    public interface IExecutableCodeRepository : IRepository<Models.ExecutableCode>
    {

        System.Linq.IQueryable<Models.ExecutableCode> GetExecutableCodes();

        Models.ExecutableCode GetByCode(string code);
    }
}
