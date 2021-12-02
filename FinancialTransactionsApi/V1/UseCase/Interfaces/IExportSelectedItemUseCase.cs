using FinancialTransactionsApi.V1.Boundary.Request;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase.Interfaces
{
    public interface IExportSelectedItemUseCase
    {
        Task<byte[]> ExecuteAsync(TransactionExportRequest query);
    }
}
