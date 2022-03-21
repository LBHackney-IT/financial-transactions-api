using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.Sns;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class UpdateSuspenseAccountUseCase : IUpdateSuspenseAccountUseCase
    {
        private readonly ITransactionGateway _gateway;
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;
        public UpdateSuspenseAccountUseCase(ITransactionGateway gateway, ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _snsGateway = snsGateway;
            _snsFactory = snsFactory;
        }

        public async Task<TransactionResponse> ExecuteAsync(Transaction transaction)
        {
            transaction.LastUpdatedAt = DateTime.UtcNow;

            await _gateway.UpdateSuspenseAccountAsync(transaction).ConfigureAwait(false);
            //var transactionSnsMessage = _snsFactory.Create(transaction);
            //var transactionTopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN");
            //await _snsGateway.Publish(transactionSnsMessage, transactionTopicArn, EventConstants.MESSAGEGROUPID).ConfigureAwait(false);

            return transaction.ToResponse();
        }
    }
}
