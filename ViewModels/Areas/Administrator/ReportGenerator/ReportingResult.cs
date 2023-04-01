using System.Linq;
using System.Data;
namespace ViewModels.Areas.Administrator.ReportGenerator
{
	public class ReportingResult
	{
		public System.Data.DataTable DataTable { get; set; }

		public System.Collections.Generic.List<ReportColumnInfo> ColumnInfos { get; set; }
	}
}
