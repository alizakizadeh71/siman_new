using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Kendo
{
	public class KendoGridResult<T>
	{
		public IEnumerable<T> Items { get; set; }
		public int TotalCount { get; set; }
	}
}
