using System.Linq;
namespace ViewModels.Areas.Administrator.ReportGenerator
{
	public class ReportParameter
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public Enums.DataType DataType { get; set; }
		public string Tag { get; set; }

	}
}
