using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;


namespace HRDemoApp.Filters
{
    public class NullToEmptyStringModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName)?.AttemptedValue;

            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Model = string.Empty; // Replace null or empty with empty string
            }
            else
            {
                bindingContext.Model = value; // Otherwise, use the original value
            }

            return true;
        }
    }

}