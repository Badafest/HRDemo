using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRDemoAPICore.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new BadRequestObjectResult(new
                {
                    Message = "The request is invalid",
                    actionContext.ModelState
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext actionContext)
        {

        }
    }
}