using Amazon.DynamoDBv2.DataModel;
using Nest;
using System;
using System.Net.Http;
using System.Threading;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Fixture
{
    public class BaseFixture
    {
        protected readonly IElasticClient ElasticSearchClient;
        //public readonly IDynamoDBContext _dbContext;
        private HttpClient HttpClient { get; set; }

        private string _elasticSearchAddress;

        protected BaseFixture(IElasticClient elasticClient, HttpClient httpHttpClient)
        {
            ElasticSearchClient = elasticClient;
            HttpClient = httpHttpClient;
            // _dbContext = dbContext;
        }

        protected void WaitForEsInstance()
        {
            EnsureEnvVarConfigured("ELASTICSEARCH_DOMAIN_URL", "http://localhost:9200");

            Exception ex = null;
            var timeout = DateTime.UtcNow.AddSeconds(20); // 10 second timeout to make sure the ES instance has started and is ready to use.
            while (DateTime.UtcNow < timeout)
            {
                try
                {
                    var pingResponse = ElasticSearchClient.Ping();
                    if (pingResponse.IsValid)
                        return;
                    else
                        ex = pingResponse.OriginalException;
                }
                catch (Exception e)
                {
                    ex = e;
                }

                Thread.Sleep(200);
            }

            if (ex != null)
            {
                throw new Exception($"Could not connect to ES instance on {_elasticSearchAddress}", ex);
            }
        }

        private void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            _elasticSearchAddress = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrEmpty(_elasticSearchAddress))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
                _elasticSearchAddress = default;
            }
        }

    }
}
