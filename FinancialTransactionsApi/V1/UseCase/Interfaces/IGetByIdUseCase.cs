using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        public Task<TransactionResponse> ExecuteAsync(Guid id);
    }
}
