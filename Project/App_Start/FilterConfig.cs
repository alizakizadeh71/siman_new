﻿using System.Web.Mvc;

namespace OPS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Infrastructure.AuthorizeAttribute());
        }
    }
}
