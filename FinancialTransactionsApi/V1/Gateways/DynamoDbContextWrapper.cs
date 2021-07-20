using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbContextWrapper
    {
        public  virtual Task<List<TransactionDbEntity>> ScanAsync(IDynamoDBContext context, IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig = null)
        {
            return context.ScanAsync<TransactionDbEntity>(conditions, operationConfig).GetRemainingAsync();
        }

        public virtual Task<TransactionDbEntity> LoadAsync(IDynamoDBContext context, Guid id, DynamoDBOperationConfig operationConfig = null)
        {
            return context.LoadAsync<TransactionDbEntity>(id);
        }
    }
}
