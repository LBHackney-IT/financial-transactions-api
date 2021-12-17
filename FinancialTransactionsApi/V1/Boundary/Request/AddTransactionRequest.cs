using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class AddTransactionRequest : BaseTransactionModel
    {
        /// <summary>
        /// Type of transaction [Charge, Rent]
        /// </summary>
        /// <example>
        /// Rent
        /// </example>
        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }
    }
}
