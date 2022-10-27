namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class PaginationMetaData
    {
        public int ResultCount { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public PaginationMetaData(int resultCount, int totalCount, int pageCount, int pageSize, int currentpage)
        {
            ResultCount = resultCount;
            TotalCount = totalCount;
            PageCount = pageCount;
            PageSize = pageSize;
            CurrentPage = currentpage;
        }
    }
}
