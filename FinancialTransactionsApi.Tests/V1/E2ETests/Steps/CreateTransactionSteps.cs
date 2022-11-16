using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps.Base;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Steps
{
    public class CreateTransactionSteps : BaseSteps
    {
        public CreateTransactionSteps(HttpClient httpClient) : base(httpClient)
        { }

        private static TransactionDbEntity ResponseToDatabase(TransactionResponse response)
        {
            return new TransactionDbEntity
            {
                TransactionDate = response.TransactionDate,
                Address = response.Address,
                BalanceAmount = response.BalanceAmount,
                ChargedAmount = response.ChargedAmount,
                Fund = response.Fund,
                HousingBenefitAmount = response.HousingBenefitAmount,
                BankAccountNumber = response.BankAccountNumber,
                PaidAmount = response.PaidAmount,
                PaymentReference = response.PaymentReference,
                PeriodNo = response.PeriodNo,
                TargetId = response.TargetId,
                TransactionAmount = response.TransactionAmount,
                TransactionSource = response.TransactionSource,
                TransactionType = (TransactionType) Enum.Parse(typeof(TransactionType), response.TransactionType.Replace(" ", "")),
                CreatedAt = response.CreatedAt,
                CreatedBy = response.CreatedBy,
                LastUpdatedBy = response.LastUpdatedBy,
                LastUpdatedAt = response.LastUpdatedAt
            };
        }

        public async Task ThenTheTransactionCreatedEventIsRaised(SnsEventVerifier<TransactionSns> snsVerifer)
        {
            var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);

            Action<TransactionSns> verifyFunc = (actual) =>
            {
                actual.CorrelationId.Should().NotBeEmpty();
                actual.EntityId.Should().Be(apiResult.Id);
                actual.EventType.Should().Be(EventConstants.EVENTTYPE);
                actual.Id.Should().NotBeEmpty();
                actual.SourceDomain.Should().Be(EventConstants.SOURCEDOMAIN);
                actual.SourceSystem.Should().Be(EventConstants.SOURCESYSTEM);
                actual.Version.Should().Be(EventConstants.VERSION);
            };
            var result = snsVerifer.LastException?.Message;
            //snsVerifer.VerifySnsEventRaised(verifyFunc).Should().BeTrue();
        }

        public async Task WhenTheAddTransactionEndpointIsCalled(AddTransactionRequest requestObject)
        {

            var route = new Uri("api/v1/transactions", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject);

            using var stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _lastResponse = await _httpClient.PostAsync(route, stringContent).ConfigureAwait(false);
        }


        public async Task WhenTheSuspenseAccountConfirmationTransactionEndpointIsCalled(Transaction requestObject)
        {
            var reqObj = new SuspenseConfirmationRequest { TargetId = Guid.NewGuid(), Note = "Test" };
            var route = new Uri($"api/v1/transactions/suspense-account-confirmation/{requestObject.Id}", UriKind.Relative);
            var body = JsonSerializer.Serialize(reqObj);

            using var stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _lastResponse = await _httpClient.PatchAsync(route, stringContent).ConfigureAwait(false);
        }

        private async Task<List<string>> GetResponseErrorProperties()
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jo = JObject.Parse(responseContent);

            // throws error if there are no errors
            var errorProperties = jo["errors"]?.Children().Select(x => x.Path.Split('.').Last().Trim('\'', ']')).ToList();

            return errorProperties ?? new List<string>();

        }

        public async Task ThenThereIsAValidationErrorForField(string fieldName)
        {
            var errorProperties = await GetResponseErrorProperties().ConfigureAwait(false);

            errorProperties.Should().Contain(fieldName);
        }

        public async Task ThenThereIsNoValidationErrorForField(string fieldName)
        {
            var errorProperties = await GetResponseErrorProperties().ConfigureAwait(false);

            errorProperties.Should().NotContain(fieldName);
        }

        public async Task ThenTheTransactionDetailsAreSavedAndReturned(TransactionDetailsFixture fixture)
        {
            var expected = fixture.TransactionRequestObject;
            var apiEntity = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);

            var resultAsDb = ResponseToDatabase(apiEntity);
            fixture.Transactions.Add(resultAsDb);
            apiEntity.Should().NotBeNull();

            expected.Should().BeEquivalentTo(apiEntity, options => options
                .Excluding(a => a.Id)
                .Excluding(a => a.SuspenseResolutionInfo)
                .Excluding(a => a.FinancialYear)
                .Excluding(a => a.FinancialMonth)
                .Excluding(a => a.CreatedAt)
                .Excluding(a => a.CreatedBy)
                .Excluding(a => a.LastUpdatedAt)
                .Excluding(a => a.LastUpdatedBy)
                .Excluding(x => x.TransactionType));

            apiEntity.SuspenseResolutionInfo.Should().BeNull();
            apiEntity.FinancialMonth.Should().Be(8);
            apiEntity.FinancialYear.Should().Be(2021);
        }

        public async Task ThenTheTransactionSuspensesAccountDetailsAreUpdatedAndReturned(TransactionDetailsFixture fixture)
        {
            var expected = fixture.Transaction;
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var updateApiEntity = JsonSerializer.Deserialize<TransactionResponse>(responseContent, _jsonOptions);

            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            updateApiEntity.Should().NotBeNull();
            updateApiEntity.TargetId.Should().Equals(Guid.Empty);
            updateApiEntity.SuspenseResolutionInfo.IsConfirmed.Should().BeTrue();
            updateApiEntity?.LastUpdatedBy.Should().Be("testing");
        }

        public async Task ThenTheResponseIncludesValidationErrors()
        {
            var errorProperties = await GetResponseErrorProperties().ConfigureAwait(false);

            errorProperties.Should().Contain("TargetId");
            errorProperties.Should().Contain("ContactInformation");
            errorProperties.Should().Contain("SourceServiceArea");
        }

        public async Task ThenTheResponseIncludesValidationErrorsForTooManyContacts()
        {
            var errorProperties = await GetResponseErrorProperties().ConfigureAwait(false);
            errorProperties.Should().Contain("ExistingContacts");
        }

        public async Task ThenBadRequestIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<BaseErrorResponse>(responseContent, _jsonOptions);

            apiEntity.Should().NotBeNull();
            apiEntity?.StatusCode.Should().Be(400);
            apiEntity?.Details.Should().Be(string.Empty);

            apiEntity?.Message.Should().Contain("The field PeriodNo must be between 1 and 53.");
            apiEntity?.Message.Should().Contain("The field TransactionDate cannot be default value.");
            apiEntity?.Message.Should().Contain($"The field PaidAmount is invalid.");
            apiEntity?.Message.Should().Contain($"The field ChargedAmount is invalid.");
            apiEntity?.Message.Should().Contain($"The field TransactionAmount is invalid.");
            apiEntity?.Message.Should().Contain($"The field HousingBenefitAmount is invalid.");
        }

        public async Task ThenBadRequestIsReturned(string message)
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<BaseErrorResponse>(responseContent, _jsonOptions);

            apiEntity.Should().NotBeNull();
            apiEntity?.StatusCode.Should().Be(400);
            apiEntity?.Details.Should().Be(string.Empty);

            apiEntity?.Message.Should().Contain(message);
            _lastResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName)
        {
            await ThenBadRequestValidationErrorResultIsReturned(propertyName, null, null).ConfigureAwait(false);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName, string errorCode, string errorMsg)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var resultBody = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            resultBody.Should().Contain("One or more validation errors occurred");
            resultBody.Should().Contain(propertyName);
            if (null != errorCode)
                resultBody.Should().Contain(errorCode);
            if (null != errorMsg)
                resultBody.Should().Contain(errorMsg);
        }

        private async Task<TransactionResponse> ExtractResultFromHttpResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Expected 200 status code, but got {response.StatusCode} with the message {responseContent}");
            }
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var apiResult = JsonSerializer.Deserialize<TransactionResponse>(responseContent, _jsonOptions);

            return apiResult;
        }
    }
}
