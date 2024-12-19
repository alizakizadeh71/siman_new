namespace DAL
{
    public interface IRequestRepository : IRepository<Models.Request>
    {
        System.Linq.IQueryable<Models.Request> Get(Models.User user);

        void Insert(Models.Request request);

        void Update(Models.Request request);

        string GetVirtualCode();

        Models.Request CustomGet(System.Guid id);
        Models.Request CustomGetByInvoiceNumber(int invoiceNumber);
        Models.Request SetControlCode(Models.Request request);

    }
}
