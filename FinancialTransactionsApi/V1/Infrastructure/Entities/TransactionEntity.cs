using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    [Table("transactions", Schema = "housingfinance_dbo")]
    public class TransactionEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("transactiontype")]
        public string TransactionType { get; set; }

        [Column("targetid")]
        public Guid TargetId { get; set; }

        [Column("targettype")]
        public string TargetType { get; set; }

        [Column("assetid")]
        public Guid AssetId { get; set; }

        [Column("assettype")]
        public string AssetType { get; set; }

        [Column("tenancyagreementref")]
        public string TenancyAgreementRef { get; set; }

        [Column("propertyref")]
        public string PropertyRef { get; set; }

        [Column("periodno")]
        public short PeriodNo { get; set; }

        [Column("transactionsource")]
        public string TransactionSource { get; set; }

        [Column("post_date")]
        public DateTime PostDate { get; set; }

        [Column("real_value")]
        public decimal RealValue { get; set; }

        [Column("paymentreference")]
        public string PaymentReference { get; set; }

        [Column("paidamount")]
        public decimal PaidAmount { get; set; }

        [Column("chargedamount")]
        public decimal ChargedAmount { get; set; }

        [Column("balanceamount")]
        public decimal BalanceAmount { get; set; }

        [Column("housingbenefitamount")]
        public decimal HousingBenefitAmount { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("fund")]
        public string Fund { get; set; }

        [Column("financialyear")]
        public short FinancialYear { get; set; }

        [Column("financialmonth")]
        public short FinancialMonth { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        [Column("createdby")]
        public string CreatedBy { get; set; }
    }
}
