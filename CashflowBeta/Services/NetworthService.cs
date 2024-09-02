using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class NetworthService
    {
        public static List<Networth> CalculateNetworth()
        {
            //Get all transactions in database and generate Datelist for all available transactions
            List<CurrencyTransaction> transactions = CurrencyTransactionService.GetAllTransactions();
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
            //Calculate in balance from the last day to the first
            foreach (Networth net in networthTrend.OrderBy(netitem => netitem.DateTime))
            {
                net.Capital = net.Capital + balance;
                balance = net.Capital;
            }

            return networthTrend;
        }
        
        private static List<DateTime> GetDateListFromTransactionList(List<CurrencyTransaction> transactions)
        {
            List<DateTime> dateList = new();
            DateTime firstDate = transactions[0].DateTime;
            DateTime lastDate = transactions[0].DateTime;
            DateTime runningDate = new();
            TimeSpan timeSpan = new();
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
            runningDate = firstDate;
            timeSpan = lastDate - firstDate;
            for (int i = 0; i < timeSpan.TotalDays; i++)
            {
                dateList.Add(runningDate);
                runningDate = runningDate.AddDays(1);
                
            }
            return dateList;
        }
    }
}
