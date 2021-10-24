using FinancialTransactionsApi.Tests.V1.E2ETests.Fixture;
using FinancialTransactionsApi.Tests.V1.E2ETests.Steps.Base;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure.Entities;

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
                IsSuspense = response.IsSuspense,
                BankAccountNumber = response.BankAccountNumber,
                PaidAmount = response.PaidAmount,
                PaymentReference = response.PaymentReference,
                PeriodNo = response.PeriodNo,
                Person = response.Person,
                TargetId = response.TargetId,
                TransactionAmount = response.TransactionAmount,
                TransactionSource = response.TransactionSource,
                TransactionType = response.TransactionType
            };
        }

        //public async Task ThenTheContactDetailsCreatedEventIsRaised(SnsEventVerifier<ContactDetailsSns> snsVerifer)
        //{
        //    var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);

        //    Action<ContactDetailsSns> verifyFunc = (actual) =>
        //    {
        //        actual.CorrelationId.Should().NotBeEmpty();
        //        actual.DateTime.Should().BeCloseTo(DateTime.UtcNow, 1000);
        //        actual.EntityId.Should().Be(apiResult.TargetId);

        //        actual.EventData.NewData.ContactType.Should().Be((int) apiResult.ContactInformation.ContactType);
        //        actual.EventData.NewData.Description.Should().Be(apiResult.ContactInformation.Description);
        //        actual.EventData.NewData.Id.Should().Be(apiResult.Id);
        //        actual.EventData.NewData.Value.Should().Be(apiResult.ContactInformation.Value);
        //        actual.EventData.OldData.Should().BeEquivalentTo(new DataItem());

        //        actual.EventType.Should().Be(EventConstants.CREATED);
        //        actual.Id.Should().NotBeEmpty();
        //        actual.SourceDomain.Should().Be(EventConstants.SOURCEDOMAIN);
        //        actual.SourceSystem.Should().Be(EventConstants.SOURCESYSTEM);
        //        actual.User.Email.Should().Be("e2e-testing@development.com");
        //        actual.User.Name.Should().Be("Tester");
        //        actual.Version.Should().Be(EventConstants.V1VERSION);
        //    };

        //    snsVerifer.VerifySnsEventRaised(verifyFunc).Should().BeTrue(snsVerifer.LastException?.Message);
        //}

        public async Task WhenTheAddTransactionEndpointIsCalled(AddTransactionRequest requestObject)
        {

            var route = new Uri("api/v1/transactions", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject);

            using var stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _lastResponse = await _httpClient.PostAsync(route, stringContent).ConfigureAwait(false);
        }


        public async Task WhenTheUpdateTransactionEndpointIsCalled(Transaction requestObject)
        {
            var route = new Uri($"api/v1/transactions/{requestObject.Id}", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject);

            using var stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _lastResponse = await _httpClient.PutAsync(route, stringContent).ConfigureAwait(false);
        }

        private async Task<List<string>> GetResponseErrorProperties()
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jo = JObject.Parse(responseContent);

            // throws error if there are no errors
            var errorProperties = jo["errors"].Children().Select(x => x.Path.Split('.').Last().Trim('\'', ']')).ToList();

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

            expected.Should().BeEquivalentTo(apiEntity, options => options.Excluding(a => a.Id)
                                                                             .Excluding(a => a.SuspenseResolutionInfo)
                                                                             .Excluding(a => a.FinancialYear)
                                                                             .Excluding(a => a.FinancialMonth));

            apiEntity.SuspenseResolutionInfo.Should().BeNull();
            apiEntity.FinancialMonth.Should().Be(8);
            apiEntity.FinancialYear.Should().Be(2021);
        }
        public async Task ThenTheTransactionDetailsAreUpdatedAndReturned(TransactionDetailsFixture fixture)
        {

            var expected = fixture.Transaction;
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var updateApiEntity = JsonSerializer.Deserialize<TransactionResponse>(responseContent, _jsonOptions);

            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            updateApiEntity.Should().NotBeNull();

            updateApiEntity.Should().BeEquivalentTo(expected);

            updateApiEntity.FinancialMonth.Should().Be(expected.FinancialMonth);
            updateApiEntity.FinancialYear.Should().Be(expected.FinancialYear);
        }
        //public async Task TheMultilineAddressIsSavedInTheValueField(ContactDetailsFixture fixture)
        //{
        //    var expected = fixture.ContactRequestObject;
        //    var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);

        //    var dbEntity = await fixture._dbContext.LoadAsync<ContactDetailsEntity>(apiResult.TargetId, apiResult.Id).ConfigureAwait(false);

        //    dbEntity.ContactInformation.ContactType.Should().Be(ContactType.address);

        //    // assert multiline address saved in value field
        //    dbEntity.ContactInformation.Value.Should().Contain(expected.ContactInformation.AddressExtended.AddressLine1);
        //    dbEntity.ContactInformation.Value.Should().Contain(expected.ContactInformation.AddressExtended.AddressLine2);
        //    dbEntity.ContactInformation.Value.Should().Contain(expected.ContactInformation.AddressExtended.AddressLine3);
        //    dbEntity.ContactInformation.Value.Should().Contain(expected.ContactInformation.AddressExtended.AddressLine4);
        //    dbEntity.ContactInformation.Value.Should().Contain(expected.ContactInformation.AddressExtended.PostCode);
        //}

        //public async Task TheMultilineAddressIsNotSavedInTheValueField(ContactDetailsFixture fixture)
        //{
        //    var expected = fixture.ContactRequestObject;
        //    var apiResult = await ExtractResultFromHttpResponse(_lastResponse).ConfigureAwait(false);

        //    var dbEntity = await fixture._dbContext.LoadAsync<ContactDetailsEntity>(apiResult.TargetId, apiResult.Id).ConfigureAwait(false);

        //    dbEntity.ContactInformation.ContactType.Should().NotBe(ContactType.address);

        //    dbEntity.ContactInformation.Value.Should().Be(expected.ContactInformation.Value);

        //    // assert multiline address saved in value field
        //    dbEntity.ContactInformation.Value.Should().NotContain(expected.ContactInformation.AddressExtended.AddressLine1);
        //    dbEntity.ContactInformation.Value.Should().NotContain(expected.ContactInformation.AddressExtended.AddressLine2);
        //    dbEntity.ContactInformation.Value.Should().NotContain(expected.ContactInformation.AddressExtended.AddressLine3);
        //    dbEntity.ContactInformation.Value.Should().NotContain(expected.ContactInformation.AddressExtended.AddressLine4);
        //    dbEntity.ContactInformation.Value.Should().NotContain(expected.ContactInformation.AddressExtended.PostCode);
        //}

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
            apiEntity.StatusCode.Should().Be(400);
            apiEntity.Details.Should().Be(string.Empty);

            apiEntity.Message.Should().Contain("The field PeriodNo must be between 1 and 53.");
            apiEntity.Message.Should().Contain("The field TargetId cannot be empty or default.");
            apiEntity.Message.Should().Contain("The field TransactionDate cannot be default value.");
            apiEntity.Message.Should().Contain($"The field PaidAmount must be between 0 and {(double) decimal.MaxValue}.");
            apiEntity.Message.Should().Contain($"The field ChargedAmount must be between 0 and {(double) decimal.MaxValue}.");
            apiEntity.Message.Should().Contain($"The field TransactionAmount must be between 0 and {(double) decimal.MaxValue}.");
            apiEntity.Message.Should().Contain($"The field HousingBenefitAmount must be between 0 and {(double) decimal.MaxValue}.");
        }

        public async Task ThenBadRequestIsReturned(string message)
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<BaseErrorResponse>(responseContent, _jsonOptions);

            apiEntity.Should().NotBeNull();
            apiEntity.StatusCode.Should().Be(400);
            apiEntity.Details.Should().Be(string.Empty);

            apiEntity.Message.Should().Contain(message);
            _lastResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName)
        {
            await ThenBadRequestValidationErrorResultIsReturned(propertyName, null, null).ConfigureAwait(false);
        }

        public async Task ThenBadRequestValidationErrorResultIsReturned(string propertyName, string errorCode)
        {
            await ThenBadRequestValidationErrorResultIsReturned(propertyName, errorCode, null).ConfigureAwait(false);
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
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiResult = JsonSerializer.Deserialize<TransactionResponse>(responseContent, _jsonOptions);

            return apiResult;
        }

        //private static Token GetToken(string jwt)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(jwt);
        //    var decodedPayload = Base64UrlEncoder.Decode(jwtToken.EncodedPayload);
        //    return JsonConvert.DeserializeObject<Token>(decodedPayload);
        //}
    }
}
