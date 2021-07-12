using FinancialTransactionsApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllSummaryUseCase
    {
        public Task<TransactionSummaryResponseObjectList> ExecuteAsync(Guid targetId, string startDate, string endDate);
    }
}
