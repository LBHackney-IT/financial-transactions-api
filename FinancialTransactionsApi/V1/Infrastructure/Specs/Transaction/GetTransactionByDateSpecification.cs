using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public sealed class GetTransactionByDateSpecification : Specification<TransactionEntity>
    {
        public GetTransactionByDateSpecification(DateTime? startDate, DateTime? endDate) : base(x => x.TransactionDate >= DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc) && x.TransactionDate <= DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)) { }
    }
}
