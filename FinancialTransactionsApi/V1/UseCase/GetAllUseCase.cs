using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionQuery query)
        {
            var result = await _gateway.GetPagedTransactionsAsync(query).ConfigureAwait(false);

            return new PaginatedResponse<TransactionResponse>(result.Results.ToResponse(), result.MetaData.Pagination);
        }
    }
}
