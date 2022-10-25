using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Domain
{
    public class Transaction : TransactionBase
    {
        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }
        [GreatAndEqualThan("0.0")]
        public decimal TransactionAmount { get; set; }
        [AllowNull]
        public string BankAccountNumber { get; set; }
        [AllowNull]
        public string SortCode { get; set; }
        public bool IsSuspense => TargetId == Guid.Empty;
        [AllowNull]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
        [Required]
        public Sender Sender { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
