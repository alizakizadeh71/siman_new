namespace Infrastructure
{
    public class CustomRequireHttpsAttribute : System.Web.Mvc.RequireHttpsAttribute
    {
        public CustomRequireHttpsAttribute()
        {
        }

        protected override void HandleNonHttpsRequest
            (System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (!Infrastructure.GlobalApplicationSettings.Instance())
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }
    }
}
