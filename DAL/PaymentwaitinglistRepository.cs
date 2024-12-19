using Models;
using System;
using System.Linq;

namespace DAL
{
    public class PaymentwaitinglistRepository : Repository<Models.Paymentwaitinglist>, IPaymentwaitinglistRepository
    {
        public PaymentwaitinglistRepository(Models.DatabaseContext databaseContext)
: base(databaseContext)
        {
        }
        public IQueryable<Paymentwaitinglist> GetPaymentwaitinglist()
        {
            IQueryable<Models.Paymentwaitinglist> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        public void Insertdata(Paymentwaitinglist Paymentwaitinglist)
        {
            try
            {
                DatabaseContext.Paymentwaitinglist.Add(Paymentwaitinglist);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
