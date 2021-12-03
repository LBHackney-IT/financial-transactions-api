using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways.Sns.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FinancialTransactionsApi.V1.Gateways.Sns
{
    public class TransactionSnsGateway : ISnsGateway
    {
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;

        public TransactionSnsGateway(IAmazonSimpleNotificationService amazonSimpleNotificationService,
            IConfiguration configuration)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _configuration = configuration;
            _jsonOptions = CreateJsonOptions();
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        public async Task Publish(TransactionsSns transactionsSns)
        {
            string message = JsonSerializer.Serialize(transactionsSns, _jsonOptions);
            var request = new PublishRequest
            {
                Message = message,
                TopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN"),
                MessageGroupId = "HeadOfChargesSnsGroupId"
            };
            await _amazonSimpleNotificationService.PublishAsync(request).ConfigureAwait(false);
        }
    }
}
