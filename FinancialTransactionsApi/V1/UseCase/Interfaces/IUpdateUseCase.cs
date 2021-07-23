using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IUpdateUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(UpdateTransactionRequest transaction, Guid id);
    }
}
