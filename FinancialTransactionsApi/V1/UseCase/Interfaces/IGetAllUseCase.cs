using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        public Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionQuery query);
    }
}
