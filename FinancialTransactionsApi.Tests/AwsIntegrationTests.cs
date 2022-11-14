using FinancialTransactionsApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace FinancialTransactionsApi.Tests
{
    public class AwsIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        private const string TestToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNjM5NDIyNzE4LCJleHAiOjE5ODY1Nzc5MTgsImF1ZCI6InRlc3QiLCJzdWIiOiJ0ZXN0IiwiZ3JvdXBzIjpbInNvbWUtdmFsaWQtZ29vZ2xlLWdyb3VwIiwic29tZS1vdGhlci12YWxpZC1nb29nbGUtZ3JvdXAiXSwibmFtZSI6InRlc3RpbmcifQ.IcpQ00PGVgksXkR_HFqWOakgbQ_PwW9dTVQu4w77tmU";

        public HttpClient Client { get; private set; }
        public readonly AwsMockWebApplicationFactory<TStartup> Factory;
        public SnsEventVerifier<TransactionSns> SnsVerifer { get; private set; }
        protected List<Action> CleanupActions { get; set; }
        private readonly List<TableDef> _tables = new List<TableDef>
        {
            new TableDef { Name = "Transactions", KeyName = "target_id", RangeName="id" }
        };

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
            }
        }

        public AwsIntegrationTests()
        {
            EnsureEnvVarConfigured("ELASTICSEARCH_DOMAIN_URL", "http://localhost:9200");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:9911");

            EnsureEnvVarConfigured("Header", "Header");
            EnsureEnvVarConfigured("SubHeader", "SubHeader");
            EnsureEnvVarConfigured("Footer", "Footer");
            EnsureEnvVarConfigured("SubFooter", "SubFooter");
            EnsureEnvVarConfigured("REQUIRED_GOOGL_GROUPS", "e2e-testing-development;HackneyAll;saml-aws-mtfh-developer;saml-aws-developer");
            Factory = new AwsMockWebApplicationFactory<TStartup>(_tables);

            Client = Factory.CreateClient();
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(TestToken);
            CleanupActions = new List<Action>();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var act in CleanupActions)
                {
                    act();
                }
                if (null != SnsVerifer)
                    SnsVerifer.Dispose();
                if (null != Client)
                    Client.Dispose();
                if (null != Factory)
                    Factory.Dispose();
                _disposed = true;
            }
        }
    }

    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public string RangeName { get; set; }
    }

    [CollectionDefinition("Aws collection", DisableParallelization = true)]
    public class AwsCollection : ICollectionFixture<AwsIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
