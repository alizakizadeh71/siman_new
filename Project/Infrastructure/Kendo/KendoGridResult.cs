using System.Collections.Generic;

namespace Infrastructure.Kendo
{
    public class KendoGridResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
