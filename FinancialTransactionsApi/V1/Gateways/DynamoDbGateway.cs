using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbGateway : ITransactionGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
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

        public async Task<TransactionList> GetPagedTransactionsAsync(TransactionQuery query)
        {

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":V_target_id", new AttributeValue {S = query.TargetId.ToString()}}
                    }
            };

            if (query.TransactionType != null)
            {
                queryRequest.FilterExpression += "transaction_type = :V_transaction_type";
                queryRequest.ExpressionAttributeValues.Add(":V_transaction_type", new AttributeValue { S = query.TransactionType.ToString() });
            }
            if (query.StartDate.HasValue)
            {
                query.EndDate ??= DateTime.Now;
                string startDate = query.StartDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);

                string endDate = query.EndDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);
                queryRequest.FilterExpression += " AND transaction_date BETWEEN :V_startDate AND :V_endDate";
                queryRequest.ExpressionAttributeValues.Add(":V_startDate", new AttributeValue { S = startDate });
                queryRequest.ExpressionAttributeValues.Add(":V_endDate", new AttributeValue { S = endDate });
            }
            var result = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);
            var transactions = result.ToTransactions();

            return new TransactionList
            {
                Transactions = transactions.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList(),
                Total = transactions.Count
            };
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid targetId, Guid id)
        {
            var data = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(targetId, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }


        public async Task<List<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate)
        {

            List<TransactionDbEntity> transactionDetailsEntities = new List<TransactionDbEntity>();
            List<ScanCondition> scanConditions = new List<ScanCondition>();
            DynamoDBOperationConfig dbOperationConfig = null;
            if (!string.IsNullOrEmpty(transactionType))
            {
                scanConditions.Add(new ScanCondition(nameof(TransactionDbEntity.TransactionType), ScanOperator.Equal, transactionType));
            }
            if (startDate.HasValue)
            {
                endDate = endDate ?? startDate;
                scanConditions.Add(new ScanCondition(nameof(TransactionDbEntity.TransactionType), ScanOperator.Between, endDate.Value, startDate.Value));
            }
            if (scanConditions.Count > 0)
                dbOperationConfig = new DynamoDBOperationConfig() { QueryFilter = scanConditions };
            var queryResult = _dynamoDbContext.QueryAsync<TransactionDbEntity>(targetId, dbOperationConfig);
            while (!queryResult.IsDone)
                transactionDetailsEntities.AddRange(await queryResult.GetNextSetAsync().ConfigureAwait(false));

            return transactionDetailsEntities.ToDomain();
        }
    }
}
