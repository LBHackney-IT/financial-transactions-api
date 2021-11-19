using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.UseCase
{
    public class ExportSelectedItemUseCase : IExportSelectedItemUseCase
    {
        private readonly ITransactionGateway _gateway;

        public ExportSelectedItemUseCase(ITransactionGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<byte[]> ExecuteAsync(TransactionExportRequest request)
        {
            var report = new ExportResponse();
            var data = new List<ExportTransactionResponse>();
            List<Transaction> response = new List<Transaction>();
            if (request.SelectedItems.Count > 0)
            {
                foreach (var item in request.SelectedItems)
                {
                    var rId = await _gateway.GetTransactionByIdAsync(item).ConfigureAwait(false);
                    response.Add(rId);
                };
            }
            else
            {

                response = await _gateway.GetAllTransactionByDateAsync(request).ConfigureAwait(false);
            }


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

            var result = FileGenerator.WriteCSVFile(report);
            return result;
        }
    }
}
