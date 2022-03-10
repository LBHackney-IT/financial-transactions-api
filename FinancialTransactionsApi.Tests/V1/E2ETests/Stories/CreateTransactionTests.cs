using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps;
using FluentAssertions;
using Hackney.Core.DynamoDb;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TestStack.BDDfy;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Stories
{

    [Story(
       AsA = "As a service user or application",
       IWant = "to be able to create a new transaction ",
       SoThat = "To be able to process transaction")]
    [Collection("Aws collection")]
    public class CreateTransactionTests : IDisposable
    {
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly TransactionDetailsFixture _transactionFixture;
        private readonly CreateTransactionSteps _steps;

        public CreateTransactionTests(AwsIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;

            _steps = new CreateTransactionSteps(_dbFixture.Client);
            _transactionFixture = new TransactionDetailsFixture(_dbFixture.DynamoDbContext);
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
                if (null != _transactionFixture)
                    _transactionFixture.Dispose();

                _disposed = true;
            }
        }

        [Fact]
        public void ServiceReturnsTheRequestedTransactionDetails()
        {
            this.Given(g => _transactionFixture.GivenANewTransactionRequest())
                .When(w => _steps.WhenTheAddTransactionEndpointIsCalled(_transactionFixture.TransactionRequestObject))
                .Then(t => _steps.ThenTheTransactionDetailsAreSavedAndReturned(_transactionFixture))
                .Then(t => _steps.ThenTheTransactionCreatedEventIsRaised(_dbFixture.SnsVerifer))
                .BDDfy();
        }

        [Theory]
        [InlineData("", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        [InlineData("1234^78", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        [InlineData("12345^789", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public void ServiceReturnsBadRequestWithInvalidBankAccountNumberLengthReturns400(string bankAccountNumber, string message)
        {
            this.Given(g => _transactionFixture.GivenAnNewTransactionRequestWithAnBankAccountNumberLength(bankAccountNumber))
                .When(w => _steps.WhenTheAddTransactionEndpointIsCalled(_transactionFixture.TransactionRequestObject))
                .Then(t => _steps.ThenBadRequestIsReturned(message))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequestWithWithInvalidModelReturns400()
        {
            this.Given(g => _transactionFixture.GivenAnInvalidNewTransactionRequest())
                .When(w => _steps.WhenTheAddTransactionEndpointIsCalled(_transactionFixture.TransactionRequestObject))
                .Then(t => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }

        [Fact]
        public void ServiceSuspenseAccountUpdateWithValidModelReturns200()
        {
            this.Given(g => _transactionFixture.GivenASuspenseAccountTransactionRequest())
                .When(w => _steps.WhenTheSuspenseAccountConfirmationTransactionEndpointIsCalled(_transactionFixture.Transaction))
                .Then(t => _steps.ThenTheTransactionSuspensesAccountDetailsAreUpdatedAndReturned(_transactionFixture))
                .BDDfy();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("false")]
        [InlineData("true")]
        public void ConfigureDynamoDbTestNoLocalModeEnvVarUsesAwsService(string localModeEnvVar)
        {
            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", localModeEnvVar);

            var services = new ServiceCollection();
            services.ConfigureDynamoDB();

            services.Any(x => x.ServiceType == typeof(IAmazonDynamoDB)).Should().BeTrue();
            services.Any(x => x.ServiceType == typeof(IDynamoDBContext)).Should().BeTrue();

            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", null);
        }
    }
}
