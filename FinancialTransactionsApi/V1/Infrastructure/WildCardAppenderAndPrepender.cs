using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public class WildCardAppenderAndPrePender : IWildCardAppenderAndPrePender
    {
        public List<string> Process(string phrase)
        {
            return string.IsNullOrEmpty(phrase) ? new List<string>() : phrase.Split(' ')
                .Select(word => $"*{word}*")
                .ToList();
        }
    }
}
