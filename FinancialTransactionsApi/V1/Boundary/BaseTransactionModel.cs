using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Boundary
{
    public abstract class BaseTransactionModel
    {
        public Guid TargetId { get; set; }

        public Guid? AssetId { get; set; }

        public string AssetType { get; set; }

        public string TenancyAgreementRef { get; set; }

        public string PropertyRef { get; set; }

        public DateTime PostDate { get; set; }

        public TargetType TargetType { get; set; }

        [Range(1, 53)]
        public short PeriodNo { get; set; }

        public string TransactionSource { get; set; }

        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal TransactionAmount { get; set; }

        public string PaymentReference { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public string BankAccountNumber { get; set; }

        [AllowNull]
        public string SortCode { get; set; }

        public bool IsSuspense { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal PaidAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal ChargedAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal BalanceAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal HousingBenefitAmount { get; set; }

        public string Address { get; set; }

        public string Fund { get; set; }

        public short FinancialYear { get; set; }

        public short FinancialMonth { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
