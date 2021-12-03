using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class GetAllSegregratedByPersonUseCase : IGetAllSegregratedByPersonUseCase
    {
        private readonly ITransactionGateway _gateway;

        public GetAllSegregratedByPersonUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<List<WeeklyReportResponse>> ExecuteAsync()
        {
            return await _gateway.GetAllSegregratedByPersonAsync().ConfigureAwait(false);
        }
    }
}
