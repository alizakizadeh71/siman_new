using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NewsReopsitory : Repository<Models.newsweb>, INewsReopsitory
    {
        public NewsReopsitory(Models.DatabaseContext databaseContext)
        : base(databaseContext)
        {
        }

        public newsweb GetByCode(string code)
        {
            Models.newsweb onews =
                Get()
                .Where(x => x.IsActived && !x.IsDeleted)
                .Where(currenct => currenct.Id.ToString() == code)
                .FirstOrDefault();

            return onews;
        }

        public IQueryable<newsweb> GetProductNames()
        {
            IQueryable<Models.newsweb> list = null;
            list = Get().Where(x => x.IsActived && !x.IsDeleted);
            return list;
        }

        public void Insertdata(newsweb newsweb)
        {
            try
            {
                DatabaseContext.News.Add(newsweb);
                DatabaseContext.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
