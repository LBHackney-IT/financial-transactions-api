using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public APIMetaData MetaData { get; set; }

        public PaginatedResponse() { }

        public PaginatedResponse(IEnumerable<T> result, PaginationMetaData paginationMetaData)
        {
            Results = result;
            MetaData = new APIMetaData()
            {
                Pagination = paginationMetaData
            };
        }
    }
}
