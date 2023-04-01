using System;
using System.Collections.Generic;
using System.Web;

namespace Infrastructure.Kendo
{
    public class KendoGridSorterCollection
    {
        public List<KendoGridSorter> Sorters { get; private set; }
        private KendoGridSorterCollection()
        {
            Sorters = new List<KendoGridSorter>();
        }

        public static KendoGridSorterCollection BuildEmptyCollection()
        {
            return new KendoGridSorterCollection();
        }

        public static KendoGridSorterCollection BuildCollection(HttpRequestBase request)
        {
            var collection = BuildEmptyCollection();

            var idex = 0;
            while(true)
            {
                var sorter = new KendoGridSorter()
                {
                    Field = request.Params["sort[" + idex + "][field]"],
                    Direction = request.Params["sort[" + idex + "][dir]"] 
                };

                if (sorter.Field == null) { break; }
                collection.Sorters.Add(sorter);
                idex++;
            }

            return collection;
        }
    }

    public class KendoGridSorter
    {
        public string Field { get; set; }
        public string Direction { get; set; }
    }
}