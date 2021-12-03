using FinancialTransactionsApi.V1.Boundary.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IGetAllSegregratedByPersonUseCase
    {
        public Task<List<WeeklyReportResponse>> ExecuteAsync();
    }
}
