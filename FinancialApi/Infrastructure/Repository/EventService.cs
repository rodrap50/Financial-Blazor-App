using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Financial.Api.Data;
using Financial.Api.Infrastructure.Exceptions;
using Financial.Shared;
using Microsoft.EntityFrameworkCore;

namespace Financial.Api.Infrastructure;

public class EventService (CosmosDbContext context) : IEventService
{
    public async Task<bool> EventExistsAsync(string id)
    {
        return await context
            .Events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id) is not null;
    }
    public async Task<List<FinancialEvent>> GetAllEventsAsync()
    {
        var events = await context.Events.AsNoTracking().ToListAsync();
        return events;

    }

    public async Task<FinancialEvent> GetEventByIdAsync(Guid id)
    {
        var events = await context
            .Events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.ToString());
        return events ?? throw new NotFoundException("Event not found");
    }

    public async Task<FinancialEvent> CreateEventAsync(FinancialEvent newEvent)
    {
        var exists = await EventExistsAsync(newEvent.Id);
        if (exists) throw new ConflictException("Event already exists");
        context.Events.Add(newEvent);
        await context.SaveChangesAsync();
        return newEvent;
    }

    public async Task<FinancialEvent> UpdateEventAsync(Guid id, FinancialEvent updatedEvent)
    {
        var exists = await EventExistsAsync(id.ToString());
        if (!exists) throw new NotFoundException("Event not found");
        updatedEvent.Id = id.ToString();
        context.Events.Update(updatedEvent);
        await context.SaveChangesAsync();
        return updatedEvent;
    }

    public async Task DeleteEventAsync(Guid id)
    {
        var fEvent = await context.Events.FirstOrDefaultAsync(x => x.Id == id.ToString());
        if(fEvent is null) throw new NotFoundException("Event not found");


        context.Events.Remove(fEvent);
        await context.SaveChangesAsync();

    }
}
