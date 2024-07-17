using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IPaymentwaitinglistRepository : IRepository<Models.Paymentwaitinglist>
    {
        System.Linq.IQueryable<Models.Paymentwaitinglist> GetPaymentwaitinglist();

        Models.Paymentwaitinglist GetById(Guid Id);
        void Insertdata(Models.Paymentwaitinglist Paymentwaitinglist);
    }
}
