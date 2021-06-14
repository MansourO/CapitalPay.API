using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers.Enums;
using Shared.Models;
using Shared.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionAPI.Service.v1.Command;
using TransactionAPI.Service.v1.Query;
using TransactionAPI.Service.v1.Services;

namespace TransactionAPI.Controllers.v1
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionController(IMapper mapper, ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        /// <summary>
        ///     Action to create a new transaction in the database.
        /// </summary>
        /// <param name="transactionModel">Model to create a new transaction</param>
        /// <returns>Returns the created transaction</returns>
        /// <response code="200">Returned if the transaction was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(PostTransaction transactionModel)
        {
            try
            {
                return await _transactionService.CreateTransaction(transactionModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Action to get a transaction.
        /// </summary>
        /// <param name="id">The id of the transaction</param>
        /// <returns>Returns the paid order</returns>
        /// <response code="200">Returned if the transaction was updated</response>
        /// <response code="400">Returned if the order could not be found with the provided id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
        {
            try
            {
                return await _transactionService.GetTransactionById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Action to get transactions.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            try
            {
                var result = await _transactionService.GetTransactions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Action to update transactions.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult<Unit>> UpdateTransactions(IEnumerable<Transaction> transactions)
        {
            try
            {
                var result = await _transactionService.UpdateTransactions(transactions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Action to delete transactions.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult<Unit>> DeleteTransactions(IEnumerable<Guid> guids)
        {
            try
            {
                var result = await _transactionService.DeleteTransaction(guids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// get a list of transaction types
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactiontypes")]
        public ActionResult<IEnumerable<KeyValuePair<string, int>>> GetTransactionTypes()
        {
            var list = Enum.GetValues(typeof(TransactionTypes))
                .Cast<TransactionTypes>()
                .Select(e => new KeyValuePair<string, int>(e.ToString(), (int)e));

            return Ok(list);
        }
    }
}