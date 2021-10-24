using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Nest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FinancialTransactionsApi.Tests
{
    public class AwsIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient Client { get; private set; }
        public readonly AwsMockWebApplicationFactory<TStartup> Factory;
        public IDynamoDBContext DynamoDbContext => Factory?.DynamoDbContext;
        public IElasticClient ElasticSearchClient => Factory.ElasticSearchClient;
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
                },
                Indices = new List<GlobalIndexDef>()
                {
                    new GlobalIndexDef()
                    {
                        IndexName = "is_suspense_dx",
                        KeyName = "is_suspense",
                        KeyScalarType = ScalarAttributeType.S,
                        KeyType = KeyType.HASH,
                        ProjectionType = "ALL"
                    }
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
            Factory = new AwsMockWebApplicationFactory<TStartup>(_tables);

            Client = Factory.CreateClient();
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
                if (null != Client)
                    //Client.Dispose();
                    if (null != Factory)
                        Factory.Dispose();
                _disposed = true;
            }
        }

    }

    public class TableDef
    {
        public string TableName { get; set; }
        public AttributeDef PartitionKey { get; set; }
        public List<GlobalIndexDef> Indices { get; set; }
    }

    public class AttributeDef
    {
        public string KeyName { get; set; }
        public ScalarAttributeType KeyScalarType { get; set; }
        public KeyType KeyType { get; set; }
    }

    public class GlobalIndexDef : AttributeDef
    {
        public string IndexName { get; set; }
        public string ProjectionType { get; set; }
    }
    [CollectionDefinition("Aws collection", DisableParallelization = true)]
    public class AwsCollection : ICollectionFixture<AwsIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
