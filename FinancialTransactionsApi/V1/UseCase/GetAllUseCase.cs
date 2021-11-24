using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponses> ExecuteAsync(TransactionQuery query)
        {
            var transactions =
                await _gateway.GetPagedTransactionsAsync(query)
                    .ConfigureAwait(false);

            return new TransactionResponses
            {
                TransactionsList = transactions.Transactions.ToResponse(),
                Total = transactions.Total
            };
        }
    }
}
