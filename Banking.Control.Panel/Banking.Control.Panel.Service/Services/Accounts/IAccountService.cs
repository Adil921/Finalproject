using Banking.Control.Panel.Model;
namespace Banking.Control.Panel.Service
{
    public interface IAccountService
    {
        public Task<List<Account>> GetAllAccount();
        public Task<Account> GetAccountById(int id);
        public Task<Account> AddAccount(Account account);
        public Task<Account> UpdateAccount(int id, Account account);
        public Task<Account> DeleteAccount(int id);
        
    }
}
