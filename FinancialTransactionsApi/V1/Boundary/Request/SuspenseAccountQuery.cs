using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseAccountQuery
    {
        [Required]
        [FromQuery]
        [Range(1, Int32.MaxValue)]
        public int PageSize { get; set; }

        [FromQuery]
        [Range(1, Int32.MaxValue)]
        public int Page { get; set; }

        [Required]
        [FromQuery]
        public bool SearchText { get; set; }

    }
}
