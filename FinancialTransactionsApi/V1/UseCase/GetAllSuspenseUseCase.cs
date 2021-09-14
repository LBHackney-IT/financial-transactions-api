using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllSuspenseUseCase : IGetAllSuspenseUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllSuspenseUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponses> ExecuteAsync(SuspenseTransactionsSearchRequest query)
        {
            var transactions =
                await _gateway.GetAllSuspenseAsync(query).ConfigureAwait(false);

            return new TransactionResponses
            {
                TransactionsList = transactions.Transactions.ToResponse(),
                Total = transactions.Total
            };
        }
    }
}
