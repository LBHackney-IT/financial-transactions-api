using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbContextWrapper
    {
        public  virtual Task<List<TransactionDbEntity>> ScanAsync(IDynamoDBContext context, IEnumerable<ScanCondition> conditions, DynamoDBOperationConfig operationConfig = null)
        {
            return context.ScanAsync<TransactionDbEntity>(conditions, operationConfig).GetRemainingAsync();
        }

     
    }
}
