using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Gateways.ElasticSearch
{
    public interface ISearchGateway
    {
        Task<GetTransactionListResponse> GetListOfTransactions(TransactionSearchRequest query);
    }
}
