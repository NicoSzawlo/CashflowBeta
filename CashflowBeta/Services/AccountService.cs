using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class AccountService
    {
        //Load all accounts from database
        public static List<Account> GetAllAccounts()
        {
            List<Account> accounts = new();
            using (var context = new CashflowContext())
            {
                accounts = new List<Account>(context.Accounts);
            }
            return accounts;
        }

        //Get total balance of all accounts
        public static decimal GetTotalBalance()
        {
            decimal balance = 0;
            List<Account> accounts = GetAllAccounts();

            foreach (var account in accounts)
            {
                balance += account.Balance;
            }
            return balance;
        }
    }
}
