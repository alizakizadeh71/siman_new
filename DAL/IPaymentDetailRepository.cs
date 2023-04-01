namespace DAL
{
    public interface IPaymentDetailRepository : IRepository<Models.PaymentDetail>
    {
        System.Linq.IQueryable<Models.PaymentDetail> GetPaymentDetails();

        Models.PaymentDetail GetByIdentiFire(string PaymentIdentifier);

    }
}
