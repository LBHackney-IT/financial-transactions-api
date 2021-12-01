using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Hackney.Core.Sns;
using FinancialTransactionsApi.V1.Domain;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class AddUseCase : IAddUseCase
    {
        private readonly ITransactionGateway _gateway;

        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public AddUseCase(ITransactionGateway gateway, ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _snsGateway = snsGateway;
            _snsFactory = snsFactory;
        }

        public async Task<TransactionResponse> ExecuteAsync(Transaction transaction)
        {
            DateTime currentDate = DateTime.UtcNow;

            transaction.FinancialMonth = (short) transaction.TransactionDate.Month;

            transaction.FinancialYear = (short) transaction.TransactionDate.Year;

            transaction.Id = Guid.NewGuid();

            transaction.CreatedAt = currentDate;
            transaction.LastUpdatedAt = currentDate;
            transaction.LastUpdatedBy = transaction.CreatedBy;

            await _gateway.AddAsync(transaction).ConfigureAwait(false);
            var transactionSnsMessage = _snsFactory.Create(transaction);
            var transactionTopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN");
            await _snsGateway.Publish(transactionSnsMessage, transactionTopicArn).ConfigureAwait(false);
            return transaction.ToResponse();
        }
    }
}
