using System.Collections.Generic;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using FinancialTransactionsApi.V1.Gateways.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways.ElasticSearch
{
    public class SearchGateway : ISearchGateway
    {
        private readonly IElasticSearchWrapper _elasticSearchWrapper;

        public SearchGateway(IElasticSearchWrapper elasticSearchWrapper)
        {
            _elasticSearchWrapper = elasticSearchWrapper;
        }
        public async Task<GetTransactionListResponse> GetListOfTransactions(TransactionSearchRequest query)
        {
            var searchResponse = await _elasticSearchWrapper.Search<QueryableTransaction>(query).ConfigureAwait(false);
            var transactionListResponse = new GetTransactionListResponse { Transactions = new List<TransactionResponse>() };
            transactionListResponse.Transactions.AddRange(searchResponse.Documents.Select(queryableTransaction =>
                queryableTransaction.Create())
            );
            transactionListResponse.SetTotal(searchResponse.Total);

            return transactionListResponse;
        }
    }
}
