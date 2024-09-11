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
            DateTime testtime = Convert.ToDateTime("08.09.2024 00:00:00");
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
