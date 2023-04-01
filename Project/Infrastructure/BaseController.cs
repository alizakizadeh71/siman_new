using System.Linq;
using System.Data.Entity;

namespace Infrastructure
{
	public class BaseController : System.Web.Mvc.Controller
	{
		public BaseController()
		{
			// اين دستور بايد دقيقا در همين محل نوشته شود
			ViewBag.PageMessages = PageMessages;

			System.Globalization.CultureInfo oCultureInfo =
				new System.Globalization.CultureInfo(Sessions.Culture);

			oCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
			oCultureInfo.NumberFormat.PercentDecimalSeparator = ".";
			oCultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";

			System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
			System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
		}

		private System.Collections.Generic.IList<Infrastructure.PageMessage> _pageMessages;
		public System.Collections.Generic.IList<Infrastructure.PageMessage> PageMessages
		{
			get
			{
				if (_pageMessages == null)
				{
					_pageMessages =
						new System.Collections.Generic.List<Infrastructure.PageMessage>();
				}
				return (_pageMessages);
			}
		}

		public int CultureLcid
		{
			get
			{
				return (System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
			}
		}

		public string CultureName
		{
			get
			{
				return (System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			}
		}

		public System.Globalization.CultureInfo Culture
		{
			get
			{
				return (System.Threading.Thread.CurrentThread.CurrentCulture);
			}
		}
	}
}
