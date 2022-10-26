using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Hackney.Core.DynamoDb;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class DynamoDbGateway
    {
        private const string TARGETID = "target_id";
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IConfiguration _configuration;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IConfiguration configuration)
        {
            _dynamoDbContext = dynamoDbContext;
            _configuration = configuration;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task<bool> AddBatchAsync(List<Transaction> transactions)
        {
            var transactionBatch = _dynamoDbContext.CreateBatchWrite<TransactionDbEntity>();

            var items = transactions.ToDatabaseList();
            var maxBatchCount = _configuration.GetValue<int>("BatchProcessing:PerBatchCount");
            if (items.Count > maxBatchCount)
            {
                var loopCount = (items.Count / maxBatchCount) + 1;
                for (var start = 0; start < loopCount; start++)
                {
                    var itemsToWrite = items.Skip(start * maxBatchCount).Take(maxBatchCount);
                    transactionBatch.AddPutItems(itemsToWrite);
                    await transactionBatch.ExecuteAsync().ConfigureAwait(false);
                }
            }
            else
            {
                transactionBatch.AddPutItems(items);
                await transactionBatch.ExecuteAsync().ConfigureAwait(false);
            }

            return true;
        }

        public async Task<PagedResult<TransactionLimitedModel>> GetAllActive(GetActiveTransactionsRequest request)
        {
            var dbTransactions = new List<TransactionLimitedDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<TransactionLimitedDbEntity>();

            var filter = new ScanFilter();
            filter.AddCondition(TARGETID, ScanOperator.NotEqual, Guid.Empty);

            var scanConfig = new ScanOperationConfig
            {
                Limit = request.PageSize,
                PaginationToken = PaginationDetails.DecodeToken(request.PaginationToken),
                Filter = filter
            };

            if (request.PeriodStartDate.HasValue)
            {
                request.PeriodEndDate = request.PeriodEndDate.HasValue
                    ? request.PeriodEndDate.Value.Date.AddDays(1).AddMilliseconds(-1)
                    : DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

                string startDate = request.PeriodStartDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);
                string endDate = request.PeriodEndDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);

                scanConfig.Filter.AddCondition("created_at", ScanOperator.Between, startDate, endDate);
            }

            var search = table.Scan(scanConfig);
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
            if (resultsSet.Any())
            {
                dbTransactions.AddRange(_dynamoDbContext.FromDocuments<TransactionLimitedDbEntity>(resultsSet));
            }

            return new PagedResult<TransactionLimitedModel>(dbTransactions.Select(x => x.ToResponse()), new PaginationDetails(search.PaginationToken));
        }

        public async Task<PagedResult<Transaction>> GetPagedTransactionsAsync(TransactionQuery query)
        {
            int pageSize = query.PageSize;
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

        public async Task<PagedResult<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query)
        {
            int pageSize = query.PageSize;
            var dbTransactions = new List<TransactionDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();

            var queryConfig = new QueryOperationConfig
            {
                BackwardSearch = true,
                ConsistentRead = true,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                Filter = new QueryFilter(TARGETID, QueryOperator.Equal, Guid.Empty)
            };
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
            if (dbTransactions.Any() && !string.IsNullOrEmpty(query.SearchText))
            {
                dbTransactions = dbTransactions.Where(x => x.PaymentReference.Contains(query.SearchText) || x.Address.ToLower().Contains(query.SearchText.ToLower())
                   || x.Sender.FullName.ToLower().Contains(query.SearchText.ToLower()) || x.TransactionAmount.ToString().Contains(query.SearchText)
                   || x.BankAccountNumber.Contains(query.SearchText) || x.Fund.ToLower().Contains(query.SearchText.ToLower())).ToList();
            }

            return new PagedResult<Transaction>(dbTransactions.Select(x => x.ToDomain()), new PaginationDetails(paginationToken));

        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid targetId, Guid id)
        {
            var data = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(targetId, id).ConfigureAwait(false);

            return data?.ToDomain();
        }

        public async Task UpdateSuspenseAccountAsync(Transaction transaction)
        {
            await _dynamoDbContext.SaveAsync(transaction.ToDatabase()).ConfigureAwait(false);
            await _dynamoDbContext.DeleteAsync<TransactionDbEntity>(Guid.Empty, transaction.Id).ConfigureAwait(false);
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
                queryConfig.Filter.AddCondition("transaction_date", QueryOperator.Between, startDate, endDate);
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

        public async Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query)
        {
            if (query.TargetIds.Count > 1)
                return await GetPagedTransactionsByTargetIdsWithScanAsync(query).ConfigureAwait(false);

            return await GetPagedTransactionsByTargetIdWithQueryAsync(query).ConfigureAwait(false);

        }

        private async Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsWithScanAsync(TransactionByTargetIdsQuery query)
        {
            int pageSize = query.PageSize;
            var dbTransactions = new List<TransactionDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();
            var filter = new ScanFilter();
            if (query.TargetIds.Count > 0)
            {
                var objectValues = query.TargetIds.Select(x => (DynamoDBEntry) x).ToArray();
                filter.AddCondition(TARGETID, ScanOperator.In, objectValues);
            }
            if (query.StartDate.HasValue)
            {
                query.EndDate = query.EndDate.HasValue
                    ? query.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)
                    : DateTime.UtcNow.Date.AddDays(1).AddMilliseconds(-1);

                string startDate = query.StartDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);
                string endDate = query.EndDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);

                filter.AddCondition("transaction_date", ScanOperator.Between, startDate, endDate);
            }

            var scanConfig = new ScanOperationConfig
            {
                Limit = pageSize,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                Filter = filter
            };



            var search = table.Scan(scanConfig);
            var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);

            var paginationToken = search.PaginationToken;
            if (resultsSet.Any())
            {
                dbTransactions.AddRange(_dynamoDbContext.FromDocuments<TransactionDbEntity>(resultsSet));
                // Look ahead for any more, but only if we have a token
                if (!string.IsNullOrEmpty(PaginationDetails.EncodeToken(paginationToken)))
                {
                    scanConfig.PaginationToken = paginationToken;
                    scanConfig.Limit = 1;
                    search = table.Scan(scanConfig);
                    resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
                    if (!resultsSet.Any())
                        paginationToken = null;
                }
            }

            return new PagedResult<Transaction>(dbTransactions.Select(x => x.ToDomain()), new PaginationDetails(paginationToken));

        }

        private async Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdWithQueryAsync(TransactionByTargetIdsQuery query)
        {
            int pageSize = query.PageSize;
            var dbTransactions = new List<TransactionDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<TransactionDbEntity>();
            var filter = new QueryFilter();
            if (query.TargetIds.Count == 1)
            {
                var objectValue = query.TargetIds.FirstOrDefault();
                filter.AddCondition(TARGETID, QueryOperator.Equal, objectValue);
            }
            if (query.StartDate.HasValue)
            {
                query.EndDate = query.EndDate.HasValue
                    ? query.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)
                    : DateTime.UtcNow.Date.AddDays(1).AddMilliseconds(-1);

                string startDate = query.StartDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);
                string endDate = query.EndDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat);

                filter.AddCondition("transaction_date", QueryOperator.Between, startDate, endDate);
            }


            var queryConfig = new QueryOperationConfig
            {
                BackwardSearch = true,
                ConsistentRead = true,
                Filter = filter,
                PaginationToken = PaginationDetails.DecodeToken(query.PaginationToken),
                Limit = pageSize
            };



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

            return new PagedResult<Transaction>(dbTransactions.Select(x => x.ToDomain()), new PaginationDetails(search.PaginationToken));

        }
    }
}
