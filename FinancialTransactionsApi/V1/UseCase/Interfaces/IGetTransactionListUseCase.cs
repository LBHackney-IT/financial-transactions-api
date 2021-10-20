using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionListUseCase
    {
        Task<GetTransactionListResponse> ExecuteAsync(TransactionSearchRequest transactionSearchRequest);
    }
}
