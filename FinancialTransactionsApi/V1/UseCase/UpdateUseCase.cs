using FinancialTransactionsApi.V1.Boundary.Request;
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

        public async Task ExecuteAsync(UpdateTransactionRequest transaction, Guid id)
        {
            var transactionDomain = transaction.ToDomain();

            transactionDomain.Id = id;

            await _gateway.UpdateAsync(transactionDomain).ConfigureAwait(false);
        }
    }
}
