using FinancialTransactionsApi.V1.Boundary.Request;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IAddUseCase
    {
        public Task<TransactionResponseObject> ExecuteAsync(TransactionRequest transaction);
    }
}
