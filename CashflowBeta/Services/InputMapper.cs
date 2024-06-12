using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CashflowBeta.Services
{
    public class InputMapper
    {
        public Account AddAccount()
        {
            Account demoAccount = new Account
            {
                ID = 1,
                Name = "John Doe's Checking",
                AccountIdentifier = "1234567890",
                BankIdentifier = "Bank of America",
                Balance = 10000
            };

            return demoAccount;
        }

        public TransactionPartner GetPartner()
        {
            TransactionPartner demoPartner = new TransactionPartner
            {
                ID = 1,
                Name = "John Doe",
                AccountIdentifier = "1234567890",
                BankIdentifier = "Bank of America",
                Bankcode = "BOFA"
            };
            return demoPartner;
        }
        public CurrencyTransaction GetTransaction()
        {
            CurrencyTransaction transaction = new CurrencyTransaction
            {
                ID = 1,
                DateTime = DateTime.Now,
                Amount = 550,
                Currency = "USD",
                Info = "Payroll deposit",
                Reference = "PAY123",
                TransactionPartner = GetPartner(),
                Account = 
            };
            return transaction; 

        }
    }
}
