using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure
{
    public static class DynamoDbInitilisationExtensions
    {
        public static void ConfigureDynamoDB(this IServiceCollection services)
        {
            _ = bool.TryParse(Environment.GetEnvironmentVariable("DynamoDb_LocalMode"), out var localMode);

            if (localMode)
            {
                var url = Environment.GetEnvironmentVariable("DynamoDb_LocalServiceUrl");
                var accessKey = Environment.GetEnvironmentVariable("DynamoDb_LocalAccessKey");
                var secretKey = Environment.GetEnvironmentVariable("DynamoDb_LocalSecretKey");
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = url };
                    var credentials = new BasicAWSCredentials(accessKey, secretKey);
                    return new AmazonDynamoDBClient(credentials, clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddScoped<IDynamoDBContext>(sp =>
            {
                var db = sp.GetService<IAmazonDynamoDB>();
                return new DynamoDBContext(db);
            });
        }
    }
}
