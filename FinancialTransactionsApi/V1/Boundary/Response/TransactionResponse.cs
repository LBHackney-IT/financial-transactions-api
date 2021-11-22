using System;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionResponse:BaseTransactionModel,ISuspenseResolution
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     2dcb38af-6558-4df4-9d90-334d0e9bf827
        /// </example>
        public Guid Id { get; set; }

        /// <summary>
        /// Information after this record ceases to be suspense
        /// </summary>
        /// <example>
        /// {
        ///     "ResolutionDate": "2021-04-28T23:00:00.000Z",
        ///     "IsResolve" : true,
        ///     "Note": "Some notes about this record"
        /// }
        /// </example>
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
    }
}
