using System.Collections.Generic;
using System.Web;

namespace Infrastructure.Kendo
{
    public class KendoGridFilterCollection
    {
        public List<KendoGridFilter> Filters { get; private set; }
        private KendoGridFilterCollection()
        {
            Filters = new List<KendoGridFilter>();
        }

        public static KendoGridFilterCollection BuildEmptyCollection()
        {
            return new KendoGridFilterCollection();
        }

        public static KendoGridFilterCollection BuildCollection(HttpRequestBase request)
        {
            var collection = BuildEmptyCollection();

            var idex = 0;
            while (true)
            {
                var filter = new KendoGridFilter()
                {
                    Field = request.Params["filter[filters][" + idex + "][field]"],
                    Operator = request.Params["filter[filters][" + idex + "][operator]"],
                    Value = request.Params["filter[filters][" + idex + "][value]"]
                };

                if (filter.Field == null) { break; }
                collection.Filters.Add(filter);
                idex++;
            }

            return collection;
        }
    }

    public class KendoGridFilter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}