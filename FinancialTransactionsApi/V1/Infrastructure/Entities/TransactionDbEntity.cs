using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Domain;
using System;
using FinancialTransactionsApi.V1.Infrastructure.Converters;
using Hackney.Core.DynamoDb.Converters;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    [DynamoDBTable("Transactions", LowerCamelCaseProperties = true)]
    public class TransactionDbEntity
    {


        [DynamoDBRangeKey]
        [DynamoDBProperty(AttributeName = "id")]
        public Guid Id { get; set; }

        [DynamoDBHashKey]
        [DynamoDBProperty(AttributeName = "target_id")]
        public Guid TargetId { get; set; }

        [DynamoDBProperty(AttributeName = "target_type", Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public TargetType TargetType { get; set; }

        [DynamoDBProperty(AttributeName = "period_no")]
        public short PeriodNo { get; set; }

        [DynamoDBProperty(AttributeName = "financial_year")]
        public short FinancialYear { get; set; }

        [DynamoDBProperty(AttributeName = "financial_month")]
        public short FinancialMonth { get; set; }

        [DynamoDBProperty(AttributeName = "transaction_source")]
        public string TransactionSource { get; set; }

        [DynamoDBProperty(AttributeName = "transaction_type", Converter = typeof(DynamoDbEnumConverter<TransactionType>))]
        public TransactionType TransactionType { get; set; }

        [DynamoDBProperty(AttributeName = "transaction_date", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime TransactionDate { get; set; }

        [DynamoDBProperty(AttributeName = "transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [DynamoDBProperty(AttributeName = "payment_reference")]
        public string PaymentReference { get; set; }

        [DynamoDBProperty(AttributeName = "bank_account_number")]
        public string BankAccountNumber { get; set; }

        [DynamoDBProperty(AttributeName = "sort_code")]
        public string SortCode { get; set; }

        [DynamoDBProperty(AttributeName = "is_suspense", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsSuspense => TargetId == Guid.Empty;

        [DynamoDBProperty(AttributeName = "suspense_resolution_info", Converter = typeof(DynamoDbObjectConverter<SuspenseResolutionInfo>))]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }

        [DynamoDBProperty(AttributeName = "paid_amount")]
        public decimal PaidAmount { get; set; }

        [DynamoDBProperty(AttributeName = "charged_amount")]
        public decimal ChargedAmount { get; set; }

        [DynamoDBProperty(AttributeName = "balance_amount")]
        public decimal BalanceAmount { get; set; }

        [DynamoDBProperty(AttributeName = "housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }

        [DynamoDBProperty(AttributeName = "address")]
        public string Address { get; set; }

        [DynamoDBProperty(AttributeName = "sender", Converter = typeof(DynamoDbObjectConverter<Sender>))]
        public Sender Sender { get; set; }

        [DynamoDBProperty(AttributeName = "fund")]
        public string Fund { get; set; }

        [DynamoDBProperty(AttributeName = "last_updated_by")]
        public string LastUpdatedBy { get; set; }

        [DynamoDBProperty(AttributeName = "last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }

        [DynamoDBProperty(AttributeName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty(AttributeName = "created_by")]
        public string CreatedBy { get; set; }
    }
}
