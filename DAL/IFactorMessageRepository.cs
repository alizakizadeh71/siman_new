namespace DAL
{
    public interface IFactorMessageRepository : IRepository<Models.FactorMessage>
	{
        string MetMessageById(System.Guid requestid);
	}
}
