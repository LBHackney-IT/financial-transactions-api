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
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionQuery query)
        {
            IEnumerable<Transaction> result = await _gateway.GetPagedTransactionsAsync(query).ConfigureAwait(false);

            var paginated = new Paginated<Transaction>()
            {
                Results = result,
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalResultCount = result.ToResponse().Count
            };

            return paginated.ToResponse();
        }
    }
}
