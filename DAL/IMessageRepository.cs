namespace DAL
{
    public interface IMessageRepository : IRepository<Models.Message>
    {
        string MetMessageByRequestId(System.Guid requestid);
    }
}
