using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetByTargetIdUseCase : IGetByTargetIdUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetByTargetIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<ResponseWrapper<IEnumerable<TransactionResponse>>> ExecuteAsync(Guid targetId)
        {
            var response = await _gateway.GetByTargetId(targetId).ConfigureAwait(false);

            return response?.ToResponseWrapper();
        }
    }
}
