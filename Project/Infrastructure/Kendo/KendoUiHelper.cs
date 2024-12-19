using Infrastructure.Helpers;
using OpenXml;
using System.Collections.Generic;
using System.Linq;
namespace Infrastructure.Kendo
{
    public class KendoUiHelper
    {
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection)
        {
            return ParseGridData<T>(collection, new KendoGridPost());
        }
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection, KendoGridPost requestParams)
        {
            if (requestParams.Export.IsNotEmpty())
            {
                ReturnXlsExport<T>(collection, requestParams);
            }
            else
            {
                return ReturnGridData<T>(requestParams, ref collection);
            }

            return null;
        }


        private static void ReturnXlsExport<T>(IQueryable<T> collection, KendoGridPost requestParams)
        {
            var o2x = new ObjectsToXls();
            o2x.AddSheet((IEnumerable<object>)collection, requestParams.Export);
            o2x.WriteToHttpResponse(requestParams.Export + ".xlsx");
        }

        private static KendoGridResult<T> ReturnGridData<T>(KendoGridPost requestParams, ref IQueryable<T> collection)
        {
            //If the sort Order is provided perform a sort on the specified column
            if (requestParams.SortOrd.IsNotEmpty())
            {
                collection = requestParams.SortOrd == "desc"
                             ? collection.OrderByDescending(requestParams.SortOn)
                             : collection.OrderBy(requestParams.SortOn);
            }

            List<T> gridData;
            try
            {
                gridData = collection.Skip(requestParams.Skip).Take(requestParams.PageSize).ToList();
            }
            catch
            {
                collection = collection.Select(x => x).OrderBy("id");
                gridData = collection.Skip(requestParams.Skip).Take(requestParams.PageSize).ToList();
            }

            //var collectionType = typeof(T);
            //var collectionProperties = collectionType.GetProperties();
            //gridData.ForEach(row => {
            //    foreach (var cProp in collectionProperties)
            //    {
            //        var pVal = cProp.GetValue(row, null);
            //        if (pVal == null)
            //        {
            //            cProp.SetValue(row, "", null);
            //        }
            //    }
            //});

            return new KendoGridResult<T>
            {
                Items = gridData,
                TotalCount = collection.Count()
            };
        }
    }
}
