using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetSuspenseAccountUseCase
    {
        public Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(SuspenseAccountQuery query);
    }
}
