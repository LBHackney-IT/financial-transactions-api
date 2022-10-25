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

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data = await _databaseContext.TransactionEntities.AsNoTracking().Where(t => t.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            return data?.ToDomain();
        }

        public Task<PagedResult<Transaction>> GetPagedTransactionsAsync(TransactionQuery query) => throw new NotImplementedException();

        public Task AddAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<bool> AddBatchAsync(List<Transaction> transactions) => throw new NotImplementedException();

        public Task UpdateSuspenseAccountAsync(Transaction transaction) => throw new NotImplementedException();

        public Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query) => throw new NotImplementedException();

        public Task<PagedResult<TransactionLimitedModel>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest) => throw new NotImplementedException();

        public Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query) => throw new NotImplementedException();
    }
}
