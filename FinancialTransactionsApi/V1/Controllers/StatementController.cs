using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
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
        private readonly IGetByTargetIdsUseCase _getByTargetIdsUseCase;

        public StatementController(
                                   IExportPdfStatementUseCase exportPdfStatementUseCase,
                                   IExportCsvStatementUseCase exportStatementUseCase,
                                   IExportSelectedItemUseCase exportSelectedItemUseCase, IGetByTargetIdsUseCase getByTargetIdsUseCase)
        {
            _exportPdfStatementUseCase = exportPdfStatementUseCase;
            _exportStatementUseCase = exportStatementUseCase;
            _exportSelectedItemUseCase = exportSelectedItemUseCase;
            _getByTargetIdsUseCase = getByTargetIdsUseCase;
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



        /// <summary>
        /// Get transaction by provided id
        /// </summary>
        /// <response code="200">Success. Transaction model was received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="targetIdsQuery">The value by which we are looking for a transaction</param>
        /// <returns>List of transactions</returns>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetByTargetId([FromQuery] TransactionByTargetIdsQuery targetIdsQuery)
        {
            var transactions = await _getByTargetIdsUseCase.ExecuteAsync(targetIdsQuery).ConfigureAwait(false);

            return Ok(transactions);
        }
    }
}
