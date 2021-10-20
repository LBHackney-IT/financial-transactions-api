using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Gateways.Models;
using FinancialTransactionsApi.V1.Infrastructure;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using FinancialTransactionsApi.V1.Boundary.Response;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryBuilder<T> CreateWildStarSearchQuery(string searchText);

        IQueryBuilder<T> CreateFilterQuery(string commaSeparatedFilters);

        IQueryBuilder<T> SpecifyFieldsToBeSearched(List<string> fields);

        IQueryBuilder<T> SpecifyFieldsToBeFiltered(List<string> fields);

        QueryContainer FilterAndRespectSearchScore(QueryContainerDescriptor<T> descriptor);

        QueryContainer Search(QueryContainerDescriptor<T> containerDescriptor);
    }

    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private readonly IWildCardAppenderAndPrePender _wildCardAppenderAndPrePender;
        private readonly List<Func<QueryContainerDescriptor<T>, QueryContainer>> _queries;
        private string _searchQuery;
        private string _filterQuery;

        public QueryBuilder(IWildCardAppenderAndPrePender wildCardAppenderAndPrePender)
        {
            _wildCardAppenderAndPrePender = wildCardAppenderAndPrePender;
            _queries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        }

        public IQueryBuilder<T> CreateWildStarSearchQuery(string searchText)
        {
            var listOfWildCardedWords = _wildCardAppenderAndPrePender.Process(searchText);
            _searchQuery = $"({string.Join(" AND ", listOfWildCardedWords)}) " +
                              string.Join(' ', listOfWildCardedWords);

            return this;
        }

        public IQueryBuilder<T> CreateFilterQuery(string commaSeparatedFilters)
        {
            _filterQuery = string.Join(' ', commaSeparatedFilters.Split(","));

            return this;
        }

        public IQueryBuilder<T> SpecifyFieldsToBeSearched(List<string> fields)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(_searchQuery)
                        .Type(TextQueryType.MostFields)
                        .Fields(f =>
                        {
                            return fields.Aggregate(f, (current, field) => current.Field(field));
                        });

                    return queryDescriptor;
                });

            _queries.Add(query);

            return this;
        }

        public IQueryBuilder<T> SpecifyFieldsToBeFiltered(List<string> fields)
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> query =
                (containerDescriptor) => containerDescriptor.QueryString(q =>
                {
                    var queryDescriptor = q.Query(_filterQuery)
                        .Type(TextQueryType.MostFields)
                        .Fields(f =>
                        {
                            return fields.Aggregate(f, (current, field) => current.Field(field));
                        });

                    return queryDescriptor;
                });

            _queries.Add(query);

            return this;
        }

        public QueryContainer FilterAndRespectSearchScore(QueryContainerDescriptor<T> containerDescriptor)
        {
            return containerDescriptor.Bool(builder => builder.Must(_queries));
        }

        public QueryContainer Search(QueryContainerDescriptor<T> containerDescriptor)
        {
            return _queries.First().Invoke(containerDescriptor);
        }
    }

    public class TransactionQueryGenerator : IQueryGenerator<QueryableTransaction>
    {
        private readonly IQueryBuilder<QueryableTransaction> _queryBuilder;
        private readonly IWildCardAppenderAndPrePender _wildCardAppenderAndPrePender;

        public TransactionQueryGenerator(IQueryBuilder<QueryableTransaction> queryBuilder, IWildCardAppenderAndPrePender wildCardAppenderAndPrepender)
        {
            _queryBuilder = queryBuilder;
            _wildCardAppenderAndPrePender = wildCardAppenderAndPrepender;
        }

        public QueryContainer Create(TransactionSearchRequest request, QueryContainerDescriptor<QueryableTransaction> containerDescriptor)
        {
            //if (!(request is GetTransactionListRequest personListRequest))
            //{
            //    return null;
            //}
            var searchFields = new List<string> { "address", "paymentReference", "bankAccountNumber" };

            _queryBuilder.CreateWildStarSearchQuery(request.SearchText)
                .SpecifyFieldsToBeSearched(searchFields);

            var rettype = _queryBuilder.FilterAndRespectSearchScore(containerDescriptor);
            //var rettype1 = _queryBuilder.CreateWildStarSearchQuery(containerDescriptor);
            return _queryBuilder.FilterAndRespectSearchScore(containerDescriptor);
        }
    }
}
