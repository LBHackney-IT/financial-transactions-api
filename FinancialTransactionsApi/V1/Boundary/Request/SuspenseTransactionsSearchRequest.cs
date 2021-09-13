using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseTransactionsSearchRequest
    {
        private const int DefaultPageSize = 11;

        [FromQuery(Name = "pageSize")]
        [Range(1, int.MaxValue, ErrorMessage = "The page size must be great and equal than 1")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "page")]
        [Range(1, int.MaxValue, ErrorMessage = "The page number must be great and equal than 1")]
        public int Page { get; set; } = 0;

        [FromQuery(Name = "searchText")]
        public string SearchText { get; set; }
    }
}
