using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IAddUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(Transaction transaction);
    }
}
