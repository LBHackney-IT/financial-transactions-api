using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllActiveTransactionsUseCase : IGetAllActiveTransactionsUseCase
    {
        private readonly ITransactionGateway _transactionGateway;

        public GetAllActiveTransactionsUseCase(ITransactionGateway transactionGateway)
        {
            _transactionGateway = transactionGateway;
        }

        public async Task<ResponseWrapper<IEnumerable<TransactionResponse>>> ExecuteAsync(GetActiveTransactionsRequest request)
        {
            Paginated<Transaction> response = await _transactionGateway.GetAllActive(request).ConfigureAwait(false);

            return response.Results?.ToResponseWrapper();
        }
    }
}
