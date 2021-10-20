using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using FinancialTransactionsApi.V1.Gateways.Models;

namespace FinancialTransactionsApi.V1.ElasticSearch
{
    public class IndexSelector : IIndexSelector
    {
        public Indices.ManyIndices Create<T>()
        {
            var type = typeof(T);

            if (type == typeof(QueryableTransaction))
                return Indices.Index(new List<IndexName> { "transactions" });
            throw new NotImplementedException($"No index for type {typeof(T)}");
        }
    }
}
