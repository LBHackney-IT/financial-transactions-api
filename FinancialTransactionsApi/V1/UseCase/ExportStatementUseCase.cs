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
            report.TotalCharge = response.Sum(x => x.ChargedAmount);
            report.TotalPaid = response.Sum(x => x.PaidAmount);
            report.TotalHousingBenefit = response.Sum(x => x.HousingBenefitAmount);
            report.BankAccountNumber = string.Join(",", response.Select(x => x.BankAccountNumber).Distinct().ToArray());
            report.FullName = response.FirstOrDefault()?.Person?.FullName;
            report.StatementPeriod = $"{response.FirstOrDefault()?.TransactionDate:D}-{response.LastOrDefault()?.TransactionDate:D}";
            foreach (var item in response)
            {

                data.Add(
                   new ExportTransactionResponse
                   {
                       BalanceAmount = NodaMoney.Money.PoundSterling(item.BalanceAmount).ToString(),
                       ChargedAmount = NodaMoney.Money.PoundSterling(item.ChargedAmount).ToString(),
                       HousingBenefitAmount = NodaMoney.Money.PoundSterling(item.HousingBenefitAmount).ToString(),
                       PaidAmount = NodaMoney.Money.PoundSterling(item.PaidAmount).ToString(),
                       PaymentReference = item.PaymentReference,
                       TransactionType = item.TransactionType.ToString(),
                       TransactionDate = item.TransactionDate.ToString("dd MMM yyyy")
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
