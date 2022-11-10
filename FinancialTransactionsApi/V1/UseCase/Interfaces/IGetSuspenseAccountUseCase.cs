using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Helpers;
using Hackney.Core.DynamoDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetSuspenseAccountUseCase
    {
        public Task<ResponseWrapper<IEnumerable<TransactionResponse>>> ExecuteAsync(SuspenseAccountQuery query);
    }
}
