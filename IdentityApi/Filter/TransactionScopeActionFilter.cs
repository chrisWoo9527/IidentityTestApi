using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Transactions;

namespace IdentityApi.Filter
{
    public class TransactionScopeActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            bool hasNotTransactionScopeAttribute = false;
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                hasNotTransactionScopeAttribute = actionDescriptor.MethodInfo.IsDefined(typeof(NotTransactionScopeAttribute));

                if (hasNotTransactionScopeAttribute)
                {
                    await next();
                    return;
                }

                using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var result = await next();
                if (result.Exception == null)
                {
                    scope.Complete();
                }
            }
        }
    }
}
