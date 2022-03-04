using FinancialTransactionsApi.V1.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseAccountQuery : BaseSearchQuery
    {
        [FromQuery]
        public string PaginationToken { get; set; }
    }
}
