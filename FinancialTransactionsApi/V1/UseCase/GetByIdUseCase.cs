using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Helpers;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetByIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<ResponseWrapper<TransactionResponse>> ExecuteAsync(Guid id)
        {
            var data = await _gateway.GetTransactionByIdAsync(id).ConfigureAwait(false);

            return data?.ToResponseWrapper();
        }
    }
}
