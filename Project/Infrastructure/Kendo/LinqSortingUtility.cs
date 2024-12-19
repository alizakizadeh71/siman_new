using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Kendo
{
    public static class LinqSortingUtility
    {
        public static IEnumerable<T> MultipleSort<T>(this IEnumerable<T> data,
          List<KendoGridSorter> sortExpressions)
        {
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
            {
                return data;
            }

            IEnumerable<T> query = from item in data select item;
            IOrderedEnumerable<T> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                var index = i;
                Func<T, object> expression = item => item.GetType()
                                .GetProperty(sortExpressions[index].Field)
                                .GetValue(item, null);

                if (sortExpressions[index].Direction == "asc")
                {
                    orderedQuery = (index == 0)
                        ? query.OrderBy(expression)
                            : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = (index == 0)
                        ? query.OrderByDescending(expression)
                            : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query;
        }
    }
}
