using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;

namespace CashflowBeta.Services;

public class BudgetService
{
    private readonly CashflowContext _db;
    private readonly AppDataStore _appDataStore;
    private readonly CurrencyTransactionService _currencyTransactionService;
    public BudgetService(
        AppDataStore appDataStore,
        CurrencyTransactionService currencyTransactionService,
        CashflowContext cashflowContext)
    {
        _db = cashflowContext;
        _appDataStore = appDataStore;
        _currencyTransactionService = currencyTransactionService;
    }

    //Load all budgets from database asynchronously
    public async Task<List<Budget>> GetAllAsync()
    {
        return await _db.Budgets.ToListAsync();
    }

    public static List<Budget> GetAllBudgets()
    {
        List<Budget> budgets = new();

        using (var context = new CashflowContext())
        {
            budgets = new List<Budget>(context.Budgets);
        }

        return budgets;
    }

    public async Task UpdateBudgetAsync(Budget budget)
    {
        //If ID of budget is null, add to database
        if (budget.ID == 0 || budget.ID == null) // Insert new record
        {
            _db.Budgets.Add(budget);
            _db.SaveChanges();
            _appDataStore.Budgets.Add(budget);
        }
        else // Update existing record
        {
            var existingBudget = await _db.Budgets.SingleOrDefaultAsync(b => b.ID == budget.ID);

            if (existingBudget != null)
            {
                // Update the existing budget properties
                existingBudget.Name = budget.Name;
                existingBudget.Amount = budget.Amount;
                //Udate the local instance
                var local = _appDataStore.Budgets.Single(bdgt => bdgt.ID == existingBudget.ID);
                local.Name = budget.Name;
                local.Amount = budget.Amount;
            }
        }
        // Use SaveChangesAsync to save changes asynchronously
        await _db.SaveChangesAsync(); 
    }

    public async Task RemoveBudgetAsync(Budget budget)
    {
        // Ensure the budget object is valid and has a valid ID
        if (budget != null && budget.ID != 0)
        {
            // Retrieve the budget entity from the database using the provided ID
            var existingBudget = await _db.Budgets.SingleOrDefaultAsync(b => b.ID == budget.ID);

            if (existingBudget != null)
            {
                // Remove the budget from the context
                _db.Budgets.Remove(existingBudget);
                _appDataStore.Budgets.Remove(existingBudget);
                // Save changes to the database
                await _db.SaveChangesAsync();
            }
        }
    }

    public List<Budget> CalculateBudgetPerMonth(DateTimeOffset month)
    {
        var transactions = _appDataStore.CurrencyTransactions.Where(t => t.DateTime >= GetFirstDayOfMonth(month))
                .Where(t => t.DateTime <= GetLastDayOfMonth(month));
        var budgets = GetAllBudgets();
        foreach (var budget in budgets)
        foreach (var transaction in transactions.Where(t => t.Budget?.ID == budget.ID))
        {
            if (transaction.Budget == null) continue;
            budget.Amount += transaction.Amount;
        }

        return budgets;
    }

    public List<Budget> CalculateIncomeExpensePerMonth(DateTimeOffset month)
    {
        var transactions = _appDataStore.CurrencyTransactions.Where(t => t.DateTime >= GetFirstDayOfMonth(month))
                .Where(t => t.DateTime <= GetLastDayOfMonth(month));
        var io = new List<Budget>
        {
            new()
            {
                ID = 1,
                Name = "Income",
                Description = "Incomes",
                Amount = 0
            },
            new()
            {
                ID = 2,
                Name = "Expense",
                Description = "Expenses",
                Amount = -0
            }
        };
        foreach (var transaction in transactions.Where(t => t.Amount > 0))
            if (transaction.TransactionPartner.Name != "Own Transfer" ||
                transaction.TransactionPartner.ParentPartner?.Name != "Own Transfer")
                io[0].Amount += transaction.Amount;
        foreach (var transaction in transactions.Where(t => t.Amount < 0))
            if (transaction.TransactionPartner.Name != "Own Transfer" ||
                transaction.TransactionPartner.ParentPartner?.Name != "Own Transfer")
                io[1].Amount += transaction.Amount;
        io[1].Amount = io[1].Amount * -1;
        return io;
    }

    private static DateTimeOffset GetFirstDayOfMonth(DateTimeOffset month)
    {
        var firstDay = month;
        while (firstDay.Month == month.Month) firstDay = firstDay.AddDays(-1);
        firstDay.AddDays(1);
        return firstDay;
    }

    private static DateTimeOffset GetLastDayOfMonth(DateTimeOffset month)
    {
        var lastDay = month;
        while (lastDay.Month == month.Month) lastDay = lastDay.AddDays(1);
        lastDay.AddDays(-1);
        return lastDay;
    }
}