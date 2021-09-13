using System;

namespace FinancialTransactionsApi.V1.Domain
{
    public class SuspenseResolutionInfo
    {
        public DateTime? ResolutionDate { get; set; }

        public bool IsResolve { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsApproved { get; set; }

        public string Note { get; set; }
    }
}
