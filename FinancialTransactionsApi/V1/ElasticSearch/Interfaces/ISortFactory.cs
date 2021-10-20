using FinancialTransactionsApi.V1.Boundary.Request;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface ISortFactory
    {
        ISort<T> Create<T>(TransactionSearchRequest request) where T : class;
    }
}
