using Avalonia.Win32.Interop.Automation;
using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class NetworthService
    {
        //Load total networth trend from database
        public static List<Networth> GetNetworthTrend()
        {
            List<Networth> networthTrend = new();
            using (var context = new CashflowContext())
            {
                networthTrend = new List<Networth>(
                    context.NetworthTrend
                    .Where(trendpoint => trendpoint.Account == null));
            };
            return networthTrend; 
        }
        //Load account specific networth trend from database
        public static List<Networth> GetNetworthTrend(Account account)
        {
            List<Networth> networthTrend = new();
            using (var context = new CashflowContext())
            {
                networthTrend = new List<Networth>(
                    context.NetworthTrend
                    .Include(trendpoint => trendpoint.Account)
                    .Where(trendpoint => trendpoint.Account.ID == account.ID));
            };
            return networthTrend;
        }
        //Method to update overall networth after a statement was added
        public static void AddNetworth()
        {
            List<Networth> networthTrend = CalculateNetworth();
            //Save data to database
            using var context = new CashflowContext();
            var entitiesToDelete = context.NetworthTrend.Where(n => n.Account == null);
            if (entitiesToDelete.Any())
            {
                context.NetworthTrend.RemoveRange(entitiesToDelete);
                context.SaveChanges();
            }
            context.AddRange(networthTrend);
            context.SaveChanges();

        }
        //Method to update account specific networth
        public static void AddNetworth(Account account)
        {
            List<Networth> networthTrend = CalculateNetworth(account);
            //Save data to database
            using var context = new CashflowContext();
            var entitiesToDelete = context.NetworthTrend.Where(n => n.Account.ID == account.ID);
            if (entitiesToDelete.Any())
            {
                context.NetworthTrend.RemoveRange(entitiesToDelete);
                context.SaveChanges();
            }
            context.Attach(account);
            context.AddRange(networthTrend);
            context.SaveChanges();

        }
        //Method to recalculate complete networth table
        public static void RecalculateNetworthTrends()
        {
            var accounts = AccountService.GetAllAccounts();
            AddNetworth();
            foreach(Account acc in accounts)
            {
                AddNetworth(acc);
            }
        }
        //Calculate and return networth list for all transaction currently in database
        public static List<Networth> CalculateNetworth()
        {
            //Get all transactions in database and generate Datelist for all available transactions
            List<CurrencyTransaction> transactions = CurrencyTransactionService.GetTransactions();
            List<DateTime> dates = GetDateListFromTransactionList(transactions);

            //Initialize networthtrend and calculate sum of transactions per day
            List<Networth> networthTrend = new List<Networth>();
            decimal tempdecimal = 0;
            foreach (DateTime date in dates)
            {
                tempdecimal = 0;
                foreach(CurrencyTransaction transaction in transactions.Where(item => item.DateTime == date))
                {
                    tempdecimal += transaction.Amount;
                }
                networthTrend.Add(new Networth { DateTime = date, Capital = tempdecimal });
            }
            //Get total account balance
            decimal balance = AccountService.GetTotalBalance();
            //Order list and Set current total balance on latest day
            networthTrend = networthTrend.OrderByDescending(netitem => netitem.DateTime).ToList();
            networthTrend[0].Capital = balance;

            //Go through each day beginning from the latest+1 and calculate networth on that day
            //by subtracting the transaction from a day from its total networth
            for(int i = 1; i < networthTrend.Count; i++)
            {
                networthTrend[i].Capital = networthTrend[i-1].Capital - networthTrend[i].Capital;
            }
            return networthTrend;
        }
        //Calculate and return networth list for all transaction of a specific account 
        public static List<Networth> CalculateNetworth(Account account)
        {
            //Get all transactions in database and generate Datelist for all available transactions
            List<CurrencyTransaction> transactions = CurrencyTransactionService.GetTransactions(account);
            List<DateTime> dates = GetDateListFromTransactionList(transactions);

            //Initialize networthtrend and calculate sum of transactions per day
            List<Networth> networthTrend = new List<Networth>();
            decimal tempdecimal = 0;
            foreach (DateTime date in dates)
            {
                tempdecimal = 0;
                foreach (CurrencyTransaction transaction in transactions.Where(item => item.DateTime == date))
                {
                    tempdecimal += transaction.Amount;
                }
                networthTrend.Add(new Networth { DateTime = date, Capital = tempdecimal , Account = account});
            }
            //Get total account balance
            decimal balance = account.Balance;
            //Order list and Set current total balance on latest day
            networthTrend = networthTrend.OrderByDescending(netitem => netitem.DateTime).ToList();
            networthTrend[0].Capital = balance;

            //Go through each day beginning from the latest+1 and calculate networth on that day
            //by subtracting the transaction from a day from its total networth
            for (int i = 1; i < networthTrend.Count; i++)
            {
                networthTrend[i].Capital = networthTrend[i - 1].Capital - networthTrend[i].Capital;
            }
            return networthTrend;
        }
        //Create list of dates for the timespan between latest and earliest documented transaction
        private static List<DateTime> GetDateListFromTransactionList(List<CurrencyTransaction> transactions)
        {   //INitialize variables
            List<DateTime> dateList = new();
            DateTime firstDate = transactions[0].DateTime;
            DateTime lastDate = transactions[0].DateTime;
            DateTime runningDate = new();
            TimeSpan timeSpan = new();

            //Get first and last date in list
            foreach (CurrencyTransaction transaction in transactions)
            {
                if (transaction.DateTime < firstDate)
                {
                    firstDate = transaction.DateTime;
                }
                if (transaction.DateTime > lastDate)
                {
                    lastDate = transaction.DateTime;
                }
            }
            //Setup datevariable for list creation
            runningDate = firstDate;
            //Calculate timespan
            timeSpan = lastDate - firstDate;

            //Create list of dates between first and last transaction + 1 day for calculation of networth
            for (int i = 0; i <= timeSpan.TotalDays+1; i++)
            {
                dateList.Add(runningDate);
                runningDate = runningDate.AddDays(1);
                
            }
            return dateList;
        }

    }
}
