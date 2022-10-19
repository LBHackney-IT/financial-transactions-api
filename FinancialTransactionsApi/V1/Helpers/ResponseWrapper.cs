using System.Collections.Generic;
using System.Linq;

namespace FinancialTransactionsApi.V1.Helpers
{
    public class ResponseWrapper<T> where T : class
    {
        public T Value { get; set; }

        public bool IsEmpty
        {
            get
            {
                if (this.Value?.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    return !(this.Value as IEnumerable<object>).Any();
                }

                return this.Value is null;
            }
        }

        public ResponseWrapper(T nullableValue) => Value = nullableValue;
    }
}
