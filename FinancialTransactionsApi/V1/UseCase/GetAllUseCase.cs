using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers.GeneralModels;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Linq;
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

        public async Task<PaginatedResponse<TransactionResponse>> ExecuteAsync(TransactionQuery query)
        {
            var transactions = await _gateway.GetPagedTransactionsAsync(query).ConfigureAwait(false);

            var page = query.Page > 1 ? query.Page : 0;

            var filteredTransactions = transactions.Skip(page * query.PageSize).Take(query.PageSize);

            var paginateTransactions = new Paginated<Transaction>()
            {
                Results = filteredTransactions,
                CurrentPage = page,
                PageSize = query.PageSize,
                TotalResultCount = transactions.Count(),
            };

            return paginateTransactions.ToResponse();
        }
    }
}
