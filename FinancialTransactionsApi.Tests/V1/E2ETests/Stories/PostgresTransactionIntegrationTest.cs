using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
using FluentAssertions.Common;
using Hackney.Core.DynamoDb;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.E2ETests.Stories
{
    public class PostgresTransactionIntegrationTest : AwsIntegrationTests<Startup>
    {
        private readonly AutoFixture.Fixture _fixture = new AutoFixture.Fixture();

        /// <summary>
        /// Method to construct a test entity that can be used in a test
        /// </summary>
        /// <returns></returns>
        private Transaction ConstructTransaction()
        {
            var entity = _fixture.Create<Transaction>();

            entity.TransactionDate = new DateTime(2021, 8, 1);
            entity.PeriodNo = 35;
            entity.SuspenseResolutionInfo = null;
            entity.BankAccountNumber = "12345678";

            return entity;
        }

        [Fact]
        public async Task GetByTargetId_WithInvalidId_Returns404()
        {
            var targetId = Guid.NewGuid();

            var uri = new Uri($"api/v1/transactions/{targetId}/tenureId", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().NotBeNull();
        }

        [Fact]
        public async Task HealthCheck_Returns200()
        {
            var uri = new Uri($"api/v1/healthcheck/ping", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Add_WithInvalidModel_Returns400()
        {
            var transaction = new Transaction()
            {
                TransactionAmount = -2000,
                PaidAmount = -2334,
                ChargedAmount = -213,
                HousingBenefitAmount = -1
            };

            var uri = new Uri("api/v1/transactions", UriKind.Relative);
            string body = JsonConvert.SerializeObject(transaction);

            HttpResponseMessage response;
            using (StringContent stringContent = new StringContent(body))
            {
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);
            }

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234^78")]
        [InlineData("12345^789")]
        public async Task Add_ModelWithInvalidBankAccountNumberLength_Returns400(string bankAccountNumber)
        {
            var transaction = ConstructTransaction();
            transaction.BankAccountNumber = bankAccountNumber;

            var uri = new Uri("api/v1/transactions", UriKind.Relative);
            string body = JsonConvert.SerializeObject(transaction);

            HttpResponseMessage response;
            using (StringContent stringContent = new StringContent(body))
            {
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);
            }
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("false")]
        [InlineData("true")]
        public void ConfigureDynamoDbTestNoLocalModeEnvVarUsesAwsService(string localModeEnvVar)
        {
            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", localModeEnvVar);

            ServiceCollection services = new ServiceCollection();
            services.ConfigureDynamoDB();

            services.Any(x => x.ServiceType == typeof(IAmazonDynamoDB)).Should().BeTrue();
            services.Any(x => x.ServiceType == typeof(IDynamoDBContext)).Should().BeTrue();

            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", null);
        }

        private async Task<Guid> CreateTransactionAndValidateResponse(Transaction transaction)
        {
            var addRequest = new AddTransactionRequest()
            {
                TransactionDate = transaction.TransactionDate,
                Address = transaction.Address,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                Fund = transaction.Fund,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                BankAccountNumber = transaction.BankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PeriodNo = transaction.PeriodNo,
                SortCode = transaction.SortCode,
                Sender = transaction.Sender,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                TransactionType = transaction.TransactionType
            };

            var uri = new Uri("api/v1/transactions", UriKind.Relative);

            string body = JsonConvert.SerializeObject(addRequest);

            using StringContent stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using var response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created, responseContent);

            var apiEntity = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);

            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<TransactionDbEntity>(apiEntity.TargetId, apiEntity.Id).ConfigureAwait(false));

            apiEntity.Should().NotBeNull();

            apiEntity.Should().BeEquivalentTo(transaction, options => options
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
            apiEntity.CreatedBy.Should().Be("testing");
            apiEntity.LastUpdatedBy.Should().Be("testing");

            return apiEntity.Id;
        }

        private async Task GetTransactionByIdAndValidateResponse(Transaction transaction)
        {
            var uri = new Uri($"api/v1/transactions/{transaction.Id}?targetId={transaction.TargetId}", UriKind.Relative);
            using var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);
            apiEntity.Should().NotBeNull();

            apiEntity.Should().BeEquivalentTo(transaction, options => options
                .Excluding(a => a.FinancialYear)
                .Excluding(a => a.FinancialMonth)
                .Excluding(a => a.TransactionDate)
                .Excluding(a => a.CreatedAt)
                .Excluding(a => a.CreatedBy)
                .Excluding(a => a.LastUpdatedAt)
                .Excluding(a => a.LastUpdatedBy)
                .Excluding(x => x.TransactionType));

            apiEntity.FinancialMonth.Should().Be(8);
            apiEntity.FinancialYear.Should().Be(2021);
        }
    }
}
