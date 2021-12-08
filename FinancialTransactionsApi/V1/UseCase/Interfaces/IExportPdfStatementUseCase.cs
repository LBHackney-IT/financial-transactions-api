using FinancialTransactionsApi.V1.Boundary.Request;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IExportPdfStatementUseCase
    {
        Task<string> ExecuteAsync(ExportTransactionQuery query);
    }
}
