using System.Web.Mvc;

namespace OPS.Areas.Carrier
{
    public class CarrierAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Carrier";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Carrier_default",
                "Carrier/{controller}/{action}/{id}",
                new { controller = "Request", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}