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
        private const string TestToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNjM5NDIyNzE4LCJleHAiOjE5ODY1Nzc5MTgsImF1ZCI6InRlc3QiLCJzdWIiOiJ0ZXN0IiwiZ3JvdXBzIjpbInNvbWUtdmFsaWQtZ29vZ2xlLWdyb3VwIiwic29tZS1vdGhlci12YWxpZC1nb29nbGUtZ3JvdXAiXSwibmFtZSI6InRlc3RpbmcifQ.IcpQ00PGVgksXkR_HFqWOakgbQ_PwW9dTVQu4w77tmU";

        public HttpClient Client { get; private set; }
        public readonly AwsMockWebApplicationFactory<TStartup> Factory;
        public IDynamoDBContext DynamoDbContext => Factory?.DynamoDbContext;
        public IAmazonSimpleNotificationService SimpleNotificationService => Factory?.SimpleNotificationService;
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
            EnsureEnvVarConfigured("REQUIRED_GOOGL_GROUPS", "some-valid-google-group");
            Factory = new AwsMockWebApplicationFactory<TStartup>(_tables);

            Client = Factory.CreateClient();
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(TestToken);
            CleanupActions = new List<Action>();
            CreateSnsTopic();
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
        private void CreateSnsTopic()
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = SimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "transactioncreated",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("TRANSACTION_SNS_ARN", response.TopicArn);

            SnsVerifer = new SnsEventVerifier<TransactionSns>(Factory.AmazonSqs, SimpleNotificationService, response.TopicArn);
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
