using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.TeleTrust;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class CurrencyTransactionService
    {
        //Add currency transactions to database
        public static void AddCurrencyTransactions(List<CurrencyTransaction> transactions, Account account)
        {
            //Add account
            transactions = AddAccountToTransactions(transactions, account);

            //Match partners with db
            transactions = MatchTransactionsWithExistingPartners(transactions);

            //Remove dupes
            transactions = RemoveDupes(transactions, account);

            //Fill null values strings until migration for db was adapted
            transactions = FillNullWithEmptyString(transactions);

            //Save data to database
            using var context = new CashflowContext();
            context.Attach(account);
            foreach (var transaction in transactions)
            {
                context.Attach(transaction.TransactionPartner);
            }
            context.AddRange(transactions);
            context.SaveChanges();
        }
        //Get all transactions from database
        public static List<CurrencyTransaction> GetTransactions()
        {
            List<CurrencyTransaction> transactions = new();
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                transactions = new List<CurrencyTransaction>(context.CurrencyTransactions
                    .Include(t => t.TransactionPartner)
                    .Include(t => t.Account)
                    .Include(t => t.Budget));
            }
            return transactions;
        }
        //Get all transactions from database for a specific account
        public static List<CurrencyTransaction> GetTransactions(Account account)
        {
            List<CurrencyTransaction> transactions = new();
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                transactions = new List<CurrencyTransaction>(context.CurrencyTransactions
                    .Where(t => t.Account.ID == account.ID)
                    .Include(t => t.TransactionPartner)
                    .Include(t => t.Account)
                    .Include(t=> t.Budget));
            }
            return transactions;
        }
        //Get all transactions between 2 dates
        public static List<CurrencyTransaction> GetTransactions(DateTimeOffset firstDay, DateTimeOffset lastDay)
        {
            List<CurrencyTransaction> transactions = new();
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                transactions = new List<CurrencyTransaction>(context.CurrencyTransactions
                    .Where(t => t.DateTime >= firstDay)
                    .Where(t => t.DateTime <= lastDay)
                    .Include(t => t.TransactionPartner)
                    .Include(t => t.Account)
                    .Include(t => t.Budget));
            }
            return transactions;
        }
        //Get earliest recorded transaction
        public static CurrencyTransaction GetFirstTransaction()
        {
            using var context = new CashflowContext();
            CurrencyTransaction transaction = context.CurrencyTransactions.OrderBy(t => t.DateTime).FirstOrDefault();
            return transaction;
        }
        //Get last recorded transaction
        public static CurrencyTransaction GetLastTransaction()
        {
            using var context = new CashflowContext();
            CurrencyTransaction transaction = context.CurrencyTransactions.OrderByDescending(t => t.DateTime).FirstOrDefault();
            return transaction;
        }
        //Update budgets for transaction with specific partner
        public static async Task UpdateBudgets(TransactionPartner partner)
        {
            using var context = new CashflowContext();
            List<CurrencyTransaction> transactions = new(
                context.CurrencyTransactions
                .Where(t => t.TransactionPartner == partner));
            //Update budget for each transaction with specific partner
            foreach (var transaction in transactions) 
            {
                transaction.Budget = partner.Budget;
            }
            await context.SaveChangesAsync();
        }
        //Method to match the partners that are inside the database with the partners in the transaction list
        private static List<CurrencyTransaction> MatchTransactionsWithExistingPartners(List<CurrencyTransaction> transactions)
        {
            List<TransactionPartner> partners = TransactionPartnerService.GetAllPartners();
            foreach (CurrencyTransaction transaction in transactions)
            {
                foreach (TransactionPartner partner in partners)
                {
                    if (partner.Name == transaction.TransactionPartner.Name)
                    {
                        transaction.TransactionPartner = partner;
                        if(partner.Budget != null)
                        {
                            transaction.Budget = partner.Budget;
                        }
                        break;
                    }
                }
            }
            return transactions;
        }
        //Method to add account to transactionslist
        private static List<CurrencyTransaction> AddAccountToTransactions(List<CurrencyTransaction> transactions, Account account)
        {
            foreach(CurrencyTransaction transaction in transactions)
            {
                transaction.Account = account;
            }
            return transactions;
        }
        //Method to eliminate null values from NOTNULL fields
        private static List<CurrencyTransaction> FillNullWithEmptyString(List<CurrencyTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.Info == null)
                {
                    transaction.Info = "";
                }
                if (transaction.Reference == null)
                {
                    transaction.Reference = "";
                }
                if (transaction.Currency == null)
                {
                    transaction.Currency = "";
                }
            }
            return transactions;
        }
        //Method to remove dupes from transactionlist
        private static List<CurrencyTransaction> RemoveDupes(List<CurrencyTransaction> newTransactions, Account account) 
        {
            using var context = new CashflowContext();
            var existingTransactions = GetTransactions(account);
            List<CurrencyTransaction> uniqueTransactions = new List<CurrencyTransaction>();
            bool isDupe = false;
            foreach(var newTransaction in newTransactions)
            {
                foreach(var existingTransaction in existingTransactions.Where(t => t.TransactionPartner.ID == newTransaction.TransactionPartner.ID))
                {
                    if(newTransaction.DateTime == existingTransaction.DateTime
                        && newTransaction.Amount == existingTransaction.Amount
                        && newTransaction.Reference == existingTransaction.Reference)
                    {
                        isDupe = true;
                    }
                }
                if (!isDupe)
                {
                    uniqueTransactions.Add(newTransaction);
                }
            }
            return uniqueTransactions;
        }

    }
}
