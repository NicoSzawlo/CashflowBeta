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
                //Go through all partners
                foreach (var partner in partners)
                {
                    if (partner.Name == transaction.TransactionPartner.Name)
                    {
                        dupe = true;
                    }
                }
                if (!dupe) { partners.Add(transaction.TransactionPartner); }
                dupe = false;
            }
            return partners;
        }
        //Sorts out distinct partners of 2 lists of partners
        public static List<TransactionPartner> GetDistinctPartners(IEnumerable<TransactionPartner> knownCollection, IEnumerable<TransactionPartner> newCollection)
        {
            List<TransactionPartner> newPartners = new();
            bool dupe = false;
            //Go through each current partner
            foreach (var newPartner in newCollection)
            {
                //Go through all new partners
                foreach (var partner2 in knownCollection)
                {
                    if (newPartner.Name == partner2.Name)
                    {
                        dupe = true;
                    }
                }
                if (!dupe) { newPartners.Add(newPartner); }
                dupe = false;
            }
            return newPartners;
        }
        //Add collection of partners to database
        public static void AddTransactionPartners(List<TransactionPartner> newPartners)
        {
            using (var context = new CashflowContext())
            {
                //Load current collection of partners
                List<TransactionPartner> currentPartners = new(context.TransactionsPartners);
                //Sort out new partners
                List<TransactionPartner> distinctPartners = GetDistinctPartners(currentPartners, newPartners);
                //Add and save to database
                context.TransactionsPartners.AddRange(newPartners);
                context.SaveChanges();
            }
        }
    }
}
