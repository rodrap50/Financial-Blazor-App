using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Financial.Shared;

namespace Financial.Api.Infrastructure;

public interface IEventService
{
    Task<bool> EventExistsAsync(string id);
    Task<List<FinancialEvent>> GetAllEventsAsync();
    Task<FinancialEvent> GetEventByIdAsync(Guid id);
    Task<FinancialEvent> CreateEventAsync(FinancialEvent newEvent);
    Task<FinancialEvent> UpdateEventAsync(Guid id, FinancialEvent updatedEvent);
    Task DeleteEventAsync(Guid id);
}
