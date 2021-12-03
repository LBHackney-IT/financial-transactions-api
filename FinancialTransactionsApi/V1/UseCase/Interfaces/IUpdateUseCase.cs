using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IUpdateUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(Transaction transaction, Guid id);
    }
}
