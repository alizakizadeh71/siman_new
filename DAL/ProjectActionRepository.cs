using System.Linq;

namespace DAL
{
    public class ProjectActionRepository : Repository<Models.ProjectAction>, IProjectActionRepository
    {
        public ProjectActionRepository(Models.DatabaseContext databaseContext)
            : base(databaseContext)
        {
        }

        public Models.ProjectAction GetAction(string areaString, string controllerString, string actionString)
        {
            var Action = Get()
                  .Where(current => current.Area == areaString)
                  .Where(current => current.Controller == controllerString)
                  .Where(current => current.Action == actionString)
                  .FirstOrDefault()
                  ;

            return Action;
        }
    }
}
