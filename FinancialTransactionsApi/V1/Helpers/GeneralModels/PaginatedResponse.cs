using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Boundary.Response;

namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    [ExcludeFromCodeCoverage]
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
        public MetadataModel Metadata { get; set; }
    }
}
