using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Hackney.Core.Sns;

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

        public async Task<TransactionResponse> ExecuteAsync(AddTransactionRequest transaction)
        {
            if (!transaction.IsSuspense)
            {
                var result = transaction.HaveAllFieldsInAddTransactionModel();
                if (!result)
                {
                    throw new ArgumentException("Transaction model don't have all information in fields!");
                }
            }

            var transactionDomain = transaction.ToDomain();

            transactionDomain.FinancialMonth = (short) transaction.TransactionDate.Month;

            transactionDomain.FinancialYear = (short) transaction.TransactionDate.Year;

            transactionDomain.Id = Guid.NewGuid();

            await _gateway.AddAsync(transactionDomain).ConfigureAwait(false);
            var transactionSnsMessage = _snsFactory.Create(transactionDomain);
            var transactionTopicArn = Environment.GetEnvironmentVariable("TRANSACTION_SNS_ARN");
            await _snsGateway.Publish(transactionSnsMessage, transactionTopicArn).ConfigureAwait(false);
            return transactionDomain.ToResponse();
        }
    }
}
