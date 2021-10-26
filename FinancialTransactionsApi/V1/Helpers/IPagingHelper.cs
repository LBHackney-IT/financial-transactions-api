using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Helpers
{
    public interface IPagingHelper
    {
        int GetPageOffset(int pageSize, int currentPage);
    }
}
