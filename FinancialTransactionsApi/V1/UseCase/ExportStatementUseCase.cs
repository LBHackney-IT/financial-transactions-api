using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Infrastructure.Settings;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet.Contracts;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class ExportStatementUseCase : IExportStatementUseCase
    {
        private readonly ITransactionGateway _gateway;
        private readonly IConverter _converter;
        private readonly ReportExportSettings _reportExportSettings;

        public ExportStatementUseCase(ITransactionGateway gateway, IConverter converter, IOptions<ReportExportSettings> reportExportSettings)
        {
            _gateway = gateway;
            _converter = converter;
            _reportExportSettings = reportExportSettings.Value;
        }

        public async Task<byte[]> ExecuteAsync(ExportTransactionQuery query)
        {
            string name;
            string period;
            DateTime startDate;
            DateTime endDate;
            var lines = new List<string>();
            if (query.StatementType == StatementType.Quaterly)
            {

                name = StatementType.Quaterly.ToString();
                startDate = DateTime.UtcNow;
                endDate = startDate.AddMonths(-3);
                period = $"{endDate:D} to {startDate:D}";
                lines.Add(_reportExportSettings.Header.Replace("{itemId}", name));
                lines.Add(_reportExportSettings.SubHeader.Replace("{itemId}", period));
            }
            else
            {
                startDate = DateTime.UtcNow;
                endDate = startDate.AddMonths(-12);
                name = StatementType.Yearly.ToString();
                period = $"{endDate:D} to {startDate:D}";
                lines.Add(_reportExportSettings.Header.Replace("{itemId}", name));
                lines.Add(_reportExportSettings.SubHeader.Replace("{itemId}", period));
            }

            var response = await _gateway.GetTransactionsAsync(query.TargetId, query.TransactionType.ToString(), startDate, endDate).ConfigureAwait(false);
            if (response.Any())
            {
                var accountBalance = $"{ response.LastOrDefault().BalanceAmount}";
                var date = $"{DateTime.Today:D}";
                lines.Add(_reportExportSettings.SubFooter.Replace("{itemId}", date).Replace("{itemId_1}", accountBalance));
                lines.Add(_reportExportSettings.Footer);
                var result = query?.FileType switch
                {
                    "csv" => FileGenerator.WriteCSVFile(response, lines),
                    "pdf" => FileGenerator.WritePdfFile(response, name, period, _converter),
                    _ => null
                };
                return result;
            }

            return null;
        }
    }
}
