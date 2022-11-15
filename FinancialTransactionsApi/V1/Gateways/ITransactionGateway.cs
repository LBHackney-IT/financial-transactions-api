using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

namespace FinancialTransactionsApi.V1.Gateways
{
    public interface ITransactionGateway
    {
        public Task<IEnumerable<Transaction>> GetByTargetId(string targetType, Guid targetId, DateTime? startDate, DateTime? endDate);
        public Task<Transaction> GetTransactionByIdAsync(Guid id);
        public Task<Paginated<Transaction>> GetPagedTransactionsAsync(TransactionQuery query);
        public Task AddAsync(Transaction transaction);
        public Task<bool> AddBatchAsync(List<Transaction> transactions);
        public Task UpdateSuspenseAccountAsync(Transaction transaction);
        public Task<IEnumerable<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query);
        public Task<IEnumerable<Transaction>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest);
        public Task<IEnumerable<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query);
    }
}
