using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class GetActiveTransactionsRequest
    {
        [FromQuery]
        [Range(1, Int32.MaxValue)]
        public int PageSize { get; set; }

        [FromQuery]
        public DateTime? PeriodStartDate { get; set; }

        [FromQuery]
        public DateTime? PeriodEndDate { get; set; }
    }
}
