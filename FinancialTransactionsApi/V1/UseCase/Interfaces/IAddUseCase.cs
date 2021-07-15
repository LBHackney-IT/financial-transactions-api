using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IAddUseCase
    {
        public Task<TransactionResponseObject> ExecuteAsync(Transaction transaction);
    }
}
