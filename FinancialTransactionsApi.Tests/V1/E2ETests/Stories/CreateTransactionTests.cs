using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TestStack.BDDfy;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Stories
{

    [Story(
       AsA = "As a service user or application",
       IWant = "to be able to create a new transaction ",
       SoThat = "To be able to process transaction")]
    [Collection("DynamoDb collection")]
    public class CreateTransactionTests : IDisposable
    {
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly TransactionDetailsFixture _transactionFixture;
        private readonly CreateTransactionSteps _steps;

        public CreateTransactionTests(DynamoDbIntegrationTests<Startup> dbFixture)
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
                //.Then(t => _steps.ThenTheContactDetailsCreatedEventIsRaised(_dbFixture.SnsVerifer))
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
        public void ServiceUpdateWithValidModelReturns200()
        {
            this.Given(g => _transactionFixture.GivenAUpdateTransactionRequest())
                .When(w => _steps.WhenTheUpdateTransactionEndpointIsCalled(_transactionFixture.Transaction))
                .Then(t => _steps.ThenTheTransactionDetailsAreUpdatedAndReturned(_transactionFixture))
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

        //[Fact]
        //public void ServiceSavesMultilineAddressToValueFieldWhenContactTypeIsAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWhereContactTypeIsAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.TheMultilineAddressIsSavedInTheValueField(_contactDetailsFixture))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceDoesntSaveMultilineAddressToValueFieldWhenContactTypeIsNotAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWhereContactTypeIsNotAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.TheMultilineAddressIsNotSavedInTheValueField(_contactDetailsFixture))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceReturnsBadRequestWithInvalidContactDetails()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenAnInvalidNewContactRequest())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenBadRequestIsReturned())
        //        .And(t => _steps.ThenTheResponseIncludesValidationErrors())
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceReturnsBadRequestWithInvalidPhoneNumber()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenAnNewContactRequestWithAnInvalidPhoneNumber())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("Value", ErrorCodes.InvalidPhoneNumber))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceReturnsBadRequestWithInvalidEmail()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenAnNewContactRequestWithAnInvalidEmail())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenBadRequestValidationErrorResultIsReturned("Value", ErrorCodes.InvalidEmail))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceDoesntValidateAddressLine1WhenContactTypeNotAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWithInvalidAddressLine1WhenTheContactTypeNotAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenThereIsNoValidationErrorForField("AddressLine1"))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceDoesntValidatePostCodeWhenContactTypeNotAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWithInvalidPostCodeWhenTheContactTypeNotAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenThereIsNoValidationErrorForField("PostCode"))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceValidatesAddressLine1WhenContactTypeIsAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWithInvalidAddressLine1WhenTheContactTypeIsAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenThereIsAValidationErrorForField("AddressLine1"))
        //        .BDDfy();
        //}

        //[Fact]
        //public void ServiceValidatesPostCodeWhenContactTypeIsAddress()
        //{
        //    this.Given(g => _contactDetailsFixture.GivenANewContactRequestWithInvalidPostCodeWhenTheContactTypeIsAddress())
        //        .When(w => _steps.WhenTheCreateContactEndpointIsCalled(_contactDetailsFixture.ContactRequestObject))
        //        .Then(t => _steps.ThenThereIsAValidationErrorForField("PostCode"))
        //        .BDDfy();
        //}
    }
}
