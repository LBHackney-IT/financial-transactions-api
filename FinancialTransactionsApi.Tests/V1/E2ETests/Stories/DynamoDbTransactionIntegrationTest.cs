using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Infrastructure.Entities;
using FluentAssertions;
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
    public class DynamoDbTransactionIntegrationTest : AwsIntegrationTests<Startup>
    {
        private const string _token = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJuYW1lIjoidGVzdGluZyIsIm5iZiI6MTYzODQ2NTY3NiwiZXhwIjoyNTM0MDIyOTAwMDAsImlhdCI6MTYzODQ2NTY3Nn0.eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0";

        private readonly AutoFixture.Fixture _fixture = new AutoFixture.Fixture();
        /// <summary>
        /// Method to construct a test entity that can be used in a test
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Transaction ConstructTransaction()
        {
            var entity = _fixture.Create<Transaction>();

            entity.TransactionDate = new DateTime(2021, 8, 1);
            entity.PeriodNo = 35;
            entity.IsSuspense = true;
            entity.SuspenseResolutionInfo = null;
            entity.BankAccountNumber = "12345678";

            return entity;
        }

        /// <summary>
        /// Method to add an entity instance to the database so that it can be used in a test.
        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task SetupTestData(Transaction entity)
        {
            var dbEntity = entity.ToDatabase();
            await DynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);

            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<TransactionDbEntity>(entity.TargetId, entity.Id).ConfigureAwait(false));
        }

        [Fact]
        public async Task GetById_WithInvalidId_Returns404()
        {
            var id = Guid.NewGuid();
            var targetId = Guid.NewGuid();

            var uri = new Uri($"api/v1/transactions/{id}?targetId={targetId}", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<BaseErrorResponse>(responseContent);

            apiEntity.Should().NotBeNull();
            apiEntity.Message.Should().BeEquivalentTo("No transaction by provided Id cannot be found!");
            apiEntity.StatusCode.Should().Be(404);
            apiEntity.Details.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public async Task HealthCheck_Returns200()
        {
            var uri = new Uri($"api/v1/healthcheck/ping", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<HealthCheckResponse>(responseContent);

            apiEntity.Should().NotBeNull();
            apiEntity.Message.Should().BeNull();
            apiEntity.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Add_WithValidModel_Returns201()
        {
            var transaction = ConstructTransaction();

            await CreateTransactionAndValidateResponse(transaction).ConfigureAwait(false);
        }

        [Fact]
        public async Task AddAndThenGetById_WithValidModelAndValidId_Returns201And200()
        {
            var transaction = ConstructTransaction();

            var id = await CreateTransactionAndValidateResponse(transaction).ConfigureAwait(false);

            transaction.Id = id;

            await GetTransactionByIdAndValidateResponse(transaction).ConfigureAwait(false);
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
                stringContent.Headers.Add("Authorization", _token);

                response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);
            }

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<BaseErrorResponse>(responseContent);

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

        [Theory]
        [InlineData("", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        [InlineData("1234^78", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        [InlineData("12345^789", "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public async Task Add_ModelWithInvalidBankAccountNumberLength_Returns400(string bankAccountNumber, string message)
        {
            var transaction = ConstructTransaction();
            transaction.BankAccountNumber = bankAccountNumber;

            var uri = new Uri("api/v1/transactions", UriKind.Relative);
            string body = JsonConvert.SerializeObject(transaction);

            HttpResponseMessage response;
            using (StringContent stringContent = new StringContent(body))
            {
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                stringContent.Headers.Add("Authorization", _token);

                response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);
            }

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<BaseErrorResponse>(responseContent);

            apiEntity.Should().NotBeNull();
            apiEntity.StatusCode.Should().Be(400);
            apiEntity.Details.Should().Be(string.Empty);

            apiEntity.Message.Should().Contain(message);
        }

        [Fact]
        public async Task CreateTwoRentGroupsGetAllReturns200()
        {
            var transactions = new[] { ConstructTransaction(), ConstructTransaction() };

            Guid targetId = Guid.NewGuid();

            transactions[0].TargetId = targetId;
            transactions[1].TargetId = targetId;
            string transType = transactions[0].TransactionType.ToString();
            var startDate = transactions[0].TransactionDate.AddDays(-1).ToString("yyyy-MM-dd");
            var endDate = transactions[1].TransactionDate.AddDays(1).ToString("yyyy-MM-dd");

            foreach (var transaction in transactions)
            {
                var id = await CreateTransactionAndValidateResponse(transaction).ConfigureAwait(false);

                transaction.Id = id;

                await GetTransactionByIdAndValidateResponse(transaction).ConfigureAwait(false);
            }

            var uri = new Uri($"api/v1/transactions?targetId={targetId}&transactionType={transType}&startDate={startDate}&endDate={endDate}&pageSize=11", UriKind.Relative);
            using var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<PagedResult<TransactionResponse>>(responseContent);

            apiEntity.Should().NotBeNull();

            var firstTransaction = apiEntity.Results.Find(r => r.Id.Equals(transactions[0].Id));

            firstTransaction.Should().BeEquivalentTo(transactions[0], opt =>
                opt.Excluding(a => a.FinancialYear)
                    .Excluding(a => a.FinancialMonth)
                    .Excluding(a => a.TransactionDate)
                    .Excluding(a => a.TransactionDate));

            firstTransaction?.FinancialMonth.Should().Be(8);
            firstTransaction?.FinancialYear.Should().Be(2021);
        }

        [Fact]
        public async Task GetTargetIdAndTransactionTypeFoundReturnsResponse()
        {
            var transactionsObj = _fixture.Build<Transaction>()
                             .With(x => x.TargetId, Guid.NewGuid())
                             .With(x => x.TransactionType, TransactionType.Charge)
                             .CreateMany(5);

            var targetId = transactionsObj.FirstOrDefault().TargetId;
            var transType = transactionsObj.FirstOrDefault().TransactionType;

            int d = -5;
            var startDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
            foreach (var entity in transactionsObj)
            {
                entity.TransactionDate = DateTime.Now.AddDays(d);
                await SetupTestData(entity).ConfigureAwait(false);
                d++;
            }
            var endDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
            var uri = new Uri($"api/v1/transactions?targetId={targetId}&transactionType={transType}&startDate={startDate}&endDate={endDate}&pageSize=11", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<PagedResult<TransactionResponse>>(responseContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            foreach (var item in transactionsObj)
            {
                CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<TransactionDbEntity>(item.TargetId, item.Id).ConfigureAwait(false));
            }

            // apiEntity.Total.Should().Be(5);
            apiEntity.Results.Should().HaveCount(5);
        }

        [Fact]
        public async Task AddAndUpdate_WithValidModel_Returns201And200()
        {
            var transaction = new Transaction()
            {
                Id = new Guid("6479ffee-b0e8-4c2a-b887-63f2dec086aa"),
                TransactionDate = new DateTime(2021, 8, 1),
                Address = "Address",
                BalanceAmount = 154.12M,
                ChargedAmount = 123.78M,
                FinancialMonth = 8,
                FinancialYear = 2021,
                BankAccountNumber = "12345678",
                IsSuspense = true,
                PaidAmount = 125.62M,
                PeriodNo = 31,
                TargetId = new Guid("9e067bac-56ed-4802-a83f-b1e32f09177e"),
                TransactionAmount = 186.90M,
                TransactionSource = "DD",
                TransactionType = TransactionType.Rent,
                Person = new Person()
                {
                    Id = new Guid("1c046cca-e9a7-403a-8b6f-8abafc4ee126"),
                    FullName = "Hyan Widro"
                }
            };

            var id = await CreateTransactionAndValidateResponse(transaction).ConfigureAwait(false);

            transaction.Id = id;
            transaction.PaymentReference = "PaymentReference";
            transaction.Fund = "Fund";
            transaction.HousingBenefitAmount = 999.9M;
            transaction.SuspenseResolutionInfo = new SuspenseResolutionInfo()
            {
                IsConfirmed = true,
                IsApproved = true,
                ResolutionDate = new DateTime(2021, 9, 1),
                Note = "Note"
            };
            transaction.IsSuspense = false;

            var updateUri = new Uri($"api/v1/transactions/{transaction.Id}?targetId={transaction.TargetId}", UriKind.Relative);
            string updateTransaction = JsonConvert.SerializeObject(transaction);

            HttpResponseMessage updateResponse;
            using var updateStringContent = new StringContent(updateTransaction);
            updateStringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            updateStringContent.Headers.Add("Authorization", _token);

            updateResponse = await Client.PutAsync(updateUri, updateStringContent).ConfigureAwait(false);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var updateApiEntity = JsonConvert.DeserializeObject<TransactionResponse>(updateResponseContent);

            updateApiEntity.Should().NotBeNull();

            updateApiEntity.Should().BeEquivalentTo(transaction);

            updateApiEntity.FinancialMonth.Should().Be(8);
            updateApiEntity.FinancialYear.Should().Be(2021);
        }

        [Fact]
        public async Task Update_WithInvalidModel_Returns400()
        {
            var transaction = new Transaction()
            {
                TransactionAmount = -2000,
                PaidAmount = -2334,
                ChargedAmount = -213,
                HousingBenefitAmount = -1
            };

            var uri = new Uri($"api/v1/transactions/{Guid.NewGuid()}", UriKind.Relative);
            string body = JsonConvert.SerializeObject(transaction);

            HttpResponseMessage response;
            using (StringContent stringContent = new StringContent(body))
            {
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                stringContent.Headers.Add("Authorization", _token);

                response = await Client.PutAsync(uri, stringContent).ConfigureAwait(false);
            }

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<BaseErrorResponse>(responseContent);

            apiEntity.Should().NotBeNull();
            apiEntity.StatusCode.Should().Be(400);
            apiEntity.Details.Should().Be(string.Empty);

            apiEntity.Message.Should().Contain("The field PeriodNo must be between 1 and 53.");
            apiEntity.Message.Should().Contain("The field TargetId cannot be empty or default.");
            apiEntity.Message.Should().Contain("The field TransactionDate cannot be default value.");
            apiEntity.Message.Should().Contain($"The field PaidAmount must be between 0 and 79228162514264337593543950335.");
            apiEntity.Message.Should().Contain($"The field ChargedAmount must be between 0 and 79228162514264337593543950335.");
            apiEntity.Message.Should().Contain($"The field TransactionAmount must be between 0 and 79228162514264337593543950335.");
            apiEntity.Message.Should().Contain($"The field HousingBenefitAmount must be between 0 and 79228162514264337593543950335.");
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
                IsSuspense = transaction.IsSuspense,
                BankAccountNumber = transaction.BankAccountNumber,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                PeriodNo = transaction.PeriodNo,
                Person = transaction.Person,
                TargetId = transaction.TargetId,
                TransactionAmount = transaction.TransactionAmount,
                TransactionSource = transaction.TransactionSource,
                TransactionType = transaction.TransactionType
            };

            var uri = new Uri("api/v1/transactions", UriKind.Relative);

            string body = JsonConvert.SerializeObject(addRequest);

            using StringContent stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            stringContent.Headers.Add("Authorization", _token);

            using var response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);

            CleanupActions.Add(async () => await DynamoDbContext.DeleteAsync<TransactionDbEntity>(apiEntity.TargetId, apiEntity.Id).ConfigureAwait(false));

            apiEntity.Should().NotBeNull();

            apiEntity.Should().BeEquivalentTo(transaction, options => options.Excluding(a => a.Id)
                                                                             .Excluding(a => a.SuspenseResolutionInfo)
                                                                             .Excluding(a => a.FinancialYear)
                                                                             .Excluding(a => a.FinancialMonth));

            apiEntity.SuspenseResolutionInfo.Should().BeNull();
            apiEntity.FinancialMonth.Should().Be(8);
            apiEntity.FinancialYear.Should().Be(2021);

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

            apiEntity.Should().BeEquivalentTo(transaction, options => options.Excluding(a => a.FinancialYear)
                                                                             .Excluding(a => a.FinancialMonth)
                                                                             .Excluding(a => a.TransactionDate));

            apiEntity.FinancialMonth.Should().Be(8);
            apiEntity.FinancialYear.Should().Be(2021);
        }
    }
}
