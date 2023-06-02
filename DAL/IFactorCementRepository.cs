using Models;

namespace DAL
{
    public interface IFactorCementRepository : IRepository<Models.FactorCement>
	{
        int GetLastInvoiceNumber();
        System.Linq.IQueryable<Models.FactorCement> GetByUser(Models.User user);
        System.Linq.IQueryable<Models.FactorCement> GetByinvoicenumber(int invoicenumber);
        System.Linq.IQueryable<Models.FactorCement> GetByAuthority(string authority);

        void Insertdata(Models.FactorCement factorCement);

    }
}
