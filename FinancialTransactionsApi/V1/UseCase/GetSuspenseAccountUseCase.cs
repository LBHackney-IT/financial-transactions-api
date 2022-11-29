using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Shared.Finance.Pagination;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetSuspenseAccountUseCase : IGetSuspenseAccountUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetSuspenseAccountUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Paginated<Transaction>> ExecuteAsync(SuspenseAccountQuery query)
        {
            return await _gateway.GetPagedSuspenseAccountTransactionsAsync(query).ConfigureAwait(false);
        }
    }
}
