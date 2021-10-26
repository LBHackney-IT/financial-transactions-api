using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Steps.Base
{
    public class BaseSteps
    {
        internal readonly HttpClient _httpClient;

        internal HttpResponseMessage _lastResponse;
        internal readonly JsonSerializerOptions _jsonOptions;

        public BaseSteps(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = CreateJsonOptions();
        }

        protected static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}
