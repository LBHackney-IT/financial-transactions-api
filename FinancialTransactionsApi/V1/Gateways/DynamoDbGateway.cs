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
        private const string PartitionKey = "#lbhtransaction";

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
        }


        public async Task AddAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase(PartitionKey)).ConfigureAwait(false);
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
                KeyConditionExpression = "pk = :V_pk",
                FilterExpression = "target_id =:V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":V_pk", new AttributeValue {S = PartitionKey} },
                     {":V_target_id", new AttributeValue {S = query.TargetId.ToString()}}
                    }
            };

            if (query.TransactionType != null)
            {
                queryRequest.FilterExpression += " AND transaction_type = :V_transaction_type";
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
        public async Task<TransactionList> GetAllSuspenseAsync(SuspenseTransactionsSearchRequest request)
        {
            #region Query Execution
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                KeyConditionExpression = "pk = :V_pk",
                FilterExpression = "is_suspense =:V_is_suspense",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":V_pk", new AttributeValue {S = PartitionKey}},
                        {":V_is_suspense", new AttributeValue {S = "true"}}
                    }
            };

            var result = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);

            var transactions = result.ToTransactions();
            if (request.SearchText != null)
            {
                transactions = transactions.Where(p =>
                    p.Person.FullName.ToLower().Contains(request.SearchText.ToLower()) ||
                    p.PaymentReference.ToLower().Contains(request.SearchText.ToLower()) ||
                    p.TransactionDate.ToString("F").Contains(request.SearchText.ToLower()) ||
                    p.BankAccountNumber.Contains(request.SearchText) ||
                    p.Fund.ToLower().Contains(request.SearchText.ToLower()) ||
                    p.BalanceAmount.ToString("F").Contains(request.SearchText)).ToList();
            }
            #endregion

            var dataList = transactions.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            return new TransactionList
            {
                Transactions = dataList,
                Total = transactions.Count
            };
        }
        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(PartitionKey, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase(PartitionKey)).ConfigureAwait(false);
        }


        public async Task<List<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate, int pageSize = 50)
        {
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();
            var results = new List<Transaction>();
            string paginationToken = "{}";
            var queryFilter = new QueryFilter();
            queryFilter.AddCondition("pk", QueryOperator.Equal, PartitionKey);
            queryFilter.AddCondition("target_id", QueryOperator.Equal, targetId);
            if (string.IsNullOrEmpty(transactionType))
            {
                queryFilter.AddCondition("transaction_type", QueryOperator.Equal, transactionType);
            }
            if (startDate.HasValue)
            {
                endDate = endDate ?? startDate;
                queryFilter.AddCondition("transaction_date", QueryOperator.Between, endDate.Value, startDate.Value);
            }
            do
            {
                var result1 = table.Query(new QueryOperationConfig
                {
                    Filter = queryFilter,
                    Limit = pageSize,
                    PaginationToken = paginationToken
                });
                var items = await result1.GetNextSetAsync().ConfigureAwait(false);
                paginationToken = result1.PaginationToken;

                var data = _dynamoDbContext.FromDocuments<TransactionDbEntity>(items);
                results.AddRange(data.Select(x => x.ToDomain()));
            }
            while (!string.Equals(paginationToken, "{}", StringComparison.Ordinal));

            return results.OrderBy(_ => _.TransactionDate).ToList();
        }
    }
}
