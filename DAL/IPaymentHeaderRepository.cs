namespace DAL
{
    public interface IPaymentHeaderRepository : IRepository<Models.PaymentHeader>
    {
        System.Linq.IQueryable<Models.PaymentHeader> GetPaymentHeaders();

        Models.PaymentHeader GetByTitele(string Titele);

    }
}
