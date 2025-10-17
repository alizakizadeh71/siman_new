using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OPS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 1️⃣ ثبت Areaها باید **قبل از RouteConfig** باشد
            AreaRegistration.RegisterAllAreas();

            // 2️⃣ ثبت فیلترهای سراسری
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // 3️⃣ ثبت مسیرهای Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // 4️⃣ ثبت مسیرهای MVC
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 5️⃣ ثبت Bundleها
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 6️⃣ ModelBinder برای DateTime
            var binder = new Infrastructure.DateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(System.DateTime), binder);
            ModelBinders.Binders.Add(typeof(System.DateTime?), binder);
        }

    }
}
