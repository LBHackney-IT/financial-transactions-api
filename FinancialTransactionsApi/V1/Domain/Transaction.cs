using System;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionsApi.V1.Domain
{
    public class Transaction : TransactionBase
    {
        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }
        [Precision(18, 2)]
        [GreatAndEqualThan("0.0")]
        public decimal TransactionAmount { get; set; }
        [AllowNull]
        public string BankAccountNumber { get; set; }
        [AllowNull]
        public string SortCode { get; set; }
        [AllowNull]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
