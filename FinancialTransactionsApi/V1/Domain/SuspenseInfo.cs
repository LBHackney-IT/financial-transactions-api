using FinancialTransactionsApi.V1.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Domain
{
    public class SuspenseInfo
    {
        [Required]
        [RequiredDateTime]
        public DateTime? ResolutionDate { get; set; }

        [BoolValidate(true)]
        public bool IsResolve { get; set; }

        [Required]
        public string Note { get; set; }
    }
}
