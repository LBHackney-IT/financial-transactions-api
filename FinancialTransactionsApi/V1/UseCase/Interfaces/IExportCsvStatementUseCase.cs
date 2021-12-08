using FinancialTransactionsApi.V1.Boundary.Request;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IExportCsvStatementUseCase
    {
        Task<byte[]> ExecuteAsync(ExportTransactionQuery query);
    }
}
