using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using FinancialTransactionsApi.V1.Infrastructure;
using Hackney.Core.DynamoDb;
using Hackney.Core.ElasticSearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Hackney.Core.Sns;

namespace FinancialTransactionsApi.Tests
{
    public class AwsMockWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly List<TableDef> _tables;

        private IConfiguration _configuration;
        public IElasticClient ElasticSearchClient { get; private set; }
        public IAmazonDynamoDB DynamoDb { get; set; }
        public IDynamoDBContext DynamoDbContext { get; private set; }
        public IAmazonSimpleNotificationService SimpleNotificationService { get; private set; }
        public IAmazonSQS AmazonSqs { get; private set; }

        public AwsMockWebApplicationFactory(List<TableDef> tables)
        {
            _tables = tables;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(b =>
                {
                    b.AddEnvironmentVariables();
                    _configuration = b.Build();
                })
                .UseStartup<Startup>();
            builder.ConfigureServices(services =>
            {
                services.ConfigureDynamoDB();
                services.ConfigureSns();
                services.ConfigureElasticSearch(_configuration, "ELASTICSEARCH_DOMAIN_URL");
                var serviceProvider = services.BuildServiceProvider();
                DynamoDb = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
                DynamoDbContext = serviceProvider.GetRequiredService<IDynamoDBContext>();
                ElasticSearchClient = serviceProvider.GetRequiredService<IElasticClient>();
                SimpleNotificationService = serviceProvider.GetRequiredService<IAmazonSimpleNotificationService>();

                var localstackUrl = Environment.GetEnvironmentVariable("Localstack_SnsServiceUrl");
                AmazonSqs = new AmazonSQSClient(new AmazonSQSConfig() { ServiceURL = localstackUrl });

                EnsureTablesExist(DynamoDb, _tables);
            });
        }

        private static void EnsureTablesExist(IAmazonDynamoDB dynamoDb, List<TableDef> tables)
        {
            foreach (var table in tables)
            {
                try
                {

                    // This command helps to prevent the next exception:
                    // Amazon.XRay.Recorder.Core.Exceptions.EntityNotAvailableException : Entity doesn't exist in AsyncLocal
                    AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;
                    var request = new CreateTableRequest(table.Name,
                        new List<KeySchemaElement> { new KeySchemaElement(table.KeyName, table.KeyType), new KeySchemaElement(table.RangeName, table.RangeType) },
                        new List<AttributeDefinition> { new AttributeDefinition(table.KeyName, table.KeyScalarType), new AttributeDefinition(table.RangeName, table.KeyScalarType) },
                        new ProvisionedThroughput(3, 3));
                    _ = dynamoDb.CreateTableAsync(request).GetAwaiter().GetResult();
                }
                catch (ResourceInUseException)
                {
                    // It already exists :-)
                }
            }
            //foreach (var table in tables)
            //{
            //    try
            //    {
            //        // Hanna Holosova
            //        // This command helps to prevent the next exception:
            //        // Amazon.XRay.Recorder.Core.Exceptions.EntityNotAvailableException : Entity doesn't exist in AsyncLocal
            //        AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

            //        List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>();
            //        List<GlobalSecondaryIndex> globalSecondaryIndexes = new List<GlobalSecondaryIndex>();

            //        attributeDefinitions.Add(new AttributeDefinition(table.PartitionKey.KeyName,
            //            table.PartitionKey.KeyScalarType));

            //        if (table.Indices != null)
            //        {
            //            foreach (var index in table.Indices)
            //            {
            //                globalSecondaryIndexes.Add(
            //                    new GlobalSecondaryIndex()
            //                    {
            //                        IndexName = index.IndexName,
            //                        ProvisionedThroughput =
            //                            new ProvisionedThroughput { ReadCapacityUnits = 1L, WriteCapacityUnits = 1L },
            //                        KeySchema =
            //                        {
            //                        new KeySchemaElement
            //                        {
            //                            AttributeName = index.KeyName, KeyType = index.KeyType
            //                        }
            //                        },
            //                        Projection = new Projection { ProjectionType = index.ProjectionType }
            //                    });
            //                attributeDefinitions.Add(new AttributeDefinition(index.KeyName, index.KeyScalarType));
            //            }
            //        }

            //        CreateTableRequest request = new CreateTableRequest
            //        {
            //            TableName = table.TableName,
            //            ProvisionedThroughput =
            //                new ProvisionedThroughput { ReadCapacityUnits = (long) 3, WriteCapacityUnits = (long) 3 },
            //            AttributeDefinitions = attributeDefinitions,
            //            KeySchema = new List<KeySchemaElement>
            //            {
            //                new KeySchemaElement(table.PartitionKey.KeyName, table.PartitionKey.KeyType)
            //            },
            //            GlobalSecondaryIndexes = globalSecondaryIndexes
            //        };

            //        _ = dynamoDb.CreateTableAsync(request).GetAwaiter().GetResult();
            //    }
            //    catch (ResourceInUseException)
            //    {
            //        // It already exists :-)
            //    }
            //    catch (Exception exception)
            //    {
            //        throw new Exception("Exception in checking table existence", exception);
            //    }
            //}
        }
    }
}
