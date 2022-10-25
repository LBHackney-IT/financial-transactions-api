using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Specs
{
    public abstract class Specification<T> : ISpecification<T> where T : class
    {
        protected Specification([NotNull] Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
    }
}
