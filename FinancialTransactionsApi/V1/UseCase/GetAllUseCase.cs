using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Domain;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;
using TransactionsApi.V1.UseCase.Interfaces;

namespace TransactionsApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetAllClaimantsUseCase
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly ITransactionGateway _gateway;
        public GetAllUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponseObjectList> ExecuteAsync(Guid targetId, string transactionType, DateTime? startDate, DateTime? endDate)
        {
            TransactionResponseObjectList transactionResponseObjectList = new TransactionResponseObjectList();
            List<Transaction> transactions =
                await _gateway.GetAllTransactionsAsync(targetId, transactionType, startDate, endDate).ConfigureAwait(false);

            transactionResponseObjectList.ResponseObjects =
                transactions.Select(p => p.ToResponse()).ToList();
            return transactionResponseObjectList;
        }
    }
}
