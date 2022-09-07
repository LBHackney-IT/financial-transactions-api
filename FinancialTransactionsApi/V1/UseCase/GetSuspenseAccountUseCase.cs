using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetSuspenseAccountUseCase : IGetSuspenseAccountUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetSuspenseAccountUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<PagedResult<TransactionResponse>> ExecuteAsync(SuspenseAccountQuery query)
        {
            var transactions =
                await _gateway.GetPagedSuspenseAccountTransactionsAsync(query)
                    .ConfigureAwait(false);

            return new PagedResult<TransactionResponse>(transactions.Results.ToResponse(), transactions.PaginationDetails);

        }
    }
}
