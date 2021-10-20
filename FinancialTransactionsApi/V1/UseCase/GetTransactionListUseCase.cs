using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Gateways.ElasticSearch;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetTransactionListUseCase : IGetTransactionListUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public GetTransactionListUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }
        //[LogCall]
        public async Task<GetTransactionListResponse> ExecuteAsync(TransactionSearchRequest transactionSearchRequest)
        {
            return await _searchGateway.GetListOfTransactions(transactionSearchRequest).ConfigureAwait(false);
        }
    }
}
