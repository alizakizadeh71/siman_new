using Utilities;

namespace Utilities.Kendo
{
	public class GridPost : System.Object
	{
		public GridPost()
		{
			if (System.Web.HttpContext.Current == null)
			{
				throw (new System.Exception("Unexpected Error!"));
			}

			int intIndex = 0;

			System.Web.HttpRequest oCurrentHttpRequest =
				System.Web.HttpContext.Current.Request;

			// **************************************************
			string strSortDir = null;
			string strSortField = null;
			string strSortDirKeyName = null;
			string strSortFieldKeyName = null;

			intIndex = 0;
			do
			{
				strSortDirKeyName =
					string.Format("sort[{0}][dir]", intIndex);

				strSortFieldKeyName =
					string.Format("sort[{0}][field]", intIndex);

				strSortDir =
					oCurrentHttpRequest.Params[strSortDirKeyName];

				strSortField =
					oCurrentHttpRequest.Params[strSortFieldKeyName];

				if (string.IsNullOrWhiteSpace(strSortField) == false)
				{
					if (string.IsNullOrWhiteSpace(strSortDir))
					{
						strSortDir = "asc";
					}

					SortCollection.Add(new GridSort(strSortField, strSortDir));
				}

				intIndex++;
			}
			while (string.IsNullOrWhiteSpace(strSortField) == false);
			// **************************************************

			// **************************************************
			Page = 1;
			try
			{
				Page =
					System.Convert.ToInt32(oCurrentHttpRequest.Params["page"]);
			}
			catch { }

			Skip = 0;
			try
			{
				Skip =
					System.Convert.ToInt32(oCurrentHttpRequest.Params["skip"]);
			}
			catch { }

			Take = 10;
			try
			{
				Take =
					System.Convert.ToInt32(oCurrentHttpRequest.Params["take"]);
			}
			catch { }

			PageSize = 10;
			try
			{
				PageSize =
					System.Convert.ToInt32(oCurrentHttpRequest.Params["pageSize"]);
			}
			catch { }
			// **************************************************
		}

		public int Page { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public int PageSize { get; set; }

		private System.Collections.Generic.List<GridSort> _sortCollection;
		public System.Collections.Generic.List<GridSort> SortCollection
		{
			get
			{
				if (_sortCollection == null)
				{
					_sortCollection =
						new System.Collections.Generic.List<GridSort>();
				}
				return (_sortCollection);
			}
		}
	}
}
