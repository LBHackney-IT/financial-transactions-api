using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetSuspenseAccountUseCase : IGetSuspenseAccountUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetSuspenseAccountUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<ResponseWrapper<IEnumerable<TransactionResponse>>> ExecuteAsync(SuspenseAccountQuery query)
        {
            var result = await _gateway.GetPagedSuspenseAccountTransactionsAsync(query).ConfigureAwait(false);

            return result?.ToResponseWrapper();

        }
    }
}
