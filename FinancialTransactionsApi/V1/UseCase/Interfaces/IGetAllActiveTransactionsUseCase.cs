using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using Hackney.Core.DynamoDb;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllActiveTransactionsUseCase
    {
        public Task<PagedResult<TransactionLimitedModel>> ExecuteAsync(GetActiveTransactionsRequest getActiveTransactionsRequest);
    }
}
