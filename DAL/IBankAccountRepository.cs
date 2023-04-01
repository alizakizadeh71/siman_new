namespace DAL
{
    public interface IBankAccountRepository : IRepository<Models.BankAccount>
	{
        System.Linq.IQueryable<Models.BankAccount> GetBankAccounts();

	}
}
