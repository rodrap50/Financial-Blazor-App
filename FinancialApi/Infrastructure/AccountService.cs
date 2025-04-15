using Financial.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Api.Infrastructure;
public class AccountService(CosmosDbContext _context) : IAccountService
{
    public async Task<bool> AccountExistsAsync(Guid accountId)
    {
        return await _context.Accounts.AnyAsync(a => a.Id == accountId.ToString());
    }

    public async Task<bool> AccountExistsByNameAsync(string accountName)
    {
        return await _context.Accounts.AnyAsync(a => a.AccountName == accountName);
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        
        if(string.IsNullOrEmpty(account.AccountName))
        {
            throw new ArgumentException("Account name cannot be null or empty.", nameof(account.AccountName));
        }
        var res = await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return res.Entity;
    }

    public async Task<bool> DeleteAccountAsync(Guid accountId)
    {
        var product = await _context.Accounts.FindAsync(accountId);
        if(product is not null)
        {
            _context.Accounts.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Task<bool> DeleteAllAccounts()
    {
        _context.Accounts.RemoveRange(_context.Accounts);
        return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }

    public async Task<Account> GetAccountByIdAsync(Guid accountId)
    {
        return await _context.Accounts.FindAsync(accountId);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        throw new NotImplementedException();
    }
}
