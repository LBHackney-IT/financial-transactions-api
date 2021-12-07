using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class ExportPdfStatementUseCase : IExportPdfStatementUseCase
    {
        private readonly ITransactionGateway _gateway;
        private readonly IFileGeneratorService _fileGenerator;

        private readonly string _header;
        private readonly string _subHeader;
        private readonly string _footer;
        private readonly string _subFooter;

        private const string Header = "Header";
        private const string SubHeader = "SubHeader";
        private const string Footer = "Footer";
        private const string SubFooter = "SubFooter";

        public ExportPdfStatementUseCase(ITransactionGateway gateway, IFileGeneratorService fileGenerator, IConfiguration configuration)
        {
            _gateway = gateway;
            _fileGenerator = fileGenerator;
            _header = configuration.GetValue<string>(Header);
            if (string.IsNullOrEmpty(_header))
            {
                throw new ArgumentException($"Configuration does not contain a report setting value for the parameter {Header}.");
            }
            _subHeader = configuration.GetValue<string>(Header);
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

        public async Task<string> ExecuteAsync(ExportTransactionQuery query)
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
                lines.Add(_header.Replace("{itemId}", name));
                lines.Add(_subHeader.Replace("{itemId}", period));
            }
            else
            {
                startDate = DateTime.UtcNow;
                endDate = startDate.AddMonths(-12);
                name = StatementType.Yearly.ToString();
                period = $"{endDate:D} to {startDate:D}";
                lines.Add(_header.Replace("{itemId}", name));
                lines.Add(_subHeader.Replace("{itemId}", period));
            }

            var response = await _gateway.GetTransactionsAsync(query.TargetId, query.TransactionType.ToString(), startDate, endDate).ConfigureAwait(false);
            if (response.Any())
            {
                var accountBalance = $"{ response.LastOrDefault().BalanceAmount}";
                var date = $"{DateTime.Today:D}";
                lines.Add(_subFooter.Replace("{itemId}", date).Replace("{itemId_1}", accountBalance));
                lines.Add(_footer);
                var result = await FileGenerator.CreatePdfTemplate(response, period, lines).ConfigureAwait(false);
                return result;
            }

            return null;
        }
    }
}
