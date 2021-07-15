using AutoFixture;
using TransactionsApi.Tests.V1.Helper;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Gateways;
using FluentAssertions;
using NUnit.Framework;

namespace TransactionsApi.Tests.V1.Gateways
{
    //TODO: Remove this file if Postgres gateway is not being used
    //TODO: Rename Tests to match gateway name
    //For instruction on how to run tests please see the wiki: https://github.com/LBHackney-IT/lbh-base-api/wiki/Running-the-test-suite.
    [TestFixture]
    public class ExampleGatewayTests : DatabaseTests
    {
        //private readonly Fixture _fixture = new Fixture();
        private TransactionGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new TransactionGateway(DatabaseContext);
        }

        ////[Test]
        ////public void GetEntityByIdReturnsNullIfEntityDoesntExist()
        ////{
        ////    var response = _classUnderTest.GetEntityById(123);

        ////    response.Should().BeNull();
        ////}

        //[Test]
        //public void GetEntityByIdReturnsTheEntityIfItExists()
        //{
        //    var entity = _fixture.Create<Transaction>();
        //    var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

        //    DatabaseContext.TransactionEntities.Add(databaseEntity);
        //    DatabaseContext.SaveChanges();

        //    var response = _classUnderTest.GetEntityById(databaseEntity.Id);

        //    databaseEntity.Id.Should().Be(response.Id);
        //    databaseEntity.CreatedAt.Should().BeSameDateAs(response.CreatedAt);
        //}

        //TODO: Add tests here for the get all method.
    }
}