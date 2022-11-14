using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public sealed class GetTransactionBySuspenseAccountSpecification : Specification<TransactionEntity>
    {
        public GetTransactionBySuspenseAccountSpecification(bool SearchText) : base(x => x.IsSuspense == SearchText) { }
    }
}

