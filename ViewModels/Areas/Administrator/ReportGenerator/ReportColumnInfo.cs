using System.Linq;
namespace ViewModels.Areas.Administrator.ReportGenerator
{
	public class ReportColumnInfo
	{
		public string Table { get; set; }
		public string Column { get; set; }
		public string Alias { get; set; }
		public string UniqueName { get; set; }
		public Enums.AggregateFunction AggregateFunction { get; set; }
		public int Order { get; set; }
		public System.Collections.Generic.List<Enums.AggregateFunction> FooterAggregateFunctions { get; set; }
	}
}
