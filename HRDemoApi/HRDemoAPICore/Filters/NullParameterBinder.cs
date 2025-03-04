using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace HRDemoAPICore.Filters
{
    public class NullToEmptyStringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Model = string.Empty; // Replace null or empty with empty string
            }
            else
            {
                bindingContext.Model = value; // Otherwise, use the original value
            }

            return Task.CompletedTask;
        }

        public class NullToEmptyStringModelBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(ModelBinderProviderContext context)
            {
                if (context.Metadata.ModelType == typeof(string))
                {
                    return new BinderTypeModelBinder(typeof(NullToEmptyStringModelBinder));
                }
                return null;
            }
        }
    }

}