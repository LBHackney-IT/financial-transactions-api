using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetByTargetIdsUseCase : IGetByTargetIdsUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetByTargetIdsUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionByTargetIdsQuery targetIdsQuery)
        {

            Paginated<Transaction> result = await _gateway.GetPagedTransactionsByTargetIdsAsync(targetIdsQuery).ConfigureAwait(false);

            return result.ToResponse();
        }
    }
}
