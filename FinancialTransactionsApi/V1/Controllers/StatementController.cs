using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinancialTransactionsApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/statements")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class StatementController : BaseController
    {
        private readonly IExportPdfStatementUseCase _exportPdfStatementUseCase;
        private readonly IExportSelectedItemUseCase _exportSelectedItemUseCase;
        private readonly IExportCsvStatementUseCase _exportStatementUseCase;

        public StatementController(
                                   IExportPdfStatementUseCase exportPdfStatementUseCase,
                                   IExportCsvStatementUseCase exportStatementUseCase,
                                   IExportSelectedItemUseCase exportSelectedItemUseCase)
        {
            _exportPdfStatementUseCase = exportPdfStatementUseCase;
            _exportStatementUseCase = exportStatementUseCase;
            _exportSelectedItemUseCase = exportSelectedItemUseCase;

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("export")]
        public async Task<IActionResult> ExportStatementReportAsync([FromBody] ExportTransactionQuery query)
        {

            switch (query?.FileType)
            {
                case "pdf":
                    {
                        var pdfResult = await _exportPdfStatementUseCase.ExecuteAsync(query).ConfigureAwait(false);
                        if (pdfResult == null)
                            return NotFound($"No records found for the following ID: {query.TargetId}");
                        return Ok(pdfResult);
                    }

                case "csv":
                    {

                        var csvResult = await _exportStatementUseCase.ExecuteAsync(query).ConfigureAwait(false);
                        if (csvResult == null)
                            return NotFound($"No records found for the following ID: {query.TargetId}");

                        return File(csvResult, "text/csv", $"{query.StatementType}_{DateTime.UtcNow.Ticks}.{query.FileType}");
                    }

                default:
                    return BadRequest("Format not supported");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("selection/export")]
        public async Task<IActionResult> ExportSelectedItemAsync([FromBody] TransactionExportRequest request)
        {
            if (!request.HaveDateRangeOrSelectedItemsModel())
            {
                return BadRequest(nameof(TransactionExportRequest));
            }
            var result = await _exportSelectedItemUseCase.ExecuteAsync(request).ConfigureAwait(false);
            if (result == null)
                return NotFound($"No records found for the following ID: {request.TargetId}");
            return File(result, "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }
    }
}
