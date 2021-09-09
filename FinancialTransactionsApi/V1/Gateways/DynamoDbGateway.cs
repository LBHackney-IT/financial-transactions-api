using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbGateway : ITransactionGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly DynamoDbContextWrapper _wrapper;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, DynamoDbContextWrapper wrapper, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _wrapper = wrapper;
            _amazonDynamoDb = amazonDynamoDb;
        }


        public async Task AddAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task AddRangeAsync(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                await AddAsync(transaction).ConfigureAwait(false);
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid targetId, TransactionType? transactionType, DateTime? startDate, DateTime? endDate)
        {
            List<ScanCondition> scanConditions = new List<ScanCondition>
            {
                new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.NotEqual, Guid.Empty)
            };

            if (transactionType != null)
            {
                scanConditions.Add(new ScanCondition("TransactionType", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, transactionType));
            }

            if (targetId != Guid.Empty)
            {
                scanConditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, targetId));
            }

            if (startDate.HasValue)
            {
                if (endDate == null)
                {
                    endDate = DateTime.Now;
                }

                scanConditions.Add(new ScanCondition("TransactionDate", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Between, startDate.Value, endDate.Value));
            }

            var data = await _wrapper
              .ScanAsync(_dynamoDbContext, scanConditions)
              .ConfigureAwait(false);

            return data.Select(p => p?.ToDomain()).ToList();
        }
        public async Task<List<Transaction>> GetAllSuspenseAsync(SuspenseTransactionsSearchRequest request)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "transactions",
                IndexName = "is_suspense_dx",
                KeyConditionExpression = "is_suspense = :V_is_suspense",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_is_suspense", new AttributeValue {S = "true"}}
                },
                ScanIndexForward = true
            };

            var result = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);

            List<Transaction> transactions = result.ToTransactions();
            return transactions;
        }
        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data = await _wrapper.LoadAsync(_dynamoDbContext, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }
    }
}
