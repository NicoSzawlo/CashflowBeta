using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;

namespace CashflowBeta.Services;

public class TransactionPartnerService
{
    //Sorts out distinct partners of a list of transactions
    public static List<TransactionPartner> GetDistinctPartners(IEnumerable<CurrencyTransaction> transactions)
    {
        List<TransactionPartner> partners = new();
        var dupe = false;
        //Go through each transaction
        foreach (var transaction in transactions)
        {
            //Go through all partners
            foreach (var partner in partners)
                if (partner.Name == transaction.TransactionPartner.Name)
                    dupe = true;
            if (!dupe) partners.Add(transaction.TransactionPartner);
            dupe = false;
        }

        return partners;
    }

    //Sorts out distinct partners of 2 lists of partners
    public static List<TransactionPartner> GetDistinctPartners(IEnumerable<TransactionPartner> knownCollection,
        IEnumerable<TransactionPartner> newCollection)
    {
        List<TransactionPartner> newPartners = new();
        var dupe = false;
        //Go through each current partner
        foreach (var newPartner in newCollection)
        {
            //Go through all new partners
            foreach (var partner2 in knownCollection)
                if (newPartner.Name == partner2.Name)
                    dupe = true;
            if (!dupe) newPartners.Add(newPartner);
            dupe = false;
        }

        return newPartners;
    }

    //Add collection of partners to database
    public static void AddTransactionPartners(List<TransactionPartner> newPartners)
    {
        using var context = new CashflowContext();
        InitializeSystemPartners(context);
        //Load current collection of partners
        List<TransactionPartner> currentPartners = new(context.TransactionsPartners);
        //Sort out new partners
        var distinctPartners = GetDistinctPartners(currentPartners, newPartners);
        //Add and save to database
        context.TransactionsPartners.AddRange(distinctPartners);
        context.SaveChanges();
    }

    //Load all partners from database
    public static List<TransactionPartner> GetAllPartners()
    {
        List<TransactionPartner> partners = new();

        using (var context = new CashflowContext())
        {
            partners = new List<TransactionPartner>(
                context.TransactionsPartners
                    .Include(p => p.Budget));
        }

        return partners;
    }

    //Load all partners from database
    public static async Task<List<TransactionPartner>> GetAllPartnersAsync()
    {
        List<TransactionPartner> partners = new();

        using (var context = new CashflowContext())
        {
            partners = new List<TransactionPartner>(
                context.TransactionsPartners
                    .Include(p => p.Budget));
        }

        return partners;
    }

    //Load instances of system partners from database
    public static List<TransactionPartner> GetSystemPartners()
    {
        List<TransactionPartner> systempartners = new();

        using (var context = new CashflowContext())
        {
            systempartners.Add(context.TransactionsPartners.Single(p => p.Name == "Cash Withdrawal"));
            systempartners.Add(context.TransactionsPartners.Single(p => p.Name == "Own Transfer"));
        }

        return systempartners;
    }

    //Set budget to partners
    public static async Task ApplyBudgetToPartnersAsync(Budget budget, List<TransactionPartner> partners)
    {
        using var context = new CashflowContext();
        foreach (var partner in partners)
        {
            var existingPartner = await context.TransactionsPartners.SingleOrDefaultAsync(p => p.ID == partner.ID);

            if (existingPartner != null)
                // Update the existing budget properties
                existingPartner.Budget = budget;
            await context.SaveChangesAsync();
            await CurrencyTransactionService.UpdateBudgets(partner);
        }
    }

    //Search keywords and return fitting partner
    public static TransactionPartner IdentifiedTransactionPartner(string input)
    {
        TransactionPartner identifiedPartner = new() { ID = 0, Name = "-" };

        //Get list of keywords linked with partners
        var partnerKeywords = FileService.LoadPartnerKeywords();
        if (input == null || input == "") return null;
        foreach (var entry in partnerKeywords)
        {
            var partner = entry.Key;
            var keywords = entry.Value;
            foreach (var keyword in keywords)
                if (input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    identifiedPartner = partner;
                    break;
                }
        }

        return identifiedPartner;
    }

    //Check for and add systempartners "Cash withdrawal/Own Transfer
    private static void InitializeSystemPartners(CashflowContext context)
    {
        if (context.TransactionsPartners.Count() <= 0)
        {
            TransactionPartner cashPartner = new()
            {
                Name = "Cash Withdrawal",
                AccountIdentifier = "-",
                BankIdentifier = "-",
                Bankcode = "-"
            };
            TransactionPartner ownTransferPartner = new()
            {
                Name = "Own Transfer",
                AccountIdentifier = "-",
                BankIdentifier = "-",
                Bankcode = "-"
            };
            context.TransactionsPartners.Add(cashPartner);
            context.TransactionsPartners.Add(ownTransferPartner);
            context.SaveChanges();
        }
    }
}