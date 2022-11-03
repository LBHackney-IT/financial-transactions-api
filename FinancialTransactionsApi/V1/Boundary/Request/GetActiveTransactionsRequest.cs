using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class GetActiveTransactionsRequest : PagingParametersRequest
    {
        [Required]
        [FromQuery]
        public DateTime? PeriodStartDate { get; set; }

        [Required]
        [FromQuery]
        public DateTime? PeriodEndDate { get; set; }
    }
}
