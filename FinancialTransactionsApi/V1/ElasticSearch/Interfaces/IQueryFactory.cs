using FinancialTransactionsApi.V1.Boundary.Request;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface IQueryFactory
    {
        IQueryGenerator<T> CreateQuery<T>(TransactionSearchRequest request) where T : class;
    }
}
