using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionsApi.V1.Infrastructure
{
    [Table("transactions")]
    [DynamoDBTable("transactions", LowerCamelCaseProperties = true)]
    public class TransactionDbEntity
    {
        [DynamoDBHashKey]
        [DynamoDBProperty(AttributeName ="id")]
        public Guid Id { get; set; }
        [DynamoDBProperty(AttributeName = "target_id")]
        public Guid TargetId { get; set; }
        [DynamoDBProperty(AttributeName = "period_no")]
        public short PeriodNo { get; set; }
        [DynamoDBProperty(AttributeName = "financial_year")]
        public short FinancialYear { get; set; }
        [DynamoDBProperty(AttributeName = "financial_month")]
        public short FinancialMonth { get; set; }
        [DynamoDBProperty(AttributeName = "transaction_type")]
        public string TransactionType { get; set; }
        [DynamoDBProperty(AttributeName = "transaction_date", Converter =(typeof(DynamoDbDateTimeConverter)))]
        public DateTime TransactionDate { get; set; }
        [DynamoDBProperty(AttributeName = "transaction_amount")]
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// This is same as the Rent Account Number
        /// </summary>
        [DynamoDBProperty(AttributeName = "payment_reference")]
        public string PaymentReference { get; set; }
        [DynamoDBProperty(AttributeName = "paid_amount")]
        public decimal PaidAmount { get; set; }
        [DynamoDBProperty(AttributeName = "charged_amount")]
        public decimal ChargedAmount { get; set; }
        [DynamoDBProperty(AttributeName = "balance_amount")]
        public decimal BalanceAmount { get; set; }
        [DynamoDBProperty(AttributeName = "housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }
        [DynamoDBProperty(AttributeName = "asset_full_address")]
        public string AssetFullAddress { get; set; }
    }
}
