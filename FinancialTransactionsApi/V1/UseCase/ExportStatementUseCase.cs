using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class ExportStatementUseCase : IExportStatementUseCase
    {
        private readonly ITransactionGateway _gateway;

        public ExportStatementUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<byte[]> ExecuteAsync(ExportTransactionQuery query)
        {
            var report = new ExportResponse();
            var data = new List<ExportTransactionResponse>();
            var name = query.StatementType == StatementType.Quaterly ? StatementType.Quaterly.ToString() : StatementType.Quaterly.ToString();
            var response = await _gateway.GetAllTransactionRecordAsync(query).ConfigureAwait(false);
            //var groupedByYear = response.GroupBy(_ => new { _.TransactionDate.Year,_.TransactionType })
            //                        .OrderByDescending(x => x.Key.Year).ToList();
            //foreach (var item in groupedByYear)
            report.TotalCharge = response.Sum(x => x.ChargedAmount);
            report.TotalPaid = response.Sum(x => x.PaidAmount);
            report.TotalHousingBenefit = response.Sum(x => x.HousingBenefitAmount);
            report.BankAccountNumber = string.Join(",", response.Select(x => x.BankAccountNumber).Distinct().ToArray());
            report.FullName = response.FirstOrDefault()?.Person?.FullName;
            report.StatementPeriod = $"{response.FirstOrDefault().TransactionDate:D}-{response.LastOrDefault().TransactionDate:D}";
            foreach (var item in response)
            {

                data.Add(
                   new ExportTransactionResponse
                   {
                       BalanceAmount = item.BalanceAmount,
                       ChargedAmount = item.ChargedAmount,
                       HousingBenefitAmount = item.HousingBenefitAmount,
                       PaidAmount = item.PaidAmount,
                       PaymentReference = item.PaymentReference,
                       TransactionType = item.TransactionType.ToString(),
                       TransactionDate = item.TransactionDate.ToString("dd/MM/yyyy")
                   });
            }
            report.Data = data;

            var result = query?.FileType switch
            {
                "csv" => FileGenerator.WriteCSVFile(report),
                "pdf" => FileGenerator.WritePdfFile(report, name),
                _ => null
            };
            return result;
        }
    }
}
