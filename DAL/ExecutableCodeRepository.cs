using System.Linq;

namespace DAL
{
    public class ExecutableCodeRepository : Repository<Models.ExecutableCode>, IExecutableCodeRepository
    {
        public ExecutableCodeRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public IQueryable<Models.ExecutableCode> GetExecutableCodes()
        {
            IQueryable<Models.ExecutableCode> list = null;
            list = Get();
            return list;
        }

        public Models.ExecutableCode GetByCode(string code)
        {
            Models.ExecutableCode oExecutableCode =
                Get()
                .Where(currenct => currenct.Code == code)
                .FirstOrDefault();

            return oExecutableCode;
        }
    }
}
