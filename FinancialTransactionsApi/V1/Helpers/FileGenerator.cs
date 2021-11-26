using CsvHelper;
using CsvHelper.Configuration;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using NodaMoney;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class FileGenerator
    {
        public static byte[] WritePdfFile(List<Transaction> transactions, string name, string period, IConverter converter)
        {

            var report = new ExportResponse();
            var data = new List<ExportTransactionResponse>();

            report.FullName = transactions.FirstOrDefault()?.Person?.FullName;
            report.Balance = Money.PoundSterling(transactions.LastOrDefault().BalanceAmount).ToString();
            report.BalanceBroughtForward = Money.PoundSterling(transactions.FirstOrDefault().BalanceAmount).ToString();
            report.StatementPeriod = period;
            foreach (var item in transactions)
            {

                data.Add(
                   new ExportTransactionResponse
                   {
                       Date = item.TransactionDate.ToString("dd MMM yyyy"),
                       TransactionDetail = item.TransactionSource,
                       Debit = Money.PoundSterling(item.PaidAmount).ToString(),
                       Credit = Money.PoundSterling(item.HousingBenefitAmount).ToString(),
                       Balance = Money.PoundSterling(item.BalanceAmount).ToString()
                   });
            }
            report.Data = data;
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = $"{name} Statement Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLReportString(report),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css"), LoadImages = true },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"{name} Statement Report" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };


            var result = converter.Convert(pdf);
            return result;
        }
        public static byte[] WriteCSVFile(List<Transaction> transactions, List<string> lines)
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
            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using var cw = new CsvWriter(sw, cc);
                    cw.WriteRecords(data);
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
                RentAccountNumber = _.PaymentReference,
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
