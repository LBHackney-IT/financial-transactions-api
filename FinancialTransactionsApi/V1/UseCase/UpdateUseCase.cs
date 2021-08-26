using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class UpdateUseCase : IUpdateUseCase
    {
        private readonly ITransactionGateway _gateway;

        public UpdateUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponse> ExecuteAsync(UpdateTransactionRequest transaction, Guid id)
        {
            if (!transaction.IsSuspense)
            {
                var result = transaction.HaveAllFieldsInUpdateTransactionModel();
                if (!result)
                {
                    throw new ArgumentException("Transaction model dont have all information in fields!");
                }
            }

            var transactionDomain = transaction.ToDomain();

            transactionDomain.FinancialMonth = (short) transaction.TransactionDate.Month;

            transactionDomain.FinancialYear = (short) transaction.TransactionDate.Year;

            transactionDomain.Id = id;

            await _gateway.UpdateAsync(transactionDomain).ConfigureAwait(false);

            return transactionDomain.ToResponse();
        }
    }
}
