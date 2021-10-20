using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface ISort<T> where T : class
    {
        SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor);
    }
}
