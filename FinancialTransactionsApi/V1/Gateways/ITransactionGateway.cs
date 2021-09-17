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
        public Task<TransactionList> GetAllTransactionsAsync(TransactionQuery query);
        public Task<TransactionList> GetAllSuspenseAsync(SuspenseTransactionsSearchRequest request);
        public Task AddAsync(Transaction transaction);
        public Task AddRangeAsync(List<Transaction> transactions);
        public Task UpdateAsync(Transaction transaction);

    }
}
