using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreTemplate.Common;
using NToastNotify;

namespace NetCoreTemplate.Web.Extensions
{
    public abstract class BaseController<T> : Controller where T: class
    {
        private ILogger<T> _logger;
        private ILogger<T> LoggerDefault => _logger
                                       ?? (_logger = new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<ILogger<T>>());

        protected CustomLogger Logger => new CustomLogger(LoggerDefault);

        private IConfiguration _config;
        protected IConfiguration Configuration => _config
                                     ?? (_config = new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IConfiguration>());

        private IToastNotification _toast;
        protected IToastNotification Toast => _toast
                                     ?? (_toast = new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IToastNotification>());

    }
}
