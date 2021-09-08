using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Gateways
{
    public interface ITransactionGateway
    {
        public Task<Transaction> GetTransactionByIdAsync(Guid id);
        public Task<List<Transaction>> GetAllTransactionsAsync(Guid targetid, TransactionType? transactionType, DateTime? startDate, DateTime? endDate);
        public Task<List<Transaction>> GetAllSuspenseAsync(SuspenseTransactionsSearchRequest request);

        public Task AddAsync(Transaction transaction);
        public Task AddRangeAsync(List<Transaction> transactions);

        public Task UpdateAsync(Transaction transaction);

    }
}
