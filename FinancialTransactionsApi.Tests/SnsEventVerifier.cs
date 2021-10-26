using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace FinancialTransactionsApi.Tests
{
    // TODO - move somewhere common?

    /// <summary>
    /// Helper class used to verify that the correct Sns event gets raised during an E2E test
    /// </summary>
    /// <typeparam name="T">The event class used to create the event.</typeparam>
    public class SnsEventVerifier<T> : IDisposable where T : class
    {
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly IAmazonSQS _amazonSQS;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly string _topicArn;
        private readonly string _queueUrl;
        private readonly string _subscriptionArn;

        private readonly string _sqsQueueName = "test-messages";

        public Exception LastException { get; private set; }

        /// <summary>
        /// Constructor
        /// * Constructs a temporary queue to receive a copy of all events raised during a test.
        /// * Configures a subscription to ensure events raised to the topic get set to the queue.
        /// </summary>
        /// <param name="amazonSQS">The SQS client</param>
        /// <param name="snsClient">The SNS client</param>
        /// <param name="topicArn">The arn of the topic</param>
        public SnsEventVerifier(IAmazonSQS amazonSQS, IAmazonSimpleNotificationService snsClient, string topicArn)
        {
            _amazonSQS = amazonSQS;
            _snsClient = snsClient;
            _topicArn = topicArn;
            _jsonOptions = CreateJsonOptions();

            var queueResponse = _amazonSQS.CreateQueueAsync(_sqsQueueName).GetAwaiter().GetResult();
            _queueUrl = queueResponse.QueueUrl;

            _subscriptionArn = _snsClient.SubscribeQueueAsync(_topicArn, _amazonSQS, _queueUrl)
                                        .GetAwaiter().GetResult();
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
                _snsClient.UnsubscribeAsync(_subscriptionArn).GetAwaiter().GetResult();
                _snsClient.DeleteTopicAsync(_topicArn).GetAwaiter().GetResult();
                _amazonSQS.DeleteQueueAsync(_queueUrl).GetAwaiter().GetResult();

                _disposed = true;
            }
        }

        /// <summary>
        /// Verifies that the an expected event has been raised.
        /// </summary>
        /// <param name="verifyFunction">A function that will receive a copy of each event raised.
        /// This function should attempt to verify that the contents of the message match what is expected.
        /// Throw an exception should then contents not match.
        /// </param>
        /// <returns>true if a message in the temporary queue satisfies the verification function.
        /// false if no message in the temporary queue satisfies the verification function</returns>
        public bool VerifySnsEventRaised(Action<T> verifyFunction)
        {
            bool eventFound = false;
            var request = new ReceiveMessageRequest(_queueUrl)
            {
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 2
            };
            var response = _amazonSQS.ReceiveMessageAsync(request).GetAwaiter().GetResult();
            foreach (var msg in response.Messages)
            {
                eventFound = IsExpectedMessage(msg, verifyFunction);
                if (eventFound) break;
            }

            return eventFound;
        }


        private bool IsExpectedMessage(Message msg, Action<T> verifyFunction)
        {
            // Here we are assuming the message is not in raw format
            // (which is the case when using SubscribeQueueAsync() in the constructor above)
            var payloadString = JsonDocument.Parse(msg.Body).RootElement.GetProperty("Message").GetString();
            var eventObject = JsonSerializer.Deserialize<T>(payloadString, _jsonOptions);
            try
            {
                verifyFunction(eventObject);
                return true;
            }
            catch (Exception e)
            {
                LastException = e;
                return false;
            }
        }
    }
}
