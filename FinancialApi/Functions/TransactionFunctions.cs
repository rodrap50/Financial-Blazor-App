namespace Financial.Api.Functions;

using System;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Shared;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

public class TransactionFunctions(ITransactionService transactionService) : ControllerBase
{
    [Function("CreateTransaction")]
    public async Task<IActionResult> CreateTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "transactions")]
        HttpRequest req)
    {
        var newTransaction = await req.ReadFromJsonAsync<Transaction>();
        if (newTransaction is null)
            return BadRequest("Request body must contain a correct transaction");
        try
        {
            var created = await transactionService.CreateTransactionAsync(newTransaction);
            return Created($"api/transactions/{created.Id}", created);
        }
        catch (ConflictException cex)
        {
            return Conflict(cex.Message);
        }
        catch (NotFoundException nfex)
        {
            return NotFound(nfex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("GetTransactionById")]
    public async Task<IActionResult> GetTransactionById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions/{transactionId}")]
        HttpRequest req,
        string transactionId)
    {
        if (!Guid.TryParse(transactionId, out var id))
            return BadRequest("Invalid transaction id");
        try
        {
            return Ok(await transactionService.GetTransactionByIdAsync(id));
        }
        catch (NotFoundException nfex)
        {
            return NotFound(nfex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("GetTransactionsByAccount")]
    public async Task<IActionResult> GetTransactionsByAccount(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions/account/{accountId}")]
        HttpRequest req,
        string accountId)
    {
        if (!Guid.TryParse(accountId, out var id))
            return BadRequest("Invalid account id");
        try
        {
            return Ok(await transactionService.GetTransactionsByAccountAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("GetTransactionsByEvent")]
    public async Task<IActionResult> GetTransactionsByEvent(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions/event/{eventId}")]
        HttpRequest req,
        string eventId)
    {
        if (!Guid.TryParse(eventId, out var id))
            return BadRequest("Invalid event id");
        try
        {
            return Ok(await transactionService.GetTransactionsByEventAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("UpdateTransaction")]
    public async Task<IActionResult> UpdateTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "transactions/{transactionId}")]
        HttpRequest req,
        string transactionId)
    {
        if (!Guid.TryParse(transactionId, out var id))
            return BadRequest("Invalid transaction id");
        var updated = await req.ReadFromJsonAsync<Transaction>();
        if (updated is null)
            return BadRequest("Request body must contain a correct transaction");
        try
        {
            return Ok(await transactionService.UpdateTransactionAsync(id, updated));
        }
        catch (NotFoundException nfex)
        {
            return NotFound(nfex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("DeleteTransaction")]
    public async Task<IActionResult> DeleteTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "transactions/{transactionId}")]
        HttpRequest req,
        string transactionId)
    {
        if (!Guid.TryParse(transactionId, out var id))
            return BadRequest("Invalid transaction id");
        try
        {
            await transactionService.DeleteTransactionAsync(id);
            return Ok(new { Deleted = true });
        }
        catch (NotFoundException nfex)
        {
            return NotFound(nfex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
