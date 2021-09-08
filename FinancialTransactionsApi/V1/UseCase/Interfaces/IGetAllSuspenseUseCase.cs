using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllSuspenseUseCase
    {
        public Task<List<TransactionResponse>> ExecuteAsync(SuspenseTransactionsSearchRequest query);
    }
}
