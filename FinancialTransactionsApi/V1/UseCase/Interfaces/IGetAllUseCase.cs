using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using Hackney.Core.DynamoDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        public Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionQuery query);
    }
}
