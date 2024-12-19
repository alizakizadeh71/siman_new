namespace Infrastructure
{
    public class DateTimeModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return Infrastructure.Utility.ToDate(value.AttemptedValue);
        }
    }
}
