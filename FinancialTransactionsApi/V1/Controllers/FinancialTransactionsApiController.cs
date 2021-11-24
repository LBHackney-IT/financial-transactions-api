using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace FinancialTransactionsApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/transactions")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class FinancialTransactionsApiController : BaseController
    {
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IGetByIdUseCase _getByIdUseCase;
        private readonly IAddUseCase _addUseCase;
        private readonly IUpdateUseCase _updateUseCase;
        private readonly IAddBatchUseCase _addBatchUseCase;
        private readonly IGetTransactionListUseCase _getTransactionListUseCase;
        private readonly IExportSelectedItemUseCase _exportSelectedItemUseCase;
        private readonly IExportStatementUseCase _exportStatementUseCase;
        public FinancialTransactionsApiController(
            IGetAllUseCase getAllUseCase,
            IGetByIdUseCase getByIdUseCase,
            IAddUseCase addUseCase,
            IUpdateUseCase updateUseCase,
            IAddBatchUseCase addBatchUseCase,
            IGetTransactionListUseCase getTransactionListUseCase,
            IExportSelectedItemUseCase exportSelectedItemUseCase,
            IExportStatementUseCase exportStatementUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _addUseCase = addUseCase;
            _updateUseCase = updateUseCase;
            _addBatchUseCase = addBatchUseCase;
            _getTransactionListUseCase = getTransactionListUseCase;
            _exportSelectedItemUseCase = exportSelectedItemUseCase;
            _exportStatementUseCase = exportStatementUseCase;
        }

        /// <summary>
        /// Get transaction by provided id
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="id">The value by which we are looking for a transaction</param>
        ///<param name="targetId">The value by which we are looking for a transaction</param>
        /// <response code="200">Success. Transaction model was received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromHeader(Name = "x-correlation-id")] string correlationId, [FromRoute] Guid id, [FromQuery] Guid targetId)
        {
            var transaction = await _getByIdUseCase.ExecuteAsync(id, targetId).ConfigureAwait(false);

            if (transaction == null)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, "No transaction by provided Id cannot be found!"));
            }

            return Ok(transaction);
        }

        /// <summary>
        /// Gets a collection of transactions for a tenancy/property
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="query">Model with parameters to get collection of transactions</param>
        /// <response code="200">Success. Transaction models were received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader(Name = "x-correlation-id")] string correlationId, [FromQuery] TransactionQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }

            var transactions = await _getAllUseCase.ExecuteAsync(query).ConfigureAwait(false);

            return Ok(transactions);
        }
        [ProducesResponseType(typeof(GetTransactionListResponse), 200)]
        [ProducesResponseType(typeof(BaseErrorResponse), 400)]
        [HttpGet, MapToApiVersion("1")]
        [Route("search")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionList([FromQuery] TransactionSearchRequest request)
        {
            try
            {
                var transactionSearchResult = await _getTransactionListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetTransactionListResponse>(transactionSearchResult)
                { Total = transactionSearchResult.Total() };

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }

        /// <summary>
        /// Create a new transaction model
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="transaction">Transaction model for create</param>
        /// <response code="201">Created. Transaction model was created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader(Name = "x-correlation-id")] string correlationId, [FromBody] AddTransactionRequest transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }

            var transactionResponse = await _addUseCase.ExecuteAsync(transaction).ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), new { id = transactionResponse.Id }, transactionResponse);
        }

        /// <summary>
        /// Create a list of new transaction records
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="transactions">List of Transaction model for create</param>
        /// <response code="201">Created. Transaction model was created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("process-weekly-charge")]
        public async Task<IActionResult> AddBatch([FromHeader(Name = "x-correlation-id")] string correlationId, [FromBody] IEnumerable<AddTransactionRequest> transactions)
        {
            if (transactions == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction models cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }

            var transactionResponse = await _addBatchUseCase.ExecuteAsync(transactions).ConfigureAwait(false);

            if (transactionResponse == transactions.Count())
                return Ok($"Total {transactionResponse} number of Transactions processed successfully");

            return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction entries processing failed!"));
        }

        /// <summary>
        /// Update a transaction model
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="id">The value by which we are looking for a transaction</param>
        /// <param name="transaction">Transaction model for update</param>
        /// <response code="200">Success. Transaction model was updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromHeader(Name = "x-correlation-id")] string correlationId, [FromRoute] Guid id, [FromBody] UpdateTransactionRequest transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }
            var existTransaction = await _getByIdUseCase.ExecuteAsync(id, transaction.TargetId).ConfigureAwait(false);

            if (existTransaction == null)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, "No transaction by provided Id cannot be found!"));
            }

            if (!existTransaction.IsSuspense)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Cannot update model with full information!"));
            }

            var transactionResponse = await _updateUseCase.ExecuteAsync(transaction, id).ConfigureAwait(false);

            return Ok(transactionResponse);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("statement/export")]
        public async Task<IActionResult> ExportStatementReportAsync([FromBody] ExportTransactionQuery query)
        {
            var result = await _exportStatementUseCase.ExecuteAsync(query).ConfigureAwait(false);
            if (result == null)
                return NotFound("No record found");
            if (query?.FileType == "pdf")
            {
                return File(result, "application/pdf", $"{query.StatementType}_{DateTime.UtcNow.Ticks}.{query.FileType}");
            }
            return File(result, "text/csv", $"{query.StatementType}_{DateTime.UtcNow.Ticks}.{query.FileType}");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("selection/export")]
        public async Task<IActionResult> ExportSelectedItemAsync([FromBody] TransactionExportRequest request)
        {
            var result = await _exportSelectedItemUseCase.ExecuteAsync(request).ConfigureAwait(false);
            if (result == null)
                return NotFound("No record found");
            return File(result, "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }
    }
}
