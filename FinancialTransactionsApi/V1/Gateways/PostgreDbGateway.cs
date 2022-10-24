using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Domain;
using System;
using FinancialTransactionsApi.V1.Factories;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Boundary.Request;
using Hackney.Core.DynamoDb;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FinancialTransactionsApi.V1.Infrastructure.Specs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinancialTransactionsApi.V1.Gateways
{
    public class PostgreDbGateway : ITransactionGateway
    {
        private readonly DatabaseContext _databaseContext;
        public PostgreDbGateway(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Transaction>> GetByTargetId(string targetType, Guid targetId)
        {
            if (targetId == Guid.Empty) throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");

            var spec = new GetTransactionByTargetTypeAndTargetId(targetType, targetId);

            var response = await _databaseContext.TransactionEntities.Where(spec.Criteria).ToListAsync().ConfigureAwait(false);

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
            getActiveTransactionsRequest.PeriodEndDate = getActiveTransactionsRequest.PeriodEndDate ?? DateTime.UtcNow;

            var spec = new GetTransactionByDate(getActiveTransactionsRequest.PeriodStartDate, getActiveTransactionsRequest.PeriodEndDate);

            var count = _databaseContext.TransactionEntities.Where(spec.Criteria).Count();

            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / getActiveTransactionsRequest.PageSize));

            var page = getActiveTransactionsRequest.Page <= lastPage ? getActiveTransactionsRequest.Page : lastPage;

            var itemStart = getActiveTransactionsRequest.Page == 1 ? 0 : page * getActiveTransactionsRequest.PageSize;

            var response = await _databaseContext.TransactionEntities.Where(spec.Criteria).Skip(itemStart).Take(getActiveTransactionsRequest.PageSize).ToListAsync().ConfigureAwait(false);

            return new PagedResult<Transaction>(response.Select(x => x.ToDomain()), new PaginationDetails(string.Empty));
        }

        public Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query) => throw new NotImplementedException();
    }
}
