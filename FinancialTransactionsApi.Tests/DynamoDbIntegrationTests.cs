using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace TransactionsApi.Tests
{
    public class DynamoDbIntegrationTests<TStartup> where TStartup : class
    {
        public HttpClient Client { get; private set; }
        private DynamoDbMockWebApplicationFactory<TStartup> _factory;
        public IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        protected List<Action> CleanupActions { get; set; }

        private readonly List<TableDef> _tables = new List<TableDef>
        {
            // TODO: Populate the list of table(s) and their key property details here, for example:
            new TableDef { Name = "transactions", KeyName = "id", KeyType = ScalarAttributeType.N }
        };

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }

        public DynamoDbIntegrationTests()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("DynamoDb_LocalSecretKey", "8ksq4m6");
            EnsureEnvVarConfigured("DynamoDb_LocalAccessKey", "ig7pb");
            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();
        }

        //[OneTimeTearDown]
        //public void OneTimeTearDown()
        //{
        //    _factory.Dispose();
        //}

        //[SetUp]
        //public void BaseSetup()
        //{
        //    Client = _factory.CreateClient();
        //    CleanupActions = new List<Action>();
        //}

        //[TearDown]
        //public void BaseTearDown()
        //{
        //    foreach (var act in CleanupActions)
        //        act();
        //    Client.Dispose();
        //}
        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
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
    }

    [CollectionDefinition("DynamoDb collection", DisableParallelization = true)]
    public class DynamoDbCollection : ICollectionFixture<DynamoDbIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
