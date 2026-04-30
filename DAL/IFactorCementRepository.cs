using System;
using System.Linq;

namespace DAL
{
    public interface IFactorCementRepository : IRepository<Models.FactorCement>
    {
        int GetLastInvoiceNumber();
        System.Linq.IQueryable<Models.FactorCement> GetByUser(Models.User user);
        System.Linq.IQueryable<Models.FactorCement> GetByinvoicenumber(int invoicenumber);
        System.Linq.IQueryable<Models.FactorCement> GetByAuthority(string authority);
        IQueryable<Models.FactorCement> GetOrdersByState(Enums.RequestState state);
        void UpdateOrderState(Guid orderId, Enums.RequestState newState);
        void SetDriverInfoAndLoadOrder(Guid orderId, string driverName, string driverLastName, string driverMobile, string driverPlate);

        void Insertdata(Models.FactorCement factorCement);

    }
}
