using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public sealed class GetTransactionByTargetTypeAndTargetIdSpecification : Specification<TransactionEntity>
    {
        public GetTransactionByTargetTypeAndTargetIdSpecification(string targetType, Guid targetId, DateTime? startDate, DateTime? endDate) : base(x => x.TargetType.ToLower() == targetType.ToLower() && x.TargetId == targetId && ((!startDate.HasValue || x.TransactionDate >= DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)) && (!endDate.HasValue || x.TransactionDate <= DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)))) { }
    }
}
