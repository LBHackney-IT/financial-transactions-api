using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddUseCase : IAddUseCase
    {
        private readonly ITransactionGateway _gateway;

        public AddUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponse> ExecuteAsync(AddTransactionRequest transaction)
        {
            var transactionDomain = transaction.ToDomain();

            transactionDomain.Id = Guid.NewGuid();

            await _gateway.AddAsync(transactionDomain).ConfigureAwait(false);

            return transactionDomain.ToResponse();
        }
    }
}
