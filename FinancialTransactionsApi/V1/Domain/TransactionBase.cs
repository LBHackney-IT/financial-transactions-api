using System.ComponentModel.DataAnnotations.Schema;
using System;
using FinancialTransactionsApi.V1.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace FinancialTransactionsApi.V1.Domain
{
    public abstract class TransactionBase
    {
        [NonEmptyGuid]
        public Guid Id { get; set; }

        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [NotNull]
        public Guid TargetId { get; set; }

        public TargetType TargetType { get; set; }

        public Guid AssetId { get; set; }

        public string AssetType { get; set; }

        public string TenancyAgreementRef { get; set; }

        public string PropertyRef { get; set; }

        [Required]
        public short PeriodNo { get; set; }

        [Required]
        public string TransactionSource { get; set; }

        public DateTime PostDate { get; set; }

        public decimal RealValue { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal PaidAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal ChargedAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal BalanceAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal HousingBenefitAmount { get; set; }

        public string Address { get; set; }

        [Required]
        public string Fund { get; set; }

        [Required]
        public short FinancialYear { get; set; }

        [Required]
        public short FinancialMonth { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
