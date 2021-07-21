using FinancialTransactionsApi.V1.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Domain
{
    public class SuspenseResolutionInfo
    {
        [RequiredDateTime]
        public DateTime? ResolutionDate { get; set; }

        public bool IsResolve { get; set; }

        [Required]
        public string Note { get; set; }
    }
}
