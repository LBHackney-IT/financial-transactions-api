using System.Diagnostics.CodeAnalysis;

namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    [ExcludeFromCodeCoverage]
    public abstract class PagingParametersDomain
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
