using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using NetCoreTemplate.Common;
using NetCoreTemplate.Web.Misc;

namespace NetCoreTemplate.Web.Extensions
{
    /// <summary>
    /// The main purpose of this class to avoid bulking the controllers with repetitive error handling
    /// logic 
    /// </summary>
    public class HandleAppError : ExceptionFilterAttribute
    {
        private readonly CustomLogger _logger;
        private readonly IToastNotification _toast;

        public string ViewName { get; set; } = "Index";
        public string ControllerName { get; set; } = "Home";
        public Type ExceptionType { get; set; } = null;

        public HandleAppError()
        {
            _toast = new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IToastNotification>();
            _logger = new CustomLogger(new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<ILogger<HandleAppError>>());
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;
            var error = context.Exception;

            switch (error)
            {
                case AggregateException ae:
                {
                    foreach (var innerException in ae.Flatten().InnerExceptions)
                    {
                        _logger.Err("Task failed", innerException);
                    }
                    _toast.AddErrorToastMessage(RawStrings.GENERIC_APP_Error);
                    break;
                }
                case UserLevelException _:
                    _toast.AddErrorToastMessage(error.Message);
                    _logger.Warn("User error occurred.", error);
                    break;
                default:
                    _logger.Err(error.Message, null);
                    _toast.AddErrorToastMessage(RawStrings.GENERIC_APP_Error);
                    break;
            }
            // redirect to the controller that was specified
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", ControllerName },
                    { "action", ViewName }
                });

            context.ExceptionHandled = true;
        }

        #endregion
    }
}
