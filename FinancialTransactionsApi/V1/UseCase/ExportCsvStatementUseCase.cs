using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class ExportCsvStatementUseCase : IExportCsvStatementUseCase
    {
        private readonly ITransactionGateway _gateway;
        private readonly string _header;
        private readonly string _subHeader;
        private readonly string _footer;
        private readonly string _subFooter;

        private const string Header = "Header";
        private const string SubHeader = "SubHeader";
        private const string Footer = "Footer";
        private const string SubFooter = "SubFooter";

        public ExportCsvStatementUseCase(ITransactionGateway gateway, IConfiguration configuration)
        {
            _gateway = gateway;
            _header = configuration.GetValue<string>(Header);
            if (string.IsNullOrEmpty(_header))
            {
                throw new ArgumentException($"Configuration does not contain a report setting value for the parameter {Header}.");
            }
            _subHeader = configuration.GetValue<string>(SubHeader);
            if (string.IsNullOrEmpty(_subHeader))
            {
                throw new ArgumentException($"Configuration does not contain a report setting value for the parameter {SubHeader}.");
            }
            _footer = configuration.GetValue<string>(Footer);
            if (string.IsNullOrEmpty(_footer))
            {
                throw new ArgumentException($"Configuration does not contain a report setting value for the parameter {Footer}.");
            }
            _subFooter = configuration.GetValue<string>(SubFooter);
            if (string.IsNullOrEmpty(_subFooter))
            {
                throw new ArgumentException($"Configuration does not contain a report setting value for the parameter {SubFooter}.");
            }
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
                endDate = DateTime.UtcNow;
                startDate = endDate.AddMonths(-3);
            }
            else
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddMonths(-12);
                name = StatementType.Yearly.ToString();
            }

            var response = await _gateway.GetTransactionsAsync(query.TargetId, query.TransactionType.ToString(), startDate, endDate).ConfigureAwait(false);
            if (response.Any())
            {
                var accountBalance = $"{ response.LastOrDefault().BalanceAmount}";
                var date = $"{DateTime.Today:D}";
                period = $"{startDate:D} to {endDate:D}";
                lines.Add(_header.Replace("{itemId}", name));
                lines.Add(_subHeader.Replace("{itemId}", period));
                lines.Add(_subFooter.Replace("{itemId}", date).Replace("{itemId_1}", accountBalance));
                lines.Add(_footer);
                return FileGenerator.WriteCSVFile(response, lines);
            }

            return null;
        }
    }
}
