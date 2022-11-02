using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    public class Paginated<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }
        public int ResultCount => Results.Count();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResultCount { get; set; }
        public int PageCount => (int) Math.Ceiling((double) TotalResultCount / PageSize);
    }
}
