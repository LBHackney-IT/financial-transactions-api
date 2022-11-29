using System;
using FinancialTransactionsApi.V1.Factories;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Boundary.Request;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Specs;
using Hackney.Shared.Finance.Pagination;

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
            var spec = new GetTransactionByTargetTypeAndTargetIdSpecification(targetType, targetId, startDate, endDate);

            var response = _databaseContext.Transactions.Where(spec.Criteria);

            return await Task.FromResult(response.AsEnumerable().ToDomain()).ConfigureAwait(false);
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var response = _databaseContext.Transactions.AsNoTracking().Where(t => t.Id == id);

            return await Task.FromResult(response.FirstOrDefault()?.ToDomain()).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Transaction>> GetPagedTransactionsAsync(TransactionQuery query)
        {
            var spec = new GetTransactionByDateSpecification(query.StartDate ?? new DateTime(), query.EndDate ?? DateTime.Now);

            var count = _databaseContext.Transactions.Where(spec.Criteria).Count();

            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / query.PageSize));

            var page = query.Page <= lastPage ? query.Page : lastPage;

            var itemStart = query.Page == 1 ? 0 : page * query.PageSize;

            var response = _databaseContext.Transactions.Where(spec.Criteria).Skip(itemStart).Take(query.PageSize);

            if (query.TransactionType.HasValue)
            {
                response = response.Where(x => x.TransactionType == query.TransactionType.ToString());
            }

            var result = await Task.FromResult(response.AsEnumerable()).ConfigureAwait(false);

            return result.ToDomain();
        }

        public Task AddAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<bool> AddBatchAsync(List<Transaction> transactions) => throw new NotImplementedException();

        public Task UpdateSuspenseAccountAsync(Transaction transaction) => throw new NotImplementedException();

        public async Task<Paginated<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query)
        {
            var spec = new GetTransactionBySuspenseAccountSpecification();

            var count = _databaseContext.Transactions.Where(spec.Criteria).Count();

            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / query.PageSize));

            var page = query.Page <= lastPage ? query.Page : lastPage;

            var itemStart = query.Page == 1 ? 0 : page * query.PageSize;

            var response = _databaseContext.Transactions.Where(spec.Criteria).Skip(itemStart).Take(query.PageSize);

            var result = await Task.FromResult(response.AsEnumerable()).ConfigureAwait(false);

            return new Paginated<Transaction>
            {
                Results = result.Select(x => x.ToDomain()),
                TotalResultCount = count,
                PageSize = query.PageSize,
                CurrentPage = query.Page
            };
        }

        public async Task<IEnumerable<Transaction>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest)
        {
            getActiveTransactionsRequest.PeriodEndDate = getActiveTransactionsRequest.PeriodEndDate ?? DateTime.UtcNow;

            var spec = new GetTransactionByDateSpecification(getActiveTransactionsRequest.PeriodStartDate, getActiveTransactionsRequest.PeriodEndDate);

            var count = _databaseContext.Transactions.Where(spec.Criteria).Count();

            var lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(count) / getActiveTransactionsRequest.PageSize));

            var page = getActiveTransactionsRequest.PageNumber <= lastPage ? getActiveTransactionsRequest.PageNumber : lastPage;

            var itemStart = getActiveTransactionsRequest.PageNumber == 1 ? 0 : page * getActiveTransactionsRequest.PageSize;

            var response = _databaseContext.Transactions.Where(spec.Criteria).Skip(itemStart).Take(getActiveTransactionsRequest.PageSize);

            var result = await Task.FromResult(response.AsEnumerable()).ConfigureAwait(false);

            return result.ToDomain();
        }

        public Task<IEnumerable<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query) => throw new NotImplementedException();
    }
}
