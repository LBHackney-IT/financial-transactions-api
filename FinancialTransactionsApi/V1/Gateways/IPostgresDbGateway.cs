using System;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;


namespace FinancialTransactionsApi.V1.Gateways
{
    public interface IPostgresDbGateway
    {
        public Task<TransactionPostgres> GetTransactionByIdAsync(Guid targetId, Guid id);
    }
}