//using AutoFixture;
//using FinancialTransactionsApi.V1.Infrastructure.Entities;
//using FluentAssertions;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using FinancialTransactionsApi;
//using FinancialTransactionsApi.Tests;
//using FinancialTransactionsApi.V1.Boundary;
//using FinancialTransactionsApi.V1.Boundary.Response;
//using FinancialTransactionsApi.V1.Domain;
//using FinancialTransactionsApi.V1.Factories;
//using Xunit;

//namespace FinancialTransactionsApi.Tests.V1.E2ETests.Stories
//{
//    [Collection("DynamoDb collection")]
//    public class DynamoDbTransactionIntegrationTest: IDisposable
//    {
//        private readonly Fixture _fixture = new Fixture();
//        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
//        private readonly List<Action> _cleanupActions = new List<Action>();
//        public DynamoDbTransactionIntegrationTest(DynamoDbIntegrationTests<Startup> dbFixture)
//        {
//            _dbFixture = dbFixture;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        private bool _disposed;
//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing && !_disposed)
//            {
//                foreach (var action in _cleanupActions)
//                    action();

//                _disposed = true;
//            }
//        }

//        /// <summary>
//        /// Method to construct a test entity that can be used in a test
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        private Transaction ConstructTestEntity()
//        {
//            var entity = _fixture.Create<Transaction>();
//            entity.TransactionDate = DateTime.UtcNow;
//            return entity;
//        }

//        /// <summary>
//        /// Method to add an entity instance to the database so that it can be used in a test.
//        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        private async Task SetupTestData(Transaction entity)
//        {
//            await _dbFixture.DynamoDbContext.SaveAsync(entity.ToDatabase()).ConfigureAwait(false);
//            _cleanupActions.Add(async () => await _dbFixture.DynamoDbContext.DeleteAsync<TransactionDbEntity>(entity.Id).ConfigureAwait(false));
//        }

//        [Fact]
//        public async Task GetTransactionByIdNotFoundReturns404()
//        {
//            var id = Guid.NewGuid();
//            //TODO: Update uri route to match the APIs endpoint
//            var uri = new Uri($"api/v1/transactions/{id}", UriKind.Relative);
//            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

//            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        [Fact]
//        public async Task GetTransactionBydIdFoundReturnsResponse()
//        {
//            var entity = ConstructTestEntity();
//            await SetupTestData(entity).ConfigureAwait(false);

//            //TODO: Update uri route to match the APIs endpoint
//            var uri = new Uri($"api/v1/transactions/{entity.Id}", UriKind.Relative);
//            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//            var apiEntity = JsonConvert.DeserializeObject<TransactionResponseObject>(responseContent);
//            apiEntity.Should().BeEquivalentTo(entity.ToResponse());
//           // apiEntity.Should().BeEquivalentTo(entity, (x) => x.Excluding(y => y.TransactionDate));
//           // apiEntity.TransactionDate.Should().BeCloseTo(DateTime.UtcNow, 1000);
//        }

//        [Fact]
//        public async Task GetEntityByTargetIdAndTransactionTypeNotFoundReturns404()
//        {
//            var targetId = Guid.NewGuid();
//            var transType = "Nothing";
//            var date = DateTime.Now.ToString("yyyy-MM-dd");
//            //TODO: Update uri route to match the APIs endpoint
//            var uri = new Uri($"api/v1/transactions?targetId={targetId}&transactionType={transType}&startDate={date}&endDate={date}", UriKind.Relative);
//            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);
//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//            var apiEntity = JsonConvert.DeserializeObject<TransactionResponseObjectList>(responseContent);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            apiEntity.ResponseObjects.Count.Should().Be(0);
//        }

//        [Fact]
//        public async Task GetTargetIdAndTransactionTypeFoundReturnsResponse()
//        {
            
//            var transactionsObj = _fixture.Build<Transaction>()
//                             .With(x => x.TargetId, Guid.NewGuid())
//                             .With(x => x.TransactionType, "Sample")
//                             .CreateMany(5);
//            var targetId = transactionsObj.FirstOrDefault().TargetId;
//            var transType = transactionsObj.FirstOrDefault().TransactionType;
//            int d = -5;
//            var startDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
//            foreach (var entity in transactionsObj)
//            {
//                entity.TransactionDate = DateTime.Now.AddDays(d);
//                await SetupTestData(entity).ConfigureAwait(false);
//                d++;
//            }
//            var endDate = DateTime.Now.AddDays(d).ToString("yyyy-MM-dd");
//            var uri = new Uri($"api/v1/transactions?targetId={targetId}&transactionType={transType}&startDate={startDate}&endDate={endDate}", UriKind.Relative);
//            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//            var apiEntity = JsonConvert.DeserializeObject<TransactionResponseObjectList>(responseContent);
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            foreach (var item in transactionsObj)
//            {
//                _cleanupActions.Add(async () => await _dbFixture.DynamoDbContext.DeleteAsync<TransactionDbEntity>(item.Id).ConfigureAwait(false));
//            }
//            apiEntity.ResponseObjects.Count.Should().Be(5);
//        }
//        [Fact]
//        public async Task CreateTransactionCreatedReturns200()
//        {
//            var entity = _fixture.Create<Transaction>();

//            await CreateTransactionAndValidateResponse(entity).ConfigureAwait(false);
//        }

//        [Fact]
//        public async Task HealchCheckOkReturns200()
//        {
//            var uri = new Uri($"api/v1/healthcheck/ping", UriKind.Relative);
//            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//            var apiEntity = JsonConvert.DeserializeObject<HealthCheckResponse>(responseContent);

//            apiEntity.Should().NotBeNull();
//            apiEntity.Message.Should().BeNull();
//            apiEntity.Success.Should().BeTrue();
//        }
//        private async Task CreateTransactionAndValidateResponse(Transaction transaction)
//        {
//            var uri = new Uri("api/v1/transactions", UriKind.Relative);

//            string body = JsonConvert.SerializeObject(transaction);

//            using StringContent stringContent = new StringContent(body);
//            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//            using var response = await _dbFixture.Client.PostAsync(uri, stringContent).ConfigureAwait(false);

//            response.StatusCode.Should().Be(HttpStatusCode.Created);

//            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
//            var apiEntity = JsonConvert.DeserializeObject<TransactionResponseObject>(responseContent);

//            _cleanupActions.Add(async () => await _dbFixture.DynamoDbContext.DeleteAsync<TransactionDbEntity>(apiEntity.Id).ConfigureAwait(false));

//            apiEntity.Should().NotBeNull();

//            //apiEntity.Should().BeEquivalentTo(transaction, options => options.Excluding(a => a.Id));
//        }
//    }
//}
