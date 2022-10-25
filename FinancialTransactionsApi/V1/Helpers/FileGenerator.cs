using CsvHelper;
using CsvHelper.Configuration;
using FinancialTransactionsApi.V1.Boundary;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Infrastructure;
using GenFu;
using NodaMoney;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class FileGenerator
    {
        public static async Task<string> CreatePdfTemplate(IEnumerable<Transaction> transactions, string period, List<string> lines)
        {

            var model = new ExportResponse();
            var data = new List<ExportTransactionResponse>();
            model.Header = lines[0];
            model.SubFooter = lines[1];
            model.SubFooter = lines[2];
            model.Footer = lines[3];

            if (transactions == null) throw new ArgumentNullException(nameof(transactions));
            else
            {
                model.Balance = Money.PoundSterling(transactions.LastOrDefault().BalanceAmount).ToString();
                model.BalanceBroughtForward = Money.PoundSterling(transactions.FirstOrDefault().BalanceAmount).ToString();
            }
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
        public static byte[] WriteCSVFile(IEnumerable<Transaction> transactions, List<string> lines)
        {
            var data = new List<ExportTransactionResponse>();
            foreach (var item in transactions)
            {

                data.Add(
                   new ExportTransactionResponse
                   {
                       Date = item.TransactionDate.ToString("dd MMM yyyy"),
                       TransactionDetail = item.TransactionSource,
                       Debit = item.PaidAmount.ToString(),
                       Credit = item.HousingBenefitAmount.ToString(),
                       Balance = item.BalanceAmount.ToString()
                   });
            }

            byte[] result;
            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-UK"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using var cw = new CsvWriter(sw, cc);
                    cw.WriteRecords(data);
                    cc.NewLine = Environment.NewLine;
                    foreach (var item in lines)
                    {
                        cw.WriteComment(item);
                    }
                }
                result = ms.ToArray();
            }
            return result;
        }

        public static byte[] WriteManualCSVFile(IEnumerable<Transaction> transactions)
        {
            var data = transactions.Select(_ => new
            {
                Date = _.TransactionDate.ToString("dd/MM/yyyy"),
                Type = _.TransactionSource,
                Charge = _.ChargedAmount,
                Paid = _.PaidAmount,
                HBCont = _.HousingBenefitAmount,
                Balence = _.BalanceAmount
            });

            byte[] result;
            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using var cw = new CsvWriter(sw, cc);
                    cw.WriteRecords(data);
                }
                result = ms.ToArray();
            }
            return result;
        }
    }
}
