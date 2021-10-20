using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Boundary.Request;
using Nest;

namespace FinancialTransactionsApi.V1.ElasticSearch.Interfaces
{
    public interface IQueryGenerator<T> where T : class

    {
        QueryContainer Create(TransactionSearchRequest request,
            QueryContainerDescriptor<T> containerDescriptor);
    }
}
