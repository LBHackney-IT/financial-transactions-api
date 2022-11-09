using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Hackney.Core.DynamoDb;
using Hackney.Core.Sns;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace FinancialTransactionsApi.Tests
{
    public class AwsMockWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly List<TableDef> _tables;

        private IConfiguration _configuration;
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
                var serviceProvider = services.BuildServiceProvider();
                AWSConfigs.AWSRegion = "eu-west-2";

                var localstackUrl = Environment.GetEnvironmentVariable("Localstack_SnsServiceUrl");
                AmazonSqs = new AmazonSQSClient(new AmazonSQSConfig() { ServiceURL = localstackUrl });
            });
        }
    }
}
