using Microsoft.Extensions.Configuration;
using NetCoreTemplate.Common;

namespace NetCoreTemplate.DAL.API
{
    public class APIClient : IServiceProvider
    {
        private readonly IConfiguration _config;
        private readonly HttpCall _httpCall;
        private readonly string _getCustomerUrl;
        private readonly string _api1;
        private readonly string _api2;

        public APIClient(IConfiguration config)
        {
            _config = config;
            _httpCall = new HttpCall();
            _api1 = _config["Api1:Base"];
            _api2 = _config["Api2:Base"];
            _getCustomerUrl = _config["Api:Esb:Actions:CustomerInfo"];
        }

        public async System.Threading.Tasks.Task<CustomerInfo> GetCustomerAsync(string custId,  ResourceManagementSystem rms)
        {
            var cmsUrl = rms.HasFlag(ResourceManagementSystem.API1) ? _api1 : _api2;
            var response = await _httpCall.GetJsonRequest<CustomerInfo[]>(
                cmsUrl +
                _getCustomerUrl +
                custId);
            return response.Length > 0 ? response[0] : null;
        }

    }

    public enum ResourceManagementSystem
    {
        API1,
        API2
    }

    public class CustomerInfoWrapper {
        public CustomerInfo[] Customers { get; set; }
    }
}
