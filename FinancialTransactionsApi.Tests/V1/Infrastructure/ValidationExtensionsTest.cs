using AutoFixture;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.Domain;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace FinancialTransactionsApi.Tests.V1.Infrastructure
{
    public class ValidationExtensionsTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ValidationAddTransactionRequest_TransactionReques_ShouldNotBeNull()
        {
            var transactionRequest = _fixture.Create<AddTransactionRequest>();

            var result = transactionRequest.HaveAllFieldsInAddTransactionModel();

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidationAddTransactionRequest_TransactionSource_ShouldBeNull()
        {
            var transactionRequest = _fixture.Create<AddTransactionRequest>();

            transactionRequest.TransactionSource = null;

            var result = transactionRequest.HaveAllFieldsInAddTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationAddTransactionRequest_PaymentReference_ShouldBeNull()
        {
            var transactionRequest = _fixture.Create<AddTransactionRequest>();

            transactionRequest.PaymentReference = null;

            var result = transactionRequest.HaveAllFieldsInAddTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationAddTransactionRequest_Fund_ShouldBeNull()
        {
            var transactionRequest = _fixture.Create<AddTransactionRequest>();

            transactionRequest.Fund = null;

            var result = transactionRequest.HaveAllFieldsInAddTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_TransactionReques_ShouldNotBeNull()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.SuspenseResolutionInfo.IsConfirmed = transactionRequest.SuspenseResolutionInfo.IsApproved = true;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeTrue();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_TargetId_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.TargetId = It.IsAny<Guid>();

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_TransactionSource_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.TransactionSource = null;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_PaymentReference_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.PaymentReference = null;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_Sender_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_SenderIdAndFullName_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_Fund_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.Fund = null;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_SuspenseResolutionInfo_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.SuspenseResolutionInfo = null;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_ResolutionDate_ShouldBeEmpty()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.SuspenseResolutionInfo.ResolutionDate = null;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_IsResolved_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.SuspenseResolutionInfo.IsConfirmed = transactionRequest.SuspenseResolutionInfo.IsApproved = false;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransactionRequest_ResolutionDateAndIsResoled_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<UpdateTransactionRequest>();

            transactionRequest.SuspenseResolutionInfo.ResolutionDate = null;
            transactionRequest.SuspenseResolutionInfo.IsConfirmed = transactionRequest.SuspenseResolutionInfo.IsApproved = false;

            var result = transactionRequest.HaveAllFieldsInUpdateTransactionModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransaction_TargetId_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<Transaction>();

            transactionRequest.TargetId = It.IsAny<Guid>();

            var result = transactionRequest.HaveAllFieldsInBatchProcessingModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransaction_TransactionSource_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<Transaction>();

            transactionRequest.TransactionSource = null;

            var result = transactionRequest.HaveAllFieldsInBatchProcessingModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransaction_PeriodNo_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<Transaction>();

            transactionRequest.PeriodNo = It.IsAny<short>();

            var result = transactionRequest.HaveAllFieldsInBatchProcessingModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransaction_TransactionType_ShouldBeFalse()
        {
            var transactionRequest = _fixture.Create<Transaction>();

            transactionRequest.TransactionType = (TransactionType) (new Random()).Next(200, 500);

            var result = transactionRequest.HaveAllFieldsInBatchProcessingModel();

            result.Should().BeFalse();
        }

        [Fact]
        public void ValidationUpdateTransaction_Transaction_ShouldNotBeNull()
        {
            var transactionRequest = _fixture.Create<Transaction>();

            var result = transactionRequest.HaveAllFieldsInBatchProcessingModel();

            result.Should().BeTrue();
        }
    }
}
