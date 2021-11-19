using CsvHelper;
using CsvHelper.Configuration;
using FinancialTransactionsApi.V1.Boundary.Response;
using System.IO;
using System.Linq;
using System.Text;
using WkHtmlToPdfDotNet;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class FileGenerator
    {
        public static byte[] WritePdfFile(ExportResponse transactions, string name)
        {
            var test = transactions;
            //byte[] result;
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
                HtmlContent = TemplateGenerator.GetHTMLReportString(transactions),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css")},
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"{name} Statement Report" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };


#pragma warning disable CA2000 // Dispose objects before losing scope
            var converter = new SynchronizedConverter(new PdfTools());
#pragma warning restore CA2000 // Dispose objects before losing scope

            var result = converter.Convert(pdf);
            return result;
        }
        public static byte[] WriteCSVFile(ExportResponse report)
        {
            var export = report.Data.Select(item =>
             new
             {
                 Date = item.TransactionDate,
                 RentAccountNo = item.PaymentReference,
                 Type = item.TransactionType,
                 Charge = item.ChargedAmount,
                 Paid = item.PaidAmount,
                 HBCount = item.HousingBenefitAmount,
                 Balance = item.BalanceAmount
             });
            byte[] result;
            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                {
                    using var cw = new CsvWriter(sw, cc);
                    cw.WriteRecords(export);
                }
                result = ms.ToArray();
            }
            return result;
        }

    }
}
