using System;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.Boundary.Response
{
    public class TransactionResponse : BaseTransactionModel, ISuspenseResolution
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

        /// <summary>
        /// Total paid amount
        /// </summary>
        /// <example>
        /// 56.78
        /// </example>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Total charged amount
        /// </summary>
        /// <example>
        /// 87.53
        /// </example>
        public decimal ChargedAmount { get; set; }

        /// <summary>
        /// Total balance amount
        /// </summary>
        /// <example>
        /// 1025.00
        /// </example>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Housing Benefit Contribution
        /// </summary>
        /// <example>
        /// 25.56
        /// </example>
        public decimal HousingBenefitAmount { get; set; }

        /// <summary>
        /// Address of property
        /// </summary>
        /// <example>
        /// Apartment 22, 18 G road, SW11
        /// </example>
        public string Address { get; set; }

        /// <summary>
        /// Person, who paid for the transaction
        /// </summary>
        /// <example>
        /// {
        ///     "Id": "6d290de9-75aa-46a9-8bf5-cb8e9bdf4ff0",
        ///     "FullName": "Kian Hayward"
        /// }
        /// </example>
        public Person Person { get; set; }

        /// <summary>
        /// ToDO: No information about this field
        /// </summary>
        /// <example>
        /// HSGSUN
        /// </example>
        public string Fund { get; set; }

        /// <example>
        ///     Admin
        /// </example>
        public string LastUpdatedBy { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime LastUpdatedAt { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime CreatedAt { get; set; }

        /// <example>
        ///     Admin
        /// </example>
        public string CreatedBy { get; set; }
    }
}
