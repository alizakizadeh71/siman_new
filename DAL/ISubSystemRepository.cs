namespace DAL
{
    public interface ISubSystemRepository : IRepository<Models.SubSystem>
    {
        Models.SubSystem GetByCode(int code);
    }
}
