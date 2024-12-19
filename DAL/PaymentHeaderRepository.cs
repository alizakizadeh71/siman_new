using System.Linq;

namespace DAL
{
    public class PaymentHeaderRepository : Repository<Models.PaymentHeader>, IPaymentHeaderRepository
    {
        public PaymentHeaderRepository(Models.DatabaseContext databaseContext)
    : base(databaseContext)
        {
        }

        public IQueryable<Models.PaymentHeader> GetPaymentHeaders()
        {
            IQueryable<Models.PaymentHeader> list = null;
            list = Get();
            return list;
        }

        public Models.PaymentHeader GetByTitele(string Titele)
        {
            Models.PaymentHeader oPaymentHeader =
                Get()
                .Where(currenct => currenct.Titele == Titele)
                .FirstOrDefault();

            return oPaymentHeader;
        }
    }
}
