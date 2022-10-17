using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
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
        private const string TestToken = " Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUxNjQ3NDY2MzU4MTYwNTkxODQiLCJlbWFpbCI6ImdhYnJpZWxhLm1hcnRpbmlAaGFja25leS5nb3YudWsiLCJpc3MiOiJIYWNrbmV5IiwibmFtZSI6IkdhYnJpZWxhIE1hcnRpbmkiLCJncm91cHMiOlsiZGV2ZWxvcG1lbnQtdGVhbS1wcm9kdWN0aW9uIiwiZGV2ZWxvcG1lbnQtdGVhbS1zdGFnaW5nIiwiSGFja25leUFsbCIsImhvdXNpbmctbmVlZHMtY29sbGFidG9vbHMtcHJvdG8iLCJpbmgtYWRtaW4tZGV2IiwiaW5oLXVzZXJzLWRldiIsIm1hbmFnZS1hcnJlYXJzLWxlYXNlaG9sZC1zdGFnaW5nIiwibWFuYWdlLWFycmVhcnMtbGVhc2Vob2xkIiwibWFuYWdlYXJyZWFycy1pbmNvbWUtY29sbGVjdGlvbi1yZWFkLXdyaXRlLWRldmVsb3BtZW50IiwibWFuYWdlYXJyZWFycy1sZWFzZWhvbGQtcmVhZC13cml0ZS1kZXZlbG9wbWVudCIsIm1taC1nZW5lcmFsLXVzZXItYWNjZXNzIiwibXRmaC11YXQtYWNjZXNzIiwic2FtbC1hd3MtYXBwc3RyZWFtLWNlZGFyIiwic2FtbC1hd3MtZGFzaGJvYXJkLWFjY2VzcyIsInNhbWwtYXdzLWRldmVsb3BlciIsInNhbWwtYXdzLW10ZmgtZGV2ZWxvcGVyIiwic2FtbC1ob3VzaW5nZmluYW5jZSIsInNpbmdsZS12aWV3LWFjY2VzcyJdLCJpYXQiOjE2NjU1NzU2ODB9.pqGJjNXrgR8e7YWjTBjiHJdWgFFoVxm-ror3UnngBXE";

        public HttpClient Client { get; private set; }
        public readonly AwsMockWebApplicationFactory<TStartup> Factory;
        public IDynamoDBContext DynamoDbContext => Factory?.DynamoDbContext;
        public SnsEventVerifier<TransactionSns> SnsVerifer { get; private set; }
        protected List<Action> CleanupActions { get; set; }
        private readonly List<TableDef> _tables = new List<TableDef>
        {
            new TableDef { Name = "Transactions", KeyName = "target_id", RangeName="id", KeyType = KeyType.HASH,RangeType = KeyType.RANGE,  KeyScalarType= ScalarAttributeType.S}
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
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("DynamoDb_LocalSecretKey", "o4fejrd");
            EnsureEnvVarConfigured("DynamoDb_LocalAccessKey", "ez1lwb");
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
        public KeyType KeyType { get; set; }
        public KeyType RangeType { get; set; }
        public ScalarAttributeType KeyScalarType { get; set; }
    }

    [CollectionDefinition("Aws collection", DisableParallelization = true)]
    public class AwsCollection : ICollectionFixture<AwsIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
