using FinancialTransactionsApi.V1.Boundary.Request;
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


            var result = FileGenerator.WriteManualCSVFile(response);
            return result;
        }
    }
}
