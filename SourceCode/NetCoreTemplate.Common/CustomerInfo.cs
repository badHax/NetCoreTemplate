using Newtonsoft.Json;

namespace NetCoreTemplate.Common
{
    public class CustomerInfo
    {
        [JsonProperty("customerId")] public string CustomerNumber { get; set; }
        [JsonProperty("accountId")] public string AccountNumber { get; set; }
    }
}
