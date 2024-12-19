using System.Linq;
using System.Linq.Dynamic;

namespace Utilities.Kendo
{
    public static class HtmlHelpers
    {
        static HtmlHelpers()
        {
        }

        public static GridResult<T> ParseGridData<T>(System.Linq.IQueryable<T> collection)
        {
            GridPost oGridPost = new GridPost();

            foreach (GridSort oGridSort in oGridPost.SortCollection)
            {
                collection = collection.OrderBy(oGridSort.Field + " " + oGridSort.Direction);
            }

            System.Collections.Generic.List<T> oGridData;

            try
            {
                oGridData = collection.Skip(oGridPost.Skip).Take(oGridPost.Take).ToList();

            }
            catch
            {
                collection.OrderBy("ID ASC");

                oGridData = collection.Skip(oGridPost.Skip).Take(oGridPost.Take).ToList();
            }

            GridResult<T> oGridResult = new GridResult<T>();

            oGridResult.Data = oGridData;
            oGridResult.Total = collection.Count();

            return (oGridResult);
        }

        public static GridResult<T> ParseGridData<T>(System.Linq.IQueryable<T> collection, bool allowPaging, out object dataSource)
        {
            dataSource = collection;

            GridPost oGridPost = new GridPost();

            foreach (GridSort oGridSort in oGridPost.SortCollection)
            {
                collection = collection.OrderBy(oGridSort.Field + " " + oGridSort.Direction);
            }

            System.Collections.Generic.List<T> oGridData;

            try
            {
                dataSource = collection.Skip(oGridPost.Skip).ToList();

                if (!allowPaging)
                    oGridData = collection.Skip(oGridPost.Skip).ToList();

                else
                    oGridData = collection.Skip(oGridPost.Skip).Take(oGridPost.Take).ToList();
            }
            catch
            {
                collection.OrderBy("ID ASC");

                dataSource = collection.Skip(oGridPost.Skip).ToList();

                if (!allowPaging)
                    oGridData = collection.Skip(oGridPost.Skip).ToList();

                else
                    oGridData = collection.Skip(oGridPost.Skip).Take(oGridPost.Take).ToList();
            }

            GridResult<T> oGridResult = new GridResult<T>();

            oGridResult.Data = oGridData;
            oGridResult.Total = collection.Count();

            return (oGridResult);
        }

    }
}
