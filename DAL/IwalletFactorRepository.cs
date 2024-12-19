namespace DAL
{
    public interface IwalletFactorRepository : IRepository<Models.walletFactor>
    {
        int GetLastInvoiceNumber();
        System.Linq.IQueryable<Models.walletFactor> GetByUser(Models.User user);
        System.Linq.IQueryable<Models.walletFactor> GetByinvoicenumber(int invoicenumber);
        System.Linq.IQueryable<Models.walletFactor> GetByAuthority(string authority);

        void Insertdata(Models.walletFactor walletFactor);

    }
}
