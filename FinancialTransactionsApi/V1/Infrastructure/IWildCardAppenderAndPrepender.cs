using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public interface IWildCardAppenderAndPrePender
    {
        List<string> Process(string phrase);
    }
}
