using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financial.Api.Data;
using Financial.Api.Infrastructure.Exceptions;
using Financial.Shared;
using Microsoft.EntityFrameworkCore;

namespace Financial.Api.Infrastructure;

// ponytail: account balances and the transaction are saved in one SaveChanges, but EF Cosmos is not
// transactional across containers — a mid-save failure can leave a balance out of step. Recompute
// balances from the Transactions container (or move to a TransactionalBatch) if that ever bites.
public class TransactionService(CosmosDbContext context) : ITransactionService
{
    public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
    {
        var exists = await context.Transactions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == transaction.Id) is not null;
        if (exists) throw new ConflictException("Transaction already exists");

        await ApplyToBalancesAsync(transaction, 1);
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid id)
    {
        var transaction = await context.Transactions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.ToString());
        return transaction ?? throw new NotFoundException("Transaction not found");
    }

    public async Task<List<Transaction>> GetTransactionsByAccountAsync(Guid accountId)
    {
        var id = accountId.ToString();
        return await context.Transactions.AsNoTracking()
            .Where(x => x.FromAccountId == id || x.ToAccountId == id || x.GeneralAccountId == id)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByEventAsync(Guid eventId)
    {
        var id = eventId.ToString();
        return await context.Transactions.AsNoTracking()
            .Where(x => x.EventId == id)
            .ToListAsync();
    }

    public async Task<Transaction> UpdateTransactionAsync(Guid id, Transaction updatedTransaction)
    {
        var existing = await GetTransactionByIdAsync(id);
        updatedTransaction.Id = existing.Id;

        await ApplyToBalancesAsync(existing, -1);
        await ApplyToBalancesAsync(updatedTransaction, 1);
        context.Transactions.Update(updatedTransaction);
        await context.SaveChangesAsync();
        return updatedTransaction;
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id.ToString());
        if (transaction is null) throw new NotFoundException("Transaction not found");

        await ApplyToBalancesAsync(transaction, -1);
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();
    }

    // Sign rules carried over from the legacy AddAccountTransaction stored procedure: a credit adds to
    // the general and "to" accounts and subtracts from the "from" account; a debit is the reverse.
    // Transfers don't move the general account. sign = -1 reverses a previously applied transaction.
    private async Task ApplyToBalancesAsync(Transaction transaction, int sign)
    {
        var amount = sign * (transaction.Direction == DebitCreditType.Credit ? transaction.Amount : -transaction.Amount);

        if (transaction.TransactionMethod != TransactionType.Transfer)
            await AdjustBalanceAsync(transaction.GeneralAccountId, amount);
        await AdjustBalanceAsync(transaction.FromAccountId, -amount);
        await AdjustBalanceAsync(transaction.ToAccountId, amount);
    }

    private async Task AdjustBalanceAsync(string accountId, decimal amount)
    {
        if (string.IsNullOrEmpty(accountId)) return;
        var account = await context.Accounts.FindAsync(accountId)
                      ?? throw new NotFoundException($"Account {accountId} not found");
        account.Balance += amount;
    }
}
