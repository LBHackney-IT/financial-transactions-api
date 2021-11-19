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
        private const string Pk = "#lbhtransaction";

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
        }


        public async Task AddAsync(Transaction transaction)
        {
            var dbEntity = transaction.ToDatabase();
            await _dynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);
        }

        public async Task AddRangeAsync(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                await AddAsync(transaction).ConfigureAwait(false);
            }
        }

        public async Task<TransactionList> GetAllTransactionsAsync(TransactionQuery query)
        {

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                KeyConditionExpression = "pk = :V_pk",
                FilterExpression = "target_id =:V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":V_pk", new AttributeValue {S = Pk}},
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
                        {":V_pk", new AttributeValue {S = Pk}},
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
            var data = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(Pk, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task<List<Transaction>> GetAllTransactionsForTheYearAsync(ExportTransactionQuery query)
        {

            var firstDay = new DateTime(DateTime.Now.Year, 1, 1);
            var lastDay = new DateTime(DateTime.Now.Year, 12, 31);
            string startDate = firstDay.ToString(AWSSDKUtils.ISO8601DateFormat);

            string endDate = lastDay.ToString(AWSSDKUtils.ISO8601DateFormat);
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                KeyConditionExpression = "pk = :V_pk",
                FilterExpression = "target_id =:V_target_id AND transaction_date BETWEEN :V_startDate AND :V_endDate",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":V_pk", new AttributeValue {S = Pk}},
                     {":V_target_id", new AttributeValue {S = query.TargetId.ToString()}},
                     {":V_startDate", new AttributeValue { S = startDate }},
                     {":V_endDate", new AttributeValue { S = endDate }}
                    }
            };

            if (query.TransactionType != null)
            {
                queryRequest.FilterExpression += " AND transaction_type = :V_transaction_type";
                queryRequest.ExpressionAttributeValues.Add(":V_transaction_type", new AttributeValue { S = query.TransactionType.ToString() });
            }

            var result = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);
            var transactions = result.ToTransactions();
            return transactions;
        }

        public async Task<List<Transaction>> GetAllTransactionRecordAsync(ExportTransactionQuery query)
        {
            DateTime firstDay;
            DateTime lastDay;
            if (query.StatementType == StatementType.Quaterly)
            {

                firstDay = DateTime.UtcNow;
                lastDay = firstDay.AddMonths(-3).AddDays(-1);

            }
            else
            {
                firstDay = DateTime.UtcNow;
                lastDay = firstDay.AddMonths(-12);
            }

            var config = new DynamoDBOperationConfig()
            {
                QueryFilter = new List<ScanCondition>() {
                    new ScanCondition("TargetId", ScanOperator.Equal, query.TargetId),
                    new ScanCondition("TransactionDate", ScanOperator.Between,lastDay,firstDay)
                }
            };
            var response = await _dynamoDbContext.QueryAsync<TransactionDbEntity>(Pk, config).GetRemainingAsync().ConfigureAwait(false);
            if (response.Count < 1) return new List<Transaction>();
            var result = response.Select(x => x.ToDomain()).OrderBy(_ => _.TransactionDate).ToList();
            return result;
        }

        public async Task<List<Transaction>> GetAllTransactionByDateAsync(TransactionExportRequest request)
        {

            var config = new DynamoDBOperationConfig()
            {
                QueryFilter = new List<ScanCondition>() {
                    new ScanCondition("TargetId", ScanOperator.Equal,request.TargetId),
                    new ScanCondition("TransactionDate", ScanOperator.Between, request.StartDate,request.EndDate)
                }
            };
            if (request.TransactionType != null)
            {
                config.QueryFilter.Add(new ScanCondition("TransactionType", ScanOperator.Equal, request.TransactionType));
            }
            var response = await _dynamoDbContext.QueryAsync<TransactionDbEntity>(Pk, config).GetRemainingAsync().ConfigureAwait(false);
            if (response.Count < 1) return new List<Transaction>();
            var result = response.Select(x => x.ToDomain()).OrderBy(_ => _.TransactionDate).ToList();
            return result;
        }
    }
}
