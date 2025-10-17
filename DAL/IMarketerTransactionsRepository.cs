namespace DAL
{
    public interface IMarketerTransactionsRepository : IRepository<Models.MarketerTransactions>
    {

        System.Linq.IQueryable<Models.MarketerTransactions> GetTransactions(int MarktingCode);
        void Insertdata(Models.MarketerTransactions Transactions);
    }
}
