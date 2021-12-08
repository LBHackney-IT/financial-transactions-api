using FinancialTransactionsApi.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Helpers
{
    public interface IFileGeneratorService
    {
        Task<string> CreatePdfTemplate(List<Transaction> transactions, string period);
    }
}
