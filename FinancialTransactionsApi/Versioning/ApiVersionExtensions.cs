using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionsApi.Versioning
{
    public static class ApiVersionExtensions
    {
        public static string GetFormattedApiVersion(this ApiVersion apiVersion)
        {
            return $"v{apiVersion}";
        }
    }
}
