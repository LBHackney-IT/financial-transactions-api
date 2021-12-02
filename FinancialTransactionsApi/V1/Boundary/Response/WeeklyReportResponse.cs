using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class WeeklyReportResponse
    {
        public Person Person { get; set; }
        public List<Transaction> Records { get; set; }
    }
}
