using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using FinancialTransactionsApi.V1.Helpers;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.ElasticSearch
{
    public class ElasticElasticSearchWrapper : IElasticSearchWrapper
    {
        private readonly IElasticClient _esClient;
        private readonly IPagingHelper _pagingHelper;
        private readonly ILogger<ElasticElasticSearchWrapper> _logger;

        public ElasticElasticSearchWrapper(IElasticClient esClient,
            IPagingHelper pagingHelper, ILogger<ElasticElasticSearchWrapper> logger)
        {
            _esClient = esClient;
            _pagingHelper = pagingHelper;
            _logger = logger;
        }

        public async Task<ISearchResponse<T>> Search<T>(TransactionSearchRequest request) where T : class
        {
            try
            {
                var esNodes = string.Join(';', _esClient.ConnectionSettings.ConnectionPool.Nodes.Select(x => x.Uri));
                _logger.LogDebug($"ElasticSearch Search begins {esNodes}");

                if (request == null)
                    return new SearchResponse<T>();

                var pageOffset = _pagingHelper.GetPageOffset(request.PageSize, request.Page);

                var searchResponse = await _esClient.SearchAsync<T>(s => s
                    .Index("transactions")
                    .Query(q => q
                        .MultiMatch(m => m
                            .Type(TextQueryType.PhrasePrefix)
                            .Fields("*")
                            .Query(request.SearchText)
                        )
                    ).Size(request.PageSize)
                    .Skip(pageOffset)
                    .TrackTotalHits()
                ).ConfigureAwait(false);

                //var result = await _esClient.SearchAsync<T>(x => x.Index(_indexSelector.Create<T>())
                //    .Query(q => BaseQuery<T>(request).Create(request, q))
                //    .Sort(_iSortFactory.Create<T>(request).GetSortDescriptor)
                //    .Size(request.PageSize)
                //    .Skip(pageOffset)
                //    .TrackTotalHits()).ConfigureAwait(false);

                _logger.LogDebug("ElasticSearch Search ended");

                return searchResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ElasticSearch Search threw an exception");
                throw;
            }
        }

        //private IQueryGenerator<T> BaseQuery<T>(TransactionSearchRequest request) where T : class
        //{
        //    return _queryFactory.CreateQuery<T>(request);
        //}
    }
}
