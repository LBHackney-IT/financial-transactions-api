using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using NodaMoney;
using Razor.Templating.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Helpers
{
    public class FileGeneratorService : IFileGeneratorService
    {

        public async Task<string> CreatePdfTemplate(List<Transaction> transactions, string period)
        {

            var model = new ExportResponse();
            var data = new List<ExportTransactionResponse>();

            decimal balance = (transactions?.LastOrDefault() == null) ? default(decimal) : transactions.LastOrDefault().BalanceAmount;
            decimal balanceBroughtForward = (transactions?.FirstOrDefault() == null) ? default(decimal) : transactions.FirstOrDefault().BalanceAmount;
            model.Balance = Money.PoundSterling(balance).ToString();
            model.BalanceBroughtForward = Money.PoundSterling(balanceBroughtForward).ToString();
            model.StatementPeriod = period;
            foreach (var item in transactions)
            {

                data.Add(
                   new ExportTransactionResponse
                   {
                       Date = item.TransactionDate.ToString("dd MMM yyyy"),
                       TransactionDetail = item.TargetType.ToString(),
                       Debit = Money.PoundSterling(item.PaidAmount).ToString(),
                       Credit = Money.PoundSterling(item.HousingBenefitAmount).ToString(),
                       Balance = Money.PoundSterling(item.BalanceAmount).ToString()
                   });
            }
            model.Data = data;
            string template = await RazorTemplateEngine.RenderAsync("~/V1/Templates/PDFTemplate.cshtml", model).ConfigureAwait(false);


            return template.EncodeBase64();
        }
    }
}
