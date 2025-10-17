using System.Web.Mvc;

public class MarkterAreaRegistration : AreaRegistration
{
    public override string AreaName => "Markter";

    public override void RegisterArea(AreaRegistrationContext context)
    {
        context.MapRoute(
            "Markter_default",
            "Markter/{controller}/{action}/{id}",
            new { controller = "MarketingTransaction", action = "Index", id = UrlParameter.Optional }
        );
    }
}