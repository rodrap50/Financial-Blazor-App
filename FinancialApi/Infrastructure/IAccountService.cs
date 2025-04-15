using Financial.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Api.Infrastructure;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(Account account);
    Task<Account> GetAccountByIdAsync(Guid accountId);
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> UpdateAccountAsync(Account account);
    Task<bool> DeleteAccountAsync(Guid accountId);
    Task<bool> DeleteAllAccounts();
    Task<bool> AccountExistsAsync(Guid accountId);
    Task<bool> AccountExistsByNameAsync(string accountName);
   
}
