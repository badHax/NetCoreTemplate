using Microsoft.AspNetCore.Mvc;
using NetCoreTemplate.BLL;
using NetCoreTemplate.DAL.API;
using NetCoreTemplate.Web.Extensions;
using NetCoreTemplate.Web.Misc;
using NetCoreTemplate.Web.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreTemplate.Common;

namespace NetCoreTemplate.Web.Controllers
{
    //redirect on error. can be custom for each action separately
    [HandleAppError(ControllerName = RawStrings.DEFAULT_ERROR_CONTROLLER, ViewName = RawStrings.DEFAULT_ERROR_VIEW)]
    public class HomeController : BaseController<HomeController>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AbstractTool _abstractTool;

        public HomeController(IServiceProvider serviceBus)
        {
            _serviceProvider = serviceBus;
            _abstractTool = HttpContext.Session.GetString(RawStrings.CUSTOMER_TYPE) == RawStrings.CUSTOMER_TYPE_ONE
                ? new AbstractTool(_serviceProvider, ResourceManagementSystem.API1)
                : new AbstractTool(_serviceProvider, ResourceManagementSystem.API2);

            var val = Configuration[RawStrings.SOME_CONFIG_VALUE]; //access app settings from base controller
            Logger.Trace("Home controller initialized"); //access logger from base controller
            Toast.RemoveAll(); // clear all toasts
        }
        public async Task<IActionResult> Index()
        {
            Logger.ActionStart(ControllerContext); // log action start
            Logger.Debug(
                $"Getting details for customer with id {HttpContext.Session.GetString(RawStrings.CUSTOMER_IDENTIFIER)}");
            var customer =
                await _abstractTool.GetCustomerData(HttpContext.Session.GetString(RawStrings.CUSTOMER_IDENTIFIER));

            // redirect to default view with error msg
            if (null == customer)
                throw new UserLevelException(RawStrings.CUSTOMER_NOT_FOUND);

            //display toast message
            Toast.AddAlertToastMessage(RawStrings.GENERIC_APP_Error);
            Toast.AddErrorToastMessage(RawStrings.GENERIC_APP_Error);
            Toast.AddSuccessToastMessage(RawStrings.GENERIC_APP_Error);
            Toast.AddWarningToastMessage(RawStrings.GENERIC_APP_Error);
            Toast.AddInfoToastMessage(RawStrings.GENERIC_APP_Error);
            Logger.ActionStart(ControllerContext); // log action end
            return View(customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}