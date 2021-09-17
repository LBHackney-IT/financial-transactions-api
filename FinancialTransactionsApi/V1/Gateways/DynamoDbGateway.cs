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
using FinancialTransactionsApi.V1.Infrastructure.Entities;

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

        public async Task<TransactionList> GetAllTransactionsAsync(TransactionQuery query)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                IndexName = "target_id_dx",
                KeyConditionExpression = "target_id = :V_target_id",
                FilterExpression = "transaction_type = :V_transaction_type",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":V_target_id", new AttributeValue {S = query.TargetId.ToString()}},
                        {":V_transaction_type", new AttributeValue {S = query.TransactionType.ToString()}}
                    }
            };
            var result = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);
            var transactions = result.ToTransactions();
            var filteredList = new List<Transaction>();
            if (query.StartDate.HasValue)
            {
                if (!query.EndDate.HasValue)
                {
                    query.EndDate = DateTime.Now;
                }
                filteredList = transactions.Where(x => x.TransactionDate >= query.StartDate && x.TransactionDate <= query.EndDate).ToList();
            }
            return new TransactionList
            {
                Transactions = filteredList.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList(),
                Total = filteredList.Count
            };
        }
        public async Task<TransactionList> GetAllSuspenseAsync(SuspenseTransactionsSearchRequest request)
        {
            #region Query Execution
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Transactions",
                IndexName = "is_suspense_dx",
                KeyConditionExpression = "is_suspense = :V_is_suspense",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
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
            var data = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }
    }
}
