using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionsApi.V1.Domain;

namespace TransactionsApi.V1.Gateways
{
    public interface ITransactionGateway
    {
        public Task<Transaction> GetTransactionByIdAsync(Guid id);
        public Task<List<Transaction>> GetAllTransactionsAsync(Guid targetid, string transactionType);

        public Task AddAsync(Transaction transaction);
        public Task AddRangeAsync(List<Transaction> transactions);

    }
}
