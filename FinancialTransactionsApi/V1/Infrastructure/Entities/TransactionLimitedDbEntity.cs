using Amazon.DynamoDBv2.DataModel;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    [DynamoDBTable("Transactions", LowerCamelCaseProperties = true)]
    public class TransactionLimitedDbEntity
    {
        [DynamoDBRangeKey]
        [DynamoDBProperty(AttributeName = "id")]
        public Guid Id { get; set; }

        [DynamoDBHashKey]
        [DynamoDBProperty(AttributeName = "target_id")]
        public Guid TargetId { get; set; }

        [DynamoDBProperty(AttributeName = "transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [DynamoDBProperty(AttributeName = "paid_amount")]
        public decimal PaidAmount { get; set; }

        [DynamoDBProperty(AttributeName = "charged_amount")]
        public decimal ChargedAmount { get; set; }

        [DynamoDBProperty(AttributeName = "balance_amount")]
        public decimal BalanceAmount { get; set; }

        [DynamoDBProperty(AttributeName = "housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }
    }
}
