using System.Linq;
using System.Data.Entity;

namespace DAL
{
    public class PaymentDetailRepository : Repository<Models.PaymentDetail>, IPaymentDetailRepository
    {
        public PaymentDetailRepository(Models.DatabaseContext databaseContext)
    : base(databaseContext)
        {
        }

        public IQueryable<Models.PaymentDetail> GetPaymentDetails()
        {
            IQueryable<Models.PaymentDetail> list = null;
            list = Get();
            return list;
        }

        public Models.PaymentDetail GetByIdentiFire(string PaymentIdentifier)
        {
            Models.PaymentDetail oPaymentDetail =
                Get()
                .Where(currenct => currenct.PaymentIdentifier == PaymentIdentifier)
                .FirstOrDefault();

            return oPaymentDetail;
        }
    }
}
