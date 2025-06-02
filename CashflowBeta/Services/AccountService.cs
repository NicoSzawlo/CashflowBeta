using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;

namespace CashflowBeta.Services;

public class AccountService
{
    private readonly AppDataStore _appDataStore;
    private readonly CashflowContext _db;
    public AccountService(CashflowContext db, AppDataStore appDataStore)
    {
        _db = db;
        _appDataStore = appDataStore;
    }
    //Load all accounts from database asynchronously
    public async Task<List<Account>> GetAllAsync()
    {
        return await _db.Accounts.ToListAsync();
    }

    // Method to calculate and return the total balance of all accounts
    public decimal GetTotalBalance()
    {
        // Initialize a variable to store the total balance
        decimal balance = 0;

        // Get a list of all accounts
        //var accounts = GetAllAccounts();

        // Loop through each account and add its balance to the total
        //foreach (var account in accounts) balance += account.Balance;

        // Return the total balance
        return balance;
    }

    //Add account to database
    public Account AddNewAccount(Account acc)
    {
        _db.Accounts.Add(acc);
        _db.SaveChanges();

        return acc;
    }

    //Update account
    public Account UpdateAccount(Account account)
    {
        if (account.ID == 0 || account.ID == null) // Insert new record
        {
            AddNewAccount(account);
        }
        else // Update existing record
        {
            var existingAccount = _db.Accounts.SingleOrDefault(a => a.ID == account.ID);

            if (existingAccount != null)
            {
                // Update the existing budget properties
                existingAccount.Name = account.Name;
                existingAccount.AccountIdentifier = account.AccountIdentifier;
                existingAccount.BankIdentifier = account.BankIdentifier;
                existingAccount.Balance = account.Balance;
            }
        }

        _db.SaveChanges();
        return account;
    }

    public async Task DeleteAccount(Account? account)
    {
        //Load account instance and all related transactions
        var acc = _db.Accounts.SingleOrDefault(a => a.ID == account.ID);
        var transactions = _db.CurrencyTransactions.Where(t => t.Account == acc);
        var trends = _db.NetworthTrend.Where(n => n.Account == acc);
        //Remove all transactions and accounts
        _db.CurrencyTransactions.RemoveRange(transactions);
        _db.NetworthTrend.RemoveRange(trends);
        _db.Accounts.Remove(acc);
        //Save changes
        await _db.SaveChangesAsync();
    }
}