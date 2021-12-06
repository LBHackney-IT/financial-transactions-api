using System;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Boundary
{
    public interface ISuspenseResolution
    {
        public Guid Id { get; set; }
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
    }
}
