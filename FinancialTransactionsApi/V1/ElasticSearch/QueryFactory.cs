using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using FinancialTransactionsApi.V1.Gateways.Models;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.ElasticSearch
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IWildCardAppenderAndPrePender _wildCardAppenderAndPrePender;
        private readonly IQueryBuilder<QueryableTransaction> _transactionQueryBuilder;


        public QueryFactory(
            IQueryBuilder<QueryableTransaction> transactionQueryBuilder, IWildCardAppenderAndPrePender wildCardAppenderAndPrePender)
        {
            _transactionQueryBuilder = transactionQueryBuilder;
            _wildCardAppenderAndPrePender = wildCardAppenderAndPrePender;
        }

        public IQueryGenerator<T> CreateQuery<T>(TransactionSearchRequest request) where T : class
        {
            if (typeof(T) == typeof(QueryableTransaction))
            {
                return (IQueryGenerator<T>) new TransactionQueryGenerator(_transactionQueryBuilder, _wildCardAppenderAndPrePender);
            }



            throw new System.NotImplementedException($"Query type {typeof(T)} is not implemented");
        }
    }
}
