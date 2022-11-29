using System;
using System.ComponentModel.DataAnnotations.Schema;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    [Table("Transactions")]
    public class TransactionDbEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("target_id")]
        public Guid TargetId { get; set; }

        [Column("target_type")]
        public TargetType TargetType { get; set; }

        [Column("period_no")]
        public short PeriodNo { get; set; }

        [Column("financial_year")]
        public short FinancialYear { get; set; }

        [Column("financial_month")]
        public short FinancialMonth { get; set; }

        [Column("transaction_source")]
        public string TransactionSource { get; set; }

        [Column("transaction_type")]
        public TransactionType TransactionType { get; set; }

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Column("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [Column("payment_reference")]
        public string PaymentReference { get; set; }

        [Column("bank_account_number")]
        public string BankAccountNumber { get; set; }

        [Column("sort_code")]
        public string SortCode { get; set; }

        [Column("is_suspense")]
        public bool IsSuspense => TargetId == Guid.Empty;

        [NotMapped]
        [Column("suspense_resolution_info")]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }

        [Column("paid_amount")]
        public decimal PaidAmount { get; set; }

        [Column("charged_amount")]
        public decimal ChargedAmount { get; set; }

        [Column("balance_amount")]
        public decimal BalanceAmount { get; set; }

        [Column("housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("sender")]
        public Sender Sender { get; set; }

        [Column("fund")]
        public string Fund { get; set; }

        [Column("last_updated_by")]
        public string LastUpdatedBy { get; set; }

        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }
    }
}
