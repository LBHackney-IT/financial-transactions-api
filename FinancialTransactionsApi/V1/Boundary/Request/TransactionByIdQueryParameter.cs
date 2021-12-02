using FinancialTransactionsApi.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FinancialTransactionsApi.V1.Boundary.Request
{
    public class TransactionByIdQueryParameter
    {
        /// <summary>
        /// The guid of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a2
        /// </example>
        [NonEmptyGuid]
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
        /// <summary>
        /// The target of a tenancy/property
        /// </summary>
        /// <example>
        /// 94b02545-0233-4640-98dd-b2900423c0a5
        /// </example>
        [NonEmptyGuid]
        [FromQuery(Name = "targetId")]
        public Guid TargetId { get; set; }


    }
}
