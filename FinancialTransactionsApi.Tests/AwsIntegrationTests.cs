using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using FinancialTransactionsApi.V1.Domain;
using Nest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace FinancialTransactionsApi.Tests
{
    public class AwsIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient Client { get; private set; }
        public readonly AwsMockWebApplicationFactory<TStartup> Factory;
        public IDynamoDBContext DynamoDbContext => Factory?.DynamoDbContext;
        public IElasticClient ElasticSearchClient => Factory.ElasticSearchClient;
        public IAmazonSimpleNotificationService SimpleNotificationService => Factory?.SimpleNotificationService;
        public SnsEventVerifier<TransactionSns> SnsVerifer { get; private set; }
        protected List<Action> CleanupActions { get; set; }
        private readonly List<TableDef> _tables = new List<TableDef>
        {
            new TableDef()
            {
                TableName = "Transactions",
                PartitionKey = new AttributeDef()
                {
                    KeyName = "id",
                    KeyType = KeyType.HASH,
                    KeyScalarType = ScalarAttributeType.S
                }
            }
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
            EnsureEnvVarConfigured("DynamoDb_LocalSecretKey", "8kmm3g");
            EnsureEnvVarConfigured("DynamoDb_LocalAccessKey", "fco1i2");
            EnsureEnvVarConfigured("ELASTICSEARCH_DOMAIN_URL", "http://localhost:9200");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:9911");
            Factory = new AwsMockWebApplicationFactory<TStartup>(_tables);

            Client = Factory.CreateClient();
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
        public string TableName { get; set; }
        public AttributeDef PartitionKey { get; set; }
    }

    public class AttributeDef
    {
        public string KeyName { get; set; }
        public ScalarAttributeType KeyScalarType { get; set; }
        public KeyType KeyType { get; set; }
    }

    [CollectionDefinition("Aws collection", DisableParallelization = true)]
    public class AwsCollection : ICollectionFixture<AwsIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
