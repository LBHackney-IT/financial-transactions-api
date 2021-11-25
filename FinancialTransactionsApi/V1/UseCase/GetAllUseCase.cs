using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
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

        public async Task<PagedResult<TransactionResponse>> ExecuteAsync(TransactionQuery query)
        {
            var transactions =
                await _gateway.GetPagedTransactionsAsync(query)
                    .ConfigureAwait(false);

            return new PagedResult<TransactionResponse>(transactions.Results.ToResponse(), transactions.PaginationDetails);

        }
    }
}
