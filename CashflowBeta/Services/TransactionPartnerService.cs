using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class TransactionPartnerService
    {
        //Sorts out distinct partners of a list of transactions
        public static List<TransactionPartner> GetDistinctPartners(IEnumerable<CurrencyTransaction> transactions)
        {
            List<TransactionPartner> partners = new();
            bool dupe = false;
            //Go through each transaction
            foreach (var transaction in transactions)
            {
                //
                foreach (var partner in partners)
                {
                    if (partner.Name == transaction.TransactionPartner.Name)
                    {
                        dupe = true;
                    }
                    if (!dupe) { partners.Add(transaction.TransactionPartner); }
                    dupe = false;
                }
            }
            return partners;
        }

        public static void AddTransactionPartners(List<TransactionPartner> partners)
        {

        }
    }
}
