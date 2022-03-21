using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllActiveTransactionsUseCase : IGetAllActiveTransactionsUseCase
    {
        private readonly ITransactionGateway _transactionGateway;

        public GetAllActiveTransactionsUseCase(ITransactionGateway transactionGateway)
        {
            _transactionGateway = transactionGateway;
        }

        public async Task<PagedResult<TransactionLimitedModel>> ExecuteAsync(GetActiveTransactionsRequest request)
        {
            return await _transactionGateway.GetAllActive(request).ConfigureAwait(false);
        }
    }
}
