using Amazon.DynamoDBv2.DataModel;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Entities
{
    public class PersonDbEntity
    {
        [DynamoDBProperty(AttributeName = "id")]
        public Guid Id { get; set; }

        [DynamoDBProperty(AttributeName = "full_name")]
        public string FullName { get; set; }
    }
}
