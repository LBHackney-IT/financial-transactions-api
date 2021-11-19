using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
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

        public ExportStatementUseCase(ITransactionGateway gateway, IConverter converter)
        {
            _gateway = gateway;
            _converter = converter;
        }

        public async Task<byte[]> ExecuteAsync(ExportTransactionQuery query)
        {
            string name;
            string period;
            if (query.StatementType == StatementType.Quaterly)
            {
                name = StatementType.Quaterly.ToString();
                period = $"{DateTime.UtcNow.AddMonths(-3).AddDays(-1):D} to {DateTime.UtcNow:D}";
            }
            else
            {
                name = StatementType.Yearly.ToString();
                period = $"{DateTime.UtcNow.AddMonths(-12):D} to {DateTime.UtcNow:D}";
            }

            var response = await _gateway.GetAllTransactionRecordAsync(query).ConfigureAwait(false);


            var result = query?.FileType switch
            {
                "csv" => FileGenerator.WriteCSVFile(response, name, period),
                "pdf" => FileGenerator.WritePdfFile(response, name, period, _converter),
                _ => null
            };
            return result;
        }
    }
}
