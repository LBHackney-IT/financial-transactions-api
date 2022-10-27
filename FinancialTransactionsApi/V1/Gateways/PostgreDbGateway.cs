using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Domain;
using System;
using FinancialTransactionsApi.V1.Factories;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Boundary.Request;
using Hackney.Core.DynamoDb;
using FinancialTransactionsApi.V1.Boundary.Response;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Amazon.Util;
using FinancialTransactionsApi.V1.Infrastructure.Specs;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class PostgreDbGateway : ITransactionGateway
    {
        private readonly DatabaseContext _databaseContext;
        public PostgreDbGateway(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Transaction>> GetByTargetId(string targetType, Guid targetId, DateTime? startDate, DateTime? endDate)
        {
            if (targetId == Guid.Empty) throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");

            var spec = new GetTransactionByTargetTypeAndTargetId(targetType, targetId, startDate, endDate);

            var response = await _databaseContext.Transactions.Where(spec.Criteria).ToListAsync().ConfigureAwait(false);

            return response?.ToDomain();
        }

        public Task<Transaction> GetTransactionByIdAsync(Guid targetId, Guid id) => throw new NotImplementedException();

        public async Task<PaginatedResponse<Transaction>> GetPagedTransactionsAsync(TransactionQuery query)
        {
            var transactionEntities = _databaseContext.Transactions.AsQueryable();

            if (query.TransactionType.HasValue)
            {
                transactionEntities = transactionEntities.Where(x => x.TransactionType == query.TransactionType.ToString());
            }

            if(query.StartDate.HasValue)
            {
                query.EndDate ??= DateTime.Now;
                transactionEntities = transactionEntities.Where(x => x.TransactionDate >= query.StartDate.Value && x.TransactionDate <= query.EndDate.Value);
            }

            var response = await transactionEntities.ToListAsync().ConfigureAwait(false);

            var page = query.Page > 1 ? query.Page : 0;

            var filteredResponse = response.Skip(page * query.PageSize).Take(query.PageSize).Select(x => x.ToDomain());

            return new PaginatedResponse<Transaction>(filteredResponse, new PaginationMetaData(filteredResponse.Count(), response.Count, 0, query.PageSize, query.Page));
        }

        public Task AddAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<bool> AddBatchAsync(List<Transaction> transactions) => throw new NotImplementedException();

        public Task UpdateSuspenseAccountAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query) => throw new NotImplementedException();

        public Task<PagedResult<TransactionLimitedModel>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query) => throw new NotImplementedException();
    }
}
