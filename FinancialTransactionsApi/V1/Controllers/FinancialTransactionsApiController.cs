using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Domain;
using FinancialTransactionsApi.V1.Factories;
using FinancialTransactionsApi.V1.Helpers;
using FinancialTransactionsApi.V1.Infrastructure;
using FinancialTransactionsApi.V1.UseCase.Interfaces;
using Hackney.Core.DynamoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly IUpdateSuspenseAccountUseCase _updateUseCase;
        private readonly IAddBatchUseCase _addBatchUseCase;
        private readonly IGetSuspenseAccountUseCase _suspenseAccountUseCase;
        private readonly IGetByTargetIdUseCase _getByTargetIdUseCase;
        private readonly IGetAllActiveTransactionsUseCase _getAllActiveTransactionsUseCase;

        public FinancialTransactionsApiController(
            IGetAllUseCase getAllUseCase,
            IGetByIdUseCase getByIdUseCase,
            IAddUseCase addUseCase,
            IUpdateSuspenseAccountUseCase updateUseCase,
            IAddBatchUseCase addBatchUseCase,
            IGetSuspenseAccountUseCase suspenseAccountUseCase,
            IGetByTargetIdUseCase getByTargetIdUseCase,
            IGetAllActiveTransactionsUseCase getAllActiveTransactionsUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getByIdUseCase = getByIdUseCase;
            _addUseCase = addUseCase;
            _updateUseCase = updateUseCase;
            _addBatchUseCase = addBatchUseCase;
            _suspenseAccountUseCase = suspenseAccountUseCase;
            _getByTargetIdUseCase = getByTargetIdUseCase;
            _getAllActiveTransactionsUseCase = getAllActiveTransactionsUseCase;
        }

        /// <summary>
        /// Get transaction by provided id
        /// </summary>
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
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {

            var transaction = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (transaction.IsEmpty)
            {
                return NotFound($"No transaction exists for this id: {id}");
            }

            return Ok(transaction.Value);
        }

        /// <summary>
        /// Get transaction by provided id
        /// </summary>
        /// <response code="200">Success. Transaction model was received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="targetType">The value by which we are looking for a transaction</param>
        /// <param name="targetId">The value by which we are looking for a transaction</param>
        /// <param name="startDate">The value by which we are looking for a transaction</param>
        /// <param name="endDate">The value by which we are looking for a transaction</param>
        /// <returns>List of transactions</returns>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{targetType}/{targetId}")]
        public async Task<IActionResult> GetByTargetId([FromRoute] string targetType, [FromRoute] Guid targetId, [FromQuery] DateTime? startDate = default(DateTime?), [FromQuery] DateTime? endDate = default(DateTime?))
        {
            ResponseWrapper<IEnumerable<TransactionResponse>> response = await _getByTargetIdUseCase.ExecuteAsync(targetType, targetId, startDate, endDate).ConfigureAwait(false);

            return (response.IsEmpty) ? NotFound(targetId) : Ok(response.Value);
        }

        /// <summary>
        /// Gets a collection of transactions for a tenancy/property
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="query">Model with parameters to get collection of transactions</param>
        /// <response code="200">Success. Transaction models were received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(PagedResult<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader(Name = "x-correlation-id")] string correlationId, [FromQuery] TransactionQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            var transactions = await _getAllUseCase.ExecuteAsync(query).ConfigureAwait(false);

            return Ok(transactions);
        }

        /// <summary>
        /// Returns ALL non-suspense transactions wthout any filters. Note, that this endpoint has bad performance and will be used only for system needs. For better performance use HousingSearchAPI
        /// </summary>
        [ProducesResponseType(typeof(PagedResult<TransactionLimitedModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [Route("active")]
        [HttpGet]
        public async Task<IActionResult> GetAllActiveTransactions([FromQuery] GetActiveTransactionsRequest request)
        {
            if (request == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Request model cannot be null!"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            var transactions = await _getAllActiveTransactionsUseCase.ExecuteAsync(request).ConfigureAwait(false);

            return Ok(transactions);
        }

        /// <summary>
        /// Gets a collection of suspense account transactions for a tenancy/property
        /// </summary>
        /// <param name="query">Model with parameters to get collection of transactions</param>
        /// <response code="200">Success. Transaction models were received successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(PagedResult<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [Route("suspense-account")]
        [HttpGet]
        public async Task<IActionResult> GetSuspenseAccount([FromQuery] SuspenseAccountQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            var transactions = await _suspenseAccountUseCase.ExecuteAsync(query).ConfigureAwait(false);

            return Ok(transactions);
        }


        /// <summary>
        /// Create a new transaction model
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="token">The jwt token value</param>
        /// <param name="transaction">Transaction model for create</param>
        /// <response code="201">Created. Transaction model was created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader(Name = "x-correlation-id")] string correlationId,
                                             [FromHeader(Name = "Authorization")] string token,
                                             [FromBody] AddTransactionRequest transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            if (!CheckAddTransactionRequest(transaction))
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model don't have all information in fields!"));
            }

            var createdBy = GetUserName(token);

            var domainTransaction = transaction.ToDomain();
            domainTransaction.CreatedBy = createdBy;
            var transactionResponse = await _addUseCase.ExecuteAsync(domainTransaction).ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), new { id = transactionResponse.Id }, transactionResponse);
        }

        /// <summary>
        /// Create a list of new transaction records
        /// </summary>
        /// <param name="correlationId">The value that is used to combine several requests into a common group</param>
        /// <param name="token">The jwt token value</param>
        /// <param name="transactions">List of Transaction model for create</param>
        /// <response code="201">Created. Transaction model was created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("process-batch")]
        public async Task<IActionResult> AddBatch([FromHeader(Name = "x-correlation-id")] string correlationId,
                                                  [FromHeader(Name = "Authorization")] string token,
                                                  [FromBody] IEnumerable<AddTransactionRequest> transactions)
        {
            if (transactions == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction models cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            if (!CheckAddTransactionRequestCollection(transactions))
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model don't have all information in fields!"));
            }

            var createdBy = GetUserName(token);

            var domainTransactions = transactions.ToDomain().ToList();
            foreach (var domainTransaction in domainTransactions)
            {
                domainTransaction.CreatedBy = createdBy;
            }
            var transactionResponse = await _addBatchUseCase.ExecuteAsync(domainTransactions).ConfigureAwait(false);

            if (transactionResponse == transactions.Count())
                return Ok($"Total {transactionResponse} number of Transactions processed successfully");

            return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction entries processing failed!"));
        }

        /// <summary>
        /// Update a transaction model
        /// </summary>
        /// /// <param name="token">The jwt token value</param>
        /// <param name="transactionId">The value by which we are looking for a transaction</param>
        /// <param name="transaction">Transaction model for update</param>
        /// <response code="200">Success. Transaction model was updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        [Route("suspense-account-confirmation/{transactionId}")]
        public async Task<IActionResult> SuspenseAccountConfirmation([FromHeader(Name = "Authorization")] string token,
                                                [FromRoute] Guid transactionId,
                                                [FromBody] SuspenseConfirmationRequest transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "SuspenseConfirmationRequest model cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }
            if (transaction.TargetId == Guid.Empty)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "SuspenseConfirmationRequest model don't have all information in fields!"));
            }

            var existTransaction = await _getByIdUseCase.ExecuteAsync(transactionId).ConfigureAwait(false);

            if (existTransaction == null)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, "No transaction by provided Id cannot be found!"));
            }

            if (!existTransaction.IsEmpty && !existTransaction.Value.IsSuspense)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Cannot update model with full information!"));
            }

            var lastUpdatedBy = GetUserName(token);

            var domainTransaction = existTransaction.Value.ResponseToDomain(transaction, lastUpdatedBy);


            var transactionResponse = await _updateUseCase.ExecuteAsync(null).ConfigureAwait(false);

            return Ok(transactionResponse);
        }

        /// <summary>
        /// Update a transaction model
        /// </summary>

        /// /// <param name="token">The jwt token value</param>
        /// <param name="transactionId">The value by which we are looking for a transaction</param>
        /// <param name="transaction">Transaction model for update</param>
        /// <response code="200">Success. Transaction model was updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Transaction by provided id cannot be found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        [Route("suspense-account-approval/{transactionId}")]
        public async Task<IActionResult> SuspenseAccountApproval(
                                                [FromHeader(Name = "Authorization")] string token,
                                                [FromRoute] Guid transactionId,
                                                [FromBody] UpdateTransactionRequest transaction)
        {
            if (transaction == null)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model cannot be null!"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, ModelState.GetErrorMessages()));
            }

            if (!CheckUpdateTransactionRequest(transaction))
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Transaction model don't have all information in fields!"));
            }
            var existTransaction = await _getByIdUseCase.ExecuteAsync(transactionId).ConfigureAwait(false);

            if (existTransaction == null)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, "No transaction by provided Id cannot be found!"));
            }

            if (!existTransaction.Value.IsSuspense)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest, "Cannot update model with full information!"));
            }

            var lastUpdatedBy = GetUserName(token);

            var domainTransaction = transaction.ToDomain();
            domainTransaction.LastUpdatedBy = lastUpdatedBy;

            var transactionResponse = await _updateUseCase.ExecuteAsync(domainTransaction).ConfigureAwait(false);

            return Ok(transactionResponse);
        }

        /// <summary>
        ///   Returns the name of the user used in token
        /// </summary>
        /// <exception cref="ArgumentNullException">Parameter 'token' is null.</exception>
        /// <exception cref="ArgumentException">Value of parameter 'name' in token is null or empty.</exception>
        private static string GetUserName(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            token = token.Replace("Bearer", string.Empty).Trim();

            var claimTypeName = "name";
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var name = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == claimTypeName)?.Value;
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Token doesn't contain a value for 'name'");
            }
            return name;
        }

        /// <summary>
        ///  Checks if transaction model is valid for add operation
        /// </summary>
        private static bool CheckAddTransactionRequest(AddTransactionRequest transaction)
        {
            return transaction.IsSuspense || transaction.HaveAllFieldsInAddTransactionModel();
        }

        /// <summary>
        ///  Checks if transaction model collection is valid for add-batch operation
        /// </summary>
        private static bool CheckAddTransactionRequestCollection(IEnumerable<AddTransactionRequest> transactions)
        {
            return transactions.All(t => t.IsSuspense || (!t.IsSuspense && t.ToDomain().HaveAllFieldsInBatchProcessingModel()));
        }

        /// <summary>
        ///  Checks if transaction model collection is valid for update operation
        /// </summary>
        private static bool CheckUpdateTransactionRequest(UpdateTransactionRequest transaction)
        {
            return transaction.IsSuspense || transaction.HaveAllFieldsInUpdateTransactionModel();
        }
    }
}
