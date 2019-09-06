using NetCoreTemplate.Common;
using System.Threading.Tasks;

namespace NetCoreTemplate.DAL.API
{
    public interface IServiceProvider
    {
        Task<CustomerInfo> GetCustomerAsync(string customerNumber, ResourceManagementSystem rms);
    }
}