using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using FinancialTransactionsApi.V1.Gateways.Models;

namespace FinancialTransactionsApi.V1.ElasticSearch
{
    public class SortFactory : ISortFactory
    {
        public ISort<T> Create<T>(TransactionSearchRequest request) where T : class
        {
            //if (typeof(T) == typeof(QueryableTransaction))
            //{
            //    if (string.IsNullOrEmpty(request.SortBy))
            //        return new DefaultSort<T>();

            //    //switch (request.IsDesc)
            //    //{
            //    //    case true:
            //    //        return (ISort<T>) new SurnameDesc();
            //    //    case false:
            //    //        return (ISort<T>) new SurnameAsc();
            //    //}
            //}

            return new DefaultSort<T>();
        }
    }
}
