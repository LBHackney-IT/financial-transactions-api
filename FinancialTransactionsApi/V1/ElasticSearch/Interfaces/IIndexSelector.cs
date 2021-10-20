using Nest;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface IIndexSelector
    {
        Indices.ManyIndices Create<T>();
    }
}
