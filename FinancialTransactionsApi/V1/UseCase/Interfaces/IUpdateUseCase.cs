using FinancialTransactionsApi.V1.Boundary.Request;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IUpdateUseCase
    {
        public Task ExecuteAsync(UpdateTransactionRequest transaction, Guid id);
    }
}
