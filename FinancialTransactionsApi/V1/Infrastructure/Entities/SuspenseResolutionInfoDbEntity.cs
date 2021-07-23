using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Infrastructure.Conventers;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    public class SuspenseResolutionInfoDbEntity
    {
        [DynamoDBProperty(AttributeName = "resolution_date", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime? ResolutionDate { get; set; }

        [DynamoDBProperty(AttributeName = "is_resolve")]
        public bool IsResolve { get; set; }

        [DynamoDBProperty(AttributeName = "note")]
        public string Note { get; set; }
    }
}
