using Microsoft.EntityFrameworkCore;
using System;

namespace FinancialTransactionsApi.V1.Domain
{
    [Keyless]
    public class SuspenseResolutionInfo
    {
        public DateTime? ResolutionDate { get; set; }
        public bool IsResolve => IsConfirmed && IsApproved;
        public bool IsConfirmed { get; set; }
        public bool IsApproved { get; set; }
        public string Note { get; set; }
    }
}
