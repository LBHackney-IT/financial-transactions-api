using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using Hackney.Core.DynamoDb;

namespace FinancialTransactionsApi.V1.Gateways
{
    public interface ITransactionGateway
    {
        public Task<IEnumerable<Transaction>> GetByTargetId(string targetType, Guid targetId, DateTime? startDate, DateTime? endDate);
        public Task<Transaction> GetTransactionByIdAsync(Guid targetId, Guid id);
        public Task<PaginatedResponse<Transaction>> GetPagedTransactionsAsync(TransactionQuery query);
        public Task AddAsync(Transaction transaction);
        public Task<bool> AddBatchAsync(List<Transaction> transactions);
        public Task UpdateSuspenseAccountAsync(Transaction transaction);
        public Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate);
        Task<PagedResult<Transaction>> GetPagedSuspenseAccountTransactionsAsync(SuspenseAccountQuery query);

        Task<PagedResult<TransactionLimitedModel>> GetAllActive(GetActiveTransactionsRequest getActiveTransactionsRequest);

        public Task<PagedResult<Transaction>> GetPagedTransactionsByTargetIdsAsync(TransactionByTargetIdsQuery query);
    }
}
