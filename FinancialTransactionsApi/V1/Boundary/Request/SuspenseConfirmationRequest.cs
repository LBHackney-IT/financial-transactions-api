using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class SuspenseConfirmationRequest
    {
        public Guid TargetId { get; set; }
        public string Note { get; set; }

    }
}
