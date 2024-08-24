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

            //Add method to check for dupes here
            //#####################################################################

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
        public static List<CurrencyTransaction> GetAllTransactions()
        {
            List<CurrencyTransaction> transactions = new();
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                transactions = new List<CurrencyTransaction>(context.CurrencyTransactions
                    .Include(t => t.TransactionPartner)
                    .Include(t => t.Account));
            }
            return transactions;
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
    }
}
