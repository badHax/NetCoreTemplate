using NetCoreTemplate.Common;
using NetCoreTemplate.DAL.API;

namespace NetCoreTemplate.BLL
{
    public class AbstractTool
    {
        private readonly IServiceProvider _provider;
   
        public ResourceManagementSystem Type { get; }
    
        public AbstractTool(IServiceProvider serviceProvider, ResourceManagementSystem rms)
        {
            _provider = serviceProvider;
            Type = rms;
        }

        public async System.Threading.Tasks.Task<CustomerInfo> GetCustomerData(string customerIdentifier) =>
               await _provider.GetCustomerAsync(customerIdentifier, Type);

    }
}
