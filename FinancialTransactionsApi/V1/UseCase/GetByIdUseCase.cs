using System;
using System.Threading.Tasks;
using TransactionsApi.V1.Boundary.Response;
using TransactionsApi.V1.Factories;
using TransactionsApi.V1.Gateways;
using TransactionsApi.V1.UseCase.Interfaces;

namespace TransactionsApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetClaimantByIdUseCase
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private ITransactionGateway _gateway;
        public GetByIdUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<TransactionResponseObject> ExecuteAsync(Guid id)
        {
            var data =await _gateway.GetTransactionByIdAsync(id).ConfigureAwait(false);
            return data?.ToResponse();
        }
    }
}
