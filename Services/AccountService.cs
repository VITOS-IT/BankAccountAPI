using AccountServicesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AccountServicesAPI.Services
{
    public class AccountService
    {
        private readonly AccountContext _context;
        //private string transactionUrl = "http://localhost:41002/api/Transaction/";
        private string transactionUrl = "https://banktransactionapi.azurewebsites.net/api/Transaction/";

        public AccountService(AccountContext context)
        {
            _context = context;
        }

        public List<Account> AllAccounts()
        {
            List<Account> accounts;
            accounts = _context.Accounts.ToList();
            return accounts;
        }

        public List<Account> GetAccountsByCustID(string id)
        {
            List<Account> accounts = new List<Account>();
            foreach (var item in _context.Accounts)
            {
                if (item.CustomerID == id)
                {
                    accounts.Add(item);
                }
            }
            return accounts;
        }

        public Account GetAccount(int id)
        {
            Account account = null;
            account = _context.Accounts.FirstOrDefault(p => p.AccountID == id);
            return account;
        }

        public Account AddAccount(Account account)
        {
            if (account != null)
            {
                Account newAccount = new Account();
                newAccount.AccountID = account.AccountID;
                newAccount.AccountType = account.AccountType;
                newAccount.CustomerID = account.CustomerID;
                newAccount.Balance = account.Balance;
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();
                return newAccount;
            }
            return null;
        }

        public Account EditAccount(int id, Account account)
        {
            Account newAccount = null;
            newAccount = _context.Accounts.FirstOrDefault(p => p.AccountID == id);
            newAccount.AccountID = account.AccountID;
            newAccount.AccountType = account.AccountType;
            newAccount.CustomerID = account.CustomerID;
            newAccount.Balance = account.Balance;
            _context.Accounts.Update(newAccount);
            _context.SaveChanges();
            return newAccount;
        }

        public Account RemoveAccount(int id)
        {
            Account account = null;
            account = _context.Accounts.FirstOrDefault(p => p.AccountID == id);
            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return account;
        }

        public List<TransactionDTO> GetStatements(int AccId, DateTime FromDate, DateTime ToDate)
        {
            List<TransactionDTO> AllTransactions = new List<TransactionDTO>();
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(transactionUrl); 
                    var getTask = client.GetAsync("getTransactions/" + AccId);
                    getTask.Wait();
                    var result = getTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var data = result.Content.ReadFromJsonAsync<List<TransactionDTO>>();
                        data.Wait();
                        AllTransactions = data.Result;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            if (AllTransactions.Count != 0)
            {
                foreach (var item in AllTransactions)
                {
                    if (item.Date >= FromDate && item.Date <= ToDate || item.Date == FromDate && item.Date == ToDate || item.Date >= FromDate && item.Date == ToDate || item.Date == FromDate && item.Date <= ToDate)
                    {
                        transactions.Add(item);
                    }
                }
            }
            return transactions;
        }

        public async Task<TransactionStatus> Withdraw(int accId, float amount)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            string status = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(transactionUrl);
                    var postTask = await client.PostAsync($"Withdraw?accId={accId}&amount={amount}", null);
                    if (postTask.IsSuccessStatusCode)
                    {
                        status = postTask.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            transactionStatus.Message = status;
            var Acc = GetAccount(accId);
            if(Acc != null)
            {
                transactionStatus.SourceBalance = Acc.Balance;
            }
            return transactionStatus;
        }

        public async Task<TransactionStatus> Deposit(int accId, float amount)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            string status = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(transactionUrl);
                    var postTask = await client.PostAsync($"Deposit?accountId={accId}&amount={amount}", null);
                    if (postTask.IsSuccessStatusCode)
                    {
                        status = postTask.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            transactionStatus.Message = status;
            var Acc = GetAccount(accId);
            if (Acc != null)
            {
                transactionStatus.SourceBalance = Acc.Balance;
            }
            return transactionStatus;
        }

        public async Task<TransactionStatus> Transfer(int FromAccId, int ToAccId, float Amount)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            string status = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(transactionUrl);
                    var postTask = await client.PostAsync($"Transfer?sourceAccountId={FromAccId}&targetAccountId={ToAccId}&amount={Amount}", null);
                    if (postTask.IsSuccessStatusCode)
                    {
                        status = postTask.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            transactionStatus.Message = status;
            var SourceAcc = GetAccount(FromAccId);
            var TargetAcc = GetAccount(ToAccId);
            if(SourceAcc != null && TargetAcc != null)
            {
                transactionStatus.SourceBalance = SourceAcc.Balance;
                transactionStatus.DestinationBalance = TargetAcc.Balance;
            }
            return transactionStatus;
        }
    }
}
