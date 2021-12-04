using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class ModelStateExtension
    {
        public static string GetErrorMessages(this ModelStateDictionary modelState)
        {
            return
                string.Join(",", modelState.SelectMany(e => e.Value.Errors.Select(s => s.ErrorMessage)));
        }
    }
}
