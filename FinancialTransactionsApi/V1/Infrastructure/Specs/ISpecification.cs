using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
