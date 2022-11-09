using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    [Table("Transactions")]
    public class TransactionLimitedDbEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("target_id")]
        public Guid TargetId { get; set; }

        [Column("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [Column("paid_amount")]
        public decimal PaidAmount { get; set; }

        [Column("charged_amount")]
        public decimal ChargedAmount { get; set; }

        [Column("balance_amount")]
        public decimal BalanceAmount { get; set; }

        [Column("housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }
    }
}
