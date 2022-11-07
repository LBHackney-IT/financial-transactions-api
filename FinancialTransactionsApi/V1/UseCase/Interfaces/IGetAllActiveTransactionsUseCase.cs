using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllActiveTransactionsUseCase
    {
        public Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(GetActiveTransactionsRequest request);
    }
}
