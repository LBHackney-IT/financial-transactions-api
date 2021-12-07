using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Gateways;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

            List<Transaction> response = new List<Transaction>();
            if (request.SelectedItems.Count > 0)
            {
                foreach (var item in request.SelectedItems)
                {
                    var rId = await _gateway.GetTransactionByIdAsync(request.TargetId, item).ConfigureAwait(false);
                    if (rId != null)
                        response.Add(rId);
                };
            }
            else
            {

                response = await _gateway.GetTransactionsAsync(request.TargetId, request.TransactionType.ToString(), request.StartDate, request.EndDate).ConfigureAwait(false);
            }

            if (response.Any())
            {
                var result = FileGenerator.WriteManualCSVFile(response);
                return result;
            };
            return null;
        }
    }
}
