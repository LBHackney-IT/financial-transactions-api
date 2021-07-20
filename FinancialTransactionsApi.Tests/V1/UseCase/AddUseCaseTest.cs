//using AutoFixture;
//using FinancialTransactionsApi.V1.Boundary.Request;
//using FinancialTransactionsApi.V1.UseCase;
//using FluentAssertions;
//using Moq;
//using System.Threading.Tasks;
//using FinancialTransactionsApi.V1.Boundary.Response;
//using FinancialTransactionsApi.V1.Domain;
//using FinancialTransactionsApi.V1.Factories;
//using FinancialTransactionsApi.V1.Gateways;
//using Xunit;

//namespace FinancialTransactionsApi.Tests.V1.UseCase
//{
//    public class AddUseCaseTest
//    {
//        private Mock<ITransactionGateway> _mockGateway;
//        private AddUseCase _classUnderTest;
//        private Fixture _fixture;
//        public AddUseCaseTest()
//        {
//            _mockGateway = new Mock<ITransactionGateway>();
//            _classUnderTest = new AddUseCase(_mockGateway.Object);
//            _fixture = new Fixture();
//        }
       

//        [Fact]
//        public async Task AddTransactionsAsync()
//        {
//            var entity = _fixture.Create<TransactionRequest>();
//            var responseEntity = _fixture.Create<TransactionResponseObject>();
            
//            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Transaction>())).Returns(Task.FromResult(false));

//            // Act
//            var response = await _classUnderTest.ExecuteAsync(entity)
//                .ConfigureAwait(false);

//            // Assert
//            var expectedResponse = entity.ToTransactionDomain().ToResponse();
//            response.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id));
           
//        }
//    }
//}
