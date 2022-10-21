using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public sealed class GetTransactionByTargetTypeAndTargetId : Specification<TransactionEntity>
    {
        public GetTransactionByTargetTypeAndTargetId(string targetType, Guid targetId) : base(x => x.TargetType.ToLower() == targetType.ToLower() && x.TargetId == targetId) { }
    }
}
