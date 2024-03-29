using System.Collections.Generic;
using Hackney.Shared.Finance.Boundary.Response;

namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
        public MetadataModel Metadata { get; set; }
    }
}
