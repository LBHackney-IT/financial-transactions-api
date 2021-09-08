using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseTransactionsSearchRequest
    {
        private const int DefaultPageSize = 11;

        [FromQuery(Name = "pageSize")]
        [Range(1, short.MaxValue, ErrorMessage = "the page size must be great and equal than {0}")]
        public int PageSize { get; set; } = DefaultPageSize;

        [FromQuery(Name = "page")]
        [Range(0, short.MaxValue, ErrorMessage = "the page number is wrong")]
        public int Page { get; set; }
    }
}
