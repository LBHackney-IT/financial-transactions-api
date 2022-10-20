using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Helpers;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        public Task<ResponseWrapper<TransactionResponse>> ExecuteAsync(Guid id);
    }
}
