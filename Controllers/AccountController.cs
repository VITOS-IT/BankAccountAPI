using AccountServicesAPI.Models;
using AccountServicesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountServicesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("getAccount/{id}")]
        public Account GetAccount(int id)
        {
            return _service.GetAccount(id);
        }

        [HttpPost("createAccount")]
        public AccountCreationStatus Post([FromBody] Account account)
        {
            AccountCreationStatus status = new();
            var acc = _service.AddAccount(account);
            if(acc != null)
            {
                status.AccountID = acc.AccountID;
                status.AccountStatus = "Success";
            }
            else
            {
                status.AccountID = 0;
                status.AccountStatus = "Failed";
            }
            return status;
        }

        [HttpPut("{id}")]
        public Account Put(int id, [FromBody] Account account)
        {
            var acc = _service.EditAccount(id, account);
            return acc;
        }

        [HttpGet("getCustomerAccounts/{id}")]
        public List<Account> GetCustomerAccounts(string id)
        {
            return _service.GetAccountsByCustID(id);
        }
        [HttpGet("getAccountStatement")]
        public IEnumerable<Statement> Transactions(int AccId, DateTime FromDate,DateTime ToDate)
        {
            return _service.GetStatements(AccId,FromDate,ToDate);
        }
        [HttpPost("Withdraw")]
        public async Task<TransactionStatus> Withdraw(int accId, float amount)
        {
            return await _service.Withdraw(accId, amount);
        }
        [HttpPost("Deposit")]
        public async Task<TransactionStatus> Deposit(int accId, float amount)
        {
            return await _service.Deposit(accId, amount);
        }
        [HttpPost("Transfer")]
        public async Task<TransactionStatus> Transfer(int fromAccId,int toAccId, float amount)
        {
            return await _service.Transfer(fromAccId,toAccId, amount);
        }
    }
}