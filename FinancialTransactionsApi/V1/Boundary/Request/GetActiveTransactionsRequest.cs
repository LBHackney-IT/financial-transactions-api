using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class GetActiveTransactionsRequest
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
        public DateTime? PeriodStartDate { get; set; }

        [Required]
        [FromQuery]
        public DateTime? PeriodEndDate { get; set; }
    }
}
