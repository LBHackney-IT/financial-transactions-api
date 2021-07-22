using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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

        public FinancialTransactionsApiController(
            IGetAllUseCase getAllUseCase,
            IGetByIdUseCase getByIdUseCase,
            IAddUseCase addUseCase,
            IUpdateUseCase updateUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _addUseCase = addUseCase;
            _updateUseCase = updateUseCase;
        }

        /// <summary>
        /// Get transaction by provided id
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="id">The value by which we are looking for a transaction</param>
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
        public async Task<IActionResult> Get([FromHeader(Name = "x-correlation-id")] string correlationId, [FromRoute] Guid id)
        {
            var transaction = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);

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
            if(!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, GetErrorMessage(ModelState)));
            }

            var transactions = await _getAllUseCase.ExecuteAsync(query).ConfigureAwait(false);

            return Ok(transactions);
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

            var existTransaction = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);

            if (existTransaction == null)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, "No transaction by provided Id cannot be found!"));
            }

            if (!existTransaction.IsSuspense)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Cannot update model with full information!"));
            }

            var transactionResponse =  await _updateUseCase.ExecuteAsync(transaction, id).ConfigureAwait(false);

            return Ok(transactionResponse);
        }
    }
}
