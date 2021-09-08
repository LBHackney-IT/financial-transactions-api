using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseTransactionsSearchRequest
    {
        private const int DefaultPageSize = 11;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "page")]
        public int Page { get; set; }

        /*[FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        [FromQuery(Name = "isDesc")]
        public bool IsDesc { get; set; }*/
    }
}
