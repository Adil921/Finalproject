using Banking.Control.Panel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Banking.Control.Panel.Service;

namespace Banking.Control.Panel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _account;
        public AccountsController(IAccountService account)
        {
            _account = account;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAllAccount()
        {
            try
            {

            var response = await _account.GetAllAccount();
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            try
            {

           
            var response = await _account.GetAccountById(id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }


        [HttpPost("AddAccount")]
        public async Task<ActionResult<Account>> AddAccount(Account account)
        {
            try
            {

            var response = await _account.AddAccount(account);
            if (response == null) { return NotFound(); }
            else
            {
                return Ok(response);
            }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> UpdateAccount(int id, Account account)
        {
            try
            {           
            var response = await _account.UpdateAccount(id, account);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
               return Ok(response);
            }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> DeleteAccount(int id)
        {
            try
            {

            var response = await _account.DeleteAccount(id);
            if (response == null)
            {
                return NotFound();
            }
            else
            {

                return Ok(response);
            }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }
    }

}