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
        public static List<Account> GetAllAccounts()
        {
            List<Account> accounts = new();
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                accounts = new List<Account>(context.Accounts);
            }
            return accounts;
        }
    }
}
