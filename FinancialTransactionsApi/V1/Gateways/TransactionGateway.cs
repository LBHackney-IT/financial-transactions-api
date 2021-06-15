using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Infrastructure;

namespace TransactionsApi.V1.Gateways
{
    public class TransactionGateway : ITransactionGateway
    {
        private readonly DatabaseContext _databaseContext;

        public TransactionGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _databaseContext.TransactionEntities.AddAsync(transaction.ToDatabase()).ConfigureAwait(false);
        }

        public async Task AddRangeAsync(List<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                await AddAsync(transaction).ConfigureAwait(false);
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid targetid, string transactionType)
        {
            IQueryable<TransactionDbEntity> data =
                (IQueryable<TransactionDbEntity>) _databaseContext
                .TransactionEntities
                .Where(p =>
                    (transactionType == null ? true : p.TransactionType == transactionType)
                    &&
                    (targetid == null ? true : p.TargetId == targetid)
                );
            return await data.Select(w => w.ToDomain()).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            var data = await _databaseContext.TransactionEntities.FindAsync(id).ConfigureAwait(false);
            return data?.ToDomain();
        }
    }
}
