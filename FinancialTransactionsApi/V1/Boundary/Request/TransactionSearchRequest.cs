using FinancialTransactionsApi.V1.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionSearchRequest
    {
        private const int DefaultPageSize = 12;

        [FromQuery(Name = "searchText")]
        public string SearchText { get; set; }



        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "page")]
        public int Page { get; set; }

        //[FromQuery(Name = "sortBy")]
        //public string SortBy { get; set; }

        //[FromQuery(Name = "isDesc")]
        //public bool IsDesc { get; set; }
    }
}
