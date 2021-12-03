using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Domain
{
    public class Transaction
    {
        [NonEmptyGuid]
        public Guid Id { get; set; }
        [NotNull]
        public Guid TargetId { get; set; }

        public TargetType TargetType { get; set; }
        [Required]
        public short PeriodNo { get; set; }
        [Required]
        public short FinancialYear { get; set; }
        [Required]
        public short FinancialMonth { get; set; }
        [Required]
        public string TransactionSource { get; set; }
        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }
        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal TransactionAmount { get; set; }
        [Required]
        public string PaymentReference { get; set; }
        [AllowNull]
        public string BankAccountNumber { get; set; }
        [Required]
        public bool IsSuspense { get; set; }
        [AllowNull]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PaidAmount { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal ChargedAmount { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal BalanceAmount { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal HousingBenefitAmount { get; set; }

        public string Address { get; set; }
        [Required]
        public Person Person { get; set; }
        [Required]
        public string Fund { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
