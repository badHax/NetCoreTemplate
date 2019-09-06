using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace NetCoreTemplate.Common
{
    public class CustomLogger
    {
        private ILogger _logger;
        public CustomLogger(ILogger configuredLogger)
        {
            _logger = configuredLogger;
        }
        public void Err(string message,Exception err = null) { _logger.LogError(err,message); }
        public void Warn(string message, Exception err = null) { _logger.LogWarning(err, message); }
        public void Debug(string message, Exception err = null) { _logger.LogDebug(err, message); }
        public void Fatal(string message, Exception err = null) { _logger.LogCritical(err, message); }
        public void Trace(string message, Exception err = null) { _logger.LogTrace(err, message); }
        public void info(string message, Exception err = null) { _logger.LogInformation(err, message); }

        public void ActionStart(ControllerContext context) {
            string controller =  context.ActionDescriptor.ControllerName;
            string method = context.ActionDescriptor.ActionName;
            _logger.LogDebug($"Entered {method} in {controller} controller");
        }
        public void ActionEnd(ControllerContext context) {
            string controller = context.ActionDescriptor.ControllerName;
            string method = context.ActionDescriptor.ActionName;
            _logger.LogDebug($"Exit {method} in {controller} controller");
        }
    }
}
