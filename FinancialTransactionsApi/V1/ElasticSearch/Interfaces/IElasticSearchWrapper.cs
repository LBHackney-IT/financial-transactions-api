using FinancialTransactionsApi.V1.Boundary.Request;
using Nest;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface IElasticSearchWrapper
    {
        Task<ISearchResponse<T>> Search<T>(TransactionSearchRequest request) where T : class;
    }
}
