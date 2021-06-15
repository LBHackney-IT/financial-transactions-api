using System;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;

namespace TransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        public Task<TransactionResponseObject> ExecuteAsync(Guid id);
    }
}
