using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface INewsReopsitory : IRepository<Models.newsweb>
    {
        System.Linq.IQueryable<Models.newsweb> GetProductNames();

        Models.newsweb GetByCode(string code);
        void Insertdata(Models.newsweb newsweb);
    }
}
