using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Util;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Hackney.Core.DynamoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbGateway : ITransactionGateway
    {
        private const int MAX_RESULTS = 10;
        private const string TARGETID = "target_id";
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }


        public async Task AddAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task<bool> AddBatchAsync(List<Transaction> transactions)
        {
            var estimateBatch = _dynamoDbContext.CreateBatchWrite<TransactionDbEntity>();

            var items = transactions.ToDatabaseList();
            int maxBatchCount = 1000;
            if (items.Count > maxBatchCount)
            {
                var loopCount = (items.Count / maxBatchCount) + 1;
                for (int start = 0; start < loopCount; start++)
                {
                    var itemsToWrite = items.Skip(start * maxBatchCount).Take(maxBatchCount);
                    estimateBatch.AddPutItems(itemsToWrite);
                    await estimateBatch.ExecuteAsync().ConfigureAwait(false);
                }
            }
            else
            {
                estimateBatch.AddPutItems(items);
                await estimateBatch.ExecuteAsync().ConfigureAwait(false);
            }

            return true;
        }

        public async Task<PagedResult<Transaction>> GetPagedTransactionsAsync(TransactionQuery query)
        {

            int pageSize = query.PageSize ?? MAX_RESULTS;
            var dbTransactions = new List<TransactionDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();

            var queryConfig = new QueryOperationConfig
            {
                BackwardSearch = true,
                ConsistentRead = true,
                Limit = pageSize,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                Filter = new QueryFilter(TARGETID, QueryOperator.Equal, query.TargetId)
            };
            if (query.TransactionType != null)
            {
                queryConfig.Filter.AddCondition("transaction_type", QueryOperator.Equal, query.TransactionType.ToString());
            }
            if (query.StartDate.HasValue)
            {
                query.EndDate ??= DateTime.Now;
                string startDate = query.StartDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);

                string endDate = query.EndDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);
                queryConfig.Filter.AddCondition("transaction_date", QueryOperator.Between, startDate, endDate);
            }
            var search = table.Query(queryConfig);
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);

            var paginationToken = search.PaginationToken;
            if (resultsSet.Any())
            {
                dbTransactions.AddRange(_dynamoDbContext.FromDocuments<TransactionDbEntity>(resultsSet));

                // Look ahead for any more, but only if we have a token
                if (!string.IsNullOrEmpty(PaginationDetails.EncodeToken(paginationToken)))
                {
                    queryConfig.PaginationToken = paginationToken;
                    queryConfig.Limit = 1;
                    search = table.Query(queryConfig);
                    resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
                    if (!resultsSet.Any())
                        paginationToken = null;
                }
            }

            return new PagedResult<Transaction>(dbTransactions.Select(x => x.ToDomain()), new PaginationDetails(paginationToken));

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

            var dbTransactions = new List<TransactionDbEntity>();
            string paginationToken = "{}";
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();

            var queryConfig = new QueryOperationConfig
            {
                BackwardSearch = true,
                ConsistentRead = true,
                Filter = new QueryFilter(TARGETID, QueryOperator.Equal, targetId),
                PaginationToken = paginationToken
            };
            if (!string.IsNullOrEmpty(transactionType))
            {
                queryConfig.Filter.AddCondition(nameof(TransactionDbEntity.TransactionType), QueryOperator.Equal, transactionType);
            }
            if (startDate.HasValue)
            {
                endDate ??= startDate;
                queryConfig.Filter.AddCondition("transaction_date", QueryOperator.Between, endDate, startDate);
            }


            do
            {
                var search = table.Query(queryConfig);
                paginationToken = search.PaginationToken;
                //_logger.LogDebug($"Querying {queryConfig.IndexName} index for targetId {query.TargetId}");
                var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
                if (resultsSet.Any())
                {
                    dbTransactions.AddRange(_dynamoDbContext.FromDocuments<TransactionDbEntity>(resultsSet));

                }
            }
            while (!string.Equals(paginationToken, "{}", StringComparison.Ordinal));

            return dbTransactions.ToDomain();
        }
    }
}
