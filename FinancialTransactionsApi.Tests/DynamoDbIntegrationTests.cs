using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Amazon.DynamoDBv2.Model;
using Nest;
using Xunit;

namespace FinancialTransactionsApi.Tests
{
    public class DynamoDbIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient Client { get; private set; }
        private readonly DynamoDbMockWebApplicationFactory<TStartup> _factory;
        public IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        public IElasticClient ElasticSearchClient => _factory.ElasticSearchClient;
        protected List<Action> CleanupActions { get; set; }
        private readonly List<TableDef> _tables = new List<TableDef>
        {
            new TableDef
            {
                Name = "Transactions",
                KeyName = "id",
                KeyType = ScalarAttributeType.S,
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>(new[]
                {
                    new GlobalSecondaryIndex
                    {
                        IndexName = "is_suspense_dx",
                        KeySchema = new List<KeySchemaElement>(new[]
                        {
                            new KeySchemaElement("is_suspense", KeyType.HASH)
                            //new KeySchemaElement("parentAssetIds", KeyType.RANGE)
                        }),
                        Projection = new Projection { ProjectionType = ProjectionType.ALL },
                        ProvisionedThroughput = new ProvisionedThroughput(10 , 10)
                    }
                })
            }
        };
        //private readonly List<TableDef> _tables = new List<TableDef>
        //{
        //    new TableDef()
        //    {
        //        TableName = "Transactions",
        //        PartitionKey = new AttributeDef()
        //        {
        //            KeyName = "id",
        //            KeyType = KeyType.HASH,
        //            KeyScalarType = ScalarAttributeType.S
        //        },
        //        Indices = new List<GlobalIndexDef>()
        //        {
        //            new GlobalIndexDef()
        //            {
        //                IndexName = "is_suspense_dx",
        //                KeyName = "is_suspense",
        //                KeyScalarType = ScalarAttributeType.S,
        //                KeyType = KeyType.HASH,
        //                ProjectionType = "ALL"
        //            }
        //        }
        //    }
        //};

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
            {
                Environment.SetEnvironmentVariable(name, defaultValue);
            }
        }

        public DynamoDbIntegrationTests()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            //EnsureEnvVarConfigured("DynamoDb_LocalSecretKey", "8kmm3g");
            //EnsureEnvVarConfigured("DynamoDb_LocalAccessKey", "fco1i2");
            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);

            Client = _factory.CreateClient();
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
                // Client.Dispose();

                if (null != _factory)
                    _factory.Dispose();
                _disposed = true;
            }
        }
    }
    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public ScalarAttributeType KeyType { get; set; }
        public string RangeKeyName { get; set; }
        public ScalarAttributeType RangeKeyType { get; set; }
        public List<LocalSecondaryIndex> LocalSecondaryIndexes { get; set; } = new List<LocalSecondaryIndex>();
        public List<GlobalSecondaryIndex> GlobalSecondaryIndexes { get; set; } = new List<GlobalSecondaryIndex>();
    }
    //public class TableDef
    //{
    //    public string TableName { get; set; }
    //    public AttributeDef PartitionKey { get; set; }
    //    public List<GlobalIndexDef> Indices { get; set; }
    //}

    //public class AttributeDef
    //{
    //    public string KeyName { get; set; }
    //    public ScalarAttributeType KeyScalarType { get; set; }
    //    public KeyType KeyType { get; set; }
    //}

    //public class GlobalIndexDef : AttributeDef
    //{
    //    public string IndexName { get; set; }
    //    public string ProjectionType { get; set; }
    //}
    [CollectionDefinition("DynamoDb collection", DisableParallelization = true)]
    public class DynamoDbCollection : ICollectionFixture<DynamoDbIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
