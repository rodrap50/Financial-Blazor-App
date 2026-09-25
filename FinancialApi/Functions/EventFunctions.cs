namespace Financial.Api.Functions;

using System;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Shared;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

public class EventFunctions(IEventService eventService) : ControllerBase
{
    [Function("CreateEvent")]
    public async Task<IActionResult> CreateEvent(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "events")]
        HttpRequest req)
    {
        var newEvent = await req.ReadFromJsonAsync<FinancialEvent>();
        if (newEvent is null)
            return BadRequest("Request body must contain a correct event");
        try
        {
            var createdEvent = await eventService.CreateEventAsync(newEvent);
            return Created($"events/{createdEvent.Id}", createdEvent);
        }
        catch (ConflictException conflictException)
        {
            return Conflict(conflictException.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("GetAllEvents")]
    public async Task<IActionResult> GetAllEvents(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "events")]
        HttpRequest req)
    {
        try
        {
            return Ok(await eventService.GetAllEventsAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Function("GetEventById")]
    public async Task<IActionResult> GetEventById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "events/{eventId}")]
        HttpRequest req,
        string eventId)
    {
        try
        {
            var valid = Guid.TryParse(eventId, out var eventGuid);
            if(!valid)
                return BadRequest("Invalid event id");

            var foundEvent = await eventService.GetEventByIdAsync(eventGuid);
            return Ok(foundEvent);
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

    [Function("UpdateEvent")]
    public async Task<IActionResult> UpdateEvent(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "events/{eventId}")]
        HttpRequest req,
        string eventId)
    {
        try
        {
            var valid = Guid.TryParse(eventId, out var eventGuid);
            if(!valid)
                return BadRequest("Invalid event id");
            var updatedEvent = await req.ReadFromJsonAsync<FinancialEvent>();
            if (updatedEvent is null)
                return BadRequest("Request body must contain a correct event");

            await eventService.UpdateEventAsync(eventGuid, updatedEvent);
            return Ok(updatedEvent);
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

    [Function("DeleteEvent")]
    public async Task<IActionResult> DeleteEvent(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "events/{eventId}")]
        HttpRequest req,
        string eventId)
    {
        try
        {
            var valid = Guid.TryParse(eventId, out var eventGuid);
            if(!valid)
                return BadRequest("Invalid event id");


            await eventService.DeleteEventAsync(eventGuid);

            return Ok(new {Deleted = true});
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
