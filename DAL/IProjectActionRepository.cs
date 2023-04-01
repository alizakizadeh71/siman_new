namespace DAL
{
    public interface IProjectActionRepository : IRepository<Models.ProjectAction>
	{
        Models.ProjectAction GetAction(string areaString, string controllerString, string actionString);
	}
}
