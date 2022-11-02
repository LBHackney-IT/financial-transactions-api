namespace FinancialTransactionsApi.V1.Helpers.GeneralModels
{
    public abstract class PagingParametersDomain
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
