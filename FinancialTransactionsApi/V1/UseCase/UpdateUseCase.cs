using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class UpdateUseCase : IUpdateUseCase
    {
        private readonly ITransactionGateway _gateway;

        public UpdateUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponse> ExecuteAsync(Transaction transaction, Guid id)
        {
            transaction.FinancialMonth = (short) transaction.TransactionDate.Month;

            transaction.FinancialYear = (short) transaction.TransactionDate.Year;
            transaction.Id = id;
            transaction.LastUpdatedAt = DateTime.UtcNow;

            await _gateway.UpdateAsync(transaction).ConfigureAwait(false);

            return transaction.ToResponse();
        }
    }
}
