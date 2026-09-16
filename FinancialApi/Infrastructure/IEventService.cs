using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Financial.Shared;

namespace Financial.Api.Infrastructure;

public interface IEventService
{
    Task<List<FinancialEvent>> GetEventAsync();
    Task<FinancialEvent> GetEventAsync(Guid id);
}