using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllActiveTransactionsUseCase : IGetAllActiveTransactionsUseCase
    {
        private readonly ITransactionGateway _transactionGateway;

        public GetAllActiveTransactionsUseCase(ITransactionGateway transactionGateway)
        {
            _transactionGateway = transactionGateway;
        }

        public async Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(GetActiveTransactionsRequest request)
        {
            IEnumerable<Transaction> result = await _transactionGateway.GetAllActive(request).ConfigureAwait(false);

            var paginated = new Paginated<Transaction>()
            {
                Results = result,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalResultCount = result.ToResponse().Count
            };

            return paginated.ToResponse();
        }
    }
}
