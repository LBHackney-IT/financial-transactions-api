using Newtonsoft.Json;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class MetadataModel
    {
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public PaginationDetails Pagination { get; set; }
    }
}
