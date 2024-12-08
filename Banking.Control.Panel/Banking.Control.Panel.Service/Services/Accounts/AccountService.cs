using Banking.Control.Panel.Model;
using Microsoft.EntityFrameworkCore;


namespace Banking.Control.Panel.Service
{ 
    public class AccountService : IAccountService
    {
        public readonly ApplicationDbContext _applicationDbContext;
        public AccountService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Account> AddAccount(Account account)
        {
            await _applicationDbContext.Accounts.AddAsync(account);
            await _applicationDbContext.SaveChangesAsync();
            return account;
        }

        public async Task<Account> DeleteAccount(int id)
        {
            var account = await _applicationDbContext.Accounts.FindAsync(id);
            _applicationDbContext.Accounts.Remove(account);

            // Save the changes to the database
            await _applicationDbContext.SaveChangesAsync();

            // Optionally, you can return the deleted user or an appropriate result
            return account;
        }

        public async Task<Account> GetAccountById(int id)
        {
            var account = await _applicationDbContext.Accounts.FindAsync(id);
            return account;
        }

        public async Task<List<Account>> GetAllAccount()
        {
            var account = await _applicationDbContext.Accounts.ToListAsync();
            return account;
        }

        public async Task<Account> UpdateAccount(int id, Account account)
        {
            var response = await _applicationDbContext.Accounts.FindAsync(id);

            if (response != null)
            {
                response!.AccountNumber = account.AccountNumber;
                response.AccountType = account.AccountType;
                response.Balance = account.Balance;
                await _applicationDbContext.SaveChangesAsync();

            }
            return response;
        }
    }
}
