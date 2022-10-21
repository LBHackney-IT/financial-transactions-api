using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Domain;
using System;
using FinancialTransactionsApi.V1.Factories;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Boundary.Request;
using Hackney.Core.DynamoDb;
using FinancialTransactionsApi.V1.Boundary.Response;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using Amazon.DynamoDBv2.DocumentModel;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class PostgreDbGateway : ITransactionGateway
    {
        private readonly DatabaseContext _databaseContext;
        public PostgreDbGateway(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Transaction>> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty) throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");

            var response = await _databaseContext.TransactionEntities.Where(x => x.TargetId == targetId).ToListAsync().ConfigureAwait(false);

            return response?.ToDomain();
        }

        public Task<Transaction> GetTransactionByIdAsync(Guid targetId, Guid id) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedTransactionsAsync(TransactionQuery query) => throw new NotImplementedException();

        public Task AddAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<bool> AddBatchAsync(List<Transaction> transactions) => throw new NotImplementedException();

        public Task UpdateSuspenseAccountAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<List<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query) => throw new NotImplementedException();

        public async Task<PagedResult<Transaction>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest)
        {
            getActiveTransactionsRequest.PeriodEndDate = getActiveTransactionsRequest.PeriodEndDate ?? DateTime.Now;

            Expression<Func<TransactionEntity, bool>> predicate = (x => x.TransactionDate >= getActiveTransactionsRequest.PeriodStartDate && x.TransactionDate <= getActiveTransactionsRequest.PeriodEndDate);

            var response = await _databaseContext.TransactionEntities.Where(predicate).ToListAsync().ConfigureAwait(false);

            return new PagedResult<Transaction>(response.Select(x => x.ToDomain()), new PaginationDetails(string.Empty));
        }

        public Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query) => throw new NotImplementedException();
    }
}
