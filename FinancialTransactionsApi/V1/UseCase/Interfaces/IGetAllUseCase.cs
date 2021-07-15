using System;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;

namespace TransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        public Task<TransactionResponseObjectList> ExecuteAsync(Guid targetId,string transactionType, DateTime? startDate, DateTime? endDate);
    }
}
