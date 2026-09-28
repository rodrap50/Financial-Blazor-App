using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Financial.Shared;

namespace Financial.Api.Infrastructure;

public interface ITransactionService
{
    Task<Transaction> CreateTransactionAsync(Transaction transaction);
    Task<Transaction> GetTransactionByIdAsync(Guid id);
    Task<List<Transaction>> GetTransactionsByAccountAsync(Guid accountId);
    Task<List<Transaction>> GetTransactionsByEventAsync(Guid eventId);
    Task<Transaction> UpdateTransactionAsync(Guid id, Transaction updatedTransaction);
    Task DeleteTransactionAsync(Guid id);
}
