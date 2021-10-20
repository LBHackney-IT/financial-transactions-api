using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.ElasticSearch.Interfaces;
using Nest;

namespace FinancialTransactionsApi.V1.ElasticSearch
{
    public class DefaultSort<T> : ISort<T> where T : class
    {
        public SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor)
        {
            return descriptor;
        }
    }
}
