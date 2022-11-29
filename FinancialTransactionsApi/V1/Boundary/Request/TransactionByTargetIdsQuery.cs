using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionByTargetIdsQuery : BaseSearchQuery
    {
        /// <summary>
        /// The guids of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5,94b02545-0233-4640-98dd-b2900423c0a6,94b02545-0233-4640-98dd-b2900423c0a7
        /// </example>
        public List<Guid> TargetIds { get; set; }

        /// <summary>
        /// Selected start date
        /// </summary>
        /// <example>
        /// 2021-07-01
        /// </example>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Selected end date
        /// </summary>
        /// <example>
        /// 2021-08-01
        /// </example>
        [DateGreaterThan("StartDate")]
        public DateTime? EndDate { get; set; }
    }
}
