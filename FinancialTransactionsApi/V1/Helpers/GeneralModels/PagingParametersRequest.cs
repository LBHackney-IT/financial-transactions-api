using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    public abstract class PagingParametersRequest
    {
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 20;

        [FromQuery(Name = "page")]
        public int PageNumber { get; set; } = 1;
    }
}
