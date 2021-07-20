using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace FinancialTransactionsApi.Versioning
{
    public static class ApiVersionDescriptionExtensions
    {
        public static string GetFormattedApiVersion(this ApiVersionDescription apiVersionDescription) =>
            $"v{apiVersionDescription.ApiVersion}";
    }
}

