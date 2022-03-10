using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class SuspenseAccountApprovalUseCase : ISuspenseAccountApprovalUseCase
    {
        private readonly ITransactionGateway _gateway;
        public SuspenseAccountApprovalUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<bool> ExecuteAsync(Transaction transaction)
        {
            transaction.LastUpdatedAt = DateTime.UtcNow;

            await _gateway.ApproveSuspenseAccountTransactionAsync(transaction).ConfigureAwait(false);


            return true;
        }
    }
}
