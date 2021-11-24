using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System;
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
            DateTime startDate;
            DateTime endDate;
            if (query.StatementType == StatementType.Quaterly)
            {
                name = StatementType.Quaterly.ToString();
                startDate = DateTime.UtcNow;
                endDate = startDate.AddMonths(-3);
                period = $"{endDate:D} to {startDate:D}";
            }
            else
            {
                startDate = DateTime.UtcNow;
                endDate = startDate.AddMonths(-12);
                name = StatementType.Yearly.ToString();
                period = $"{endDate:D} to {startDate:D}";
            }

            var response = await _gateway.GetTransactionsAsync(query.TargetId, query.TransactionType.ToString(), startDate, endDate).ConfigureAwait(false);


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
