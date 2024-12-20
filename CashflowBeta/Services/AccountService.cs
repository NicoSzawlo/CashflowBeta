﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashflowBeta.Models;

namespace CashflowBeta.Services;

public class AccountService
{
    //Load all accounts from database
    public static List<Account?> GetAllAccounts()
    {
        List<Account?> accounts;
        using (var context = new CashflowContext())
        {
            accounts = new List<Account?>(context.Accounts);
        }

        return accounts;
    }

    // Method to calculate and return the total balance of all accounts
    public static decimal GetTotalBalance()
    {
        // Initialize a variable to store the total balance
        decimal balance = 0;

        // Get a list of all accounts
        var accounts = GetAllAccounts();

        // Loop through each account and add its balance to the total
        foreach (var account in accounts) balance += account.Balance;

        // Return the total balance
        return balance;
    }

    //Add account to database
    public static Account? AddNewAccount(Account? acc)
    {
        using var context = new CashflowContext();
        context.Accounts.Add(acc);
        context.SaveChanges();

        return acc;
    }

    //Update account
    public static Account? UpdateAccount(Account? account)
    {
        using var context = new CashflowContext();
        if (account.ID == 0 || account.ID == null) // Insert new record
        {
            context.Accounts.Add(account); // Use AddAsync for asynchronous insert
        }
        else // Update existing record
        {
            var existingAccount = context.Accounts.SingleOrDefault(a => a.ID == account.ID);

            if (existingAccount != null)
            {
                // Update the existing budget properties
                existingAccount.Name = account.Name;
                existingAccount.AccountIdentifier = account.AccountIdentifier;
                existingAccount.BankIdentifier = account.BankIdentifier;
                existingAccount.Balance = account.Balance;
            }
        }

        context.SaveChanges();
        return account;
    }

    public static async Task DeleteAccount(Account? account)
    {
        using var context = new CashflowContext();
        //Load account instance and all related transactions
        var acc = context.Accounts.SingleOrDefault(a => a.ID == account.ID);
        var transactions = context.CurrencyTransactions.Where(t => t.Account == acc);
        var trends = context.NetworthTrend.Where(n => n.Account == acc);
        //Remove all transactions and accounts
        context.CurrencyTransactions.RemoveRange(transactions);
        context.NetworthTrend.RemoveRange(trends);
        context.Accounts.Remove(acc);
        //Save changes
        await context.SaveChangesAsync();
    }
}