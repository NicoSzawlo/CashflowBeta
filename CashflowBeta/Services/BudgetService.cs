using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services
{
    public class BudgetService
    {
        public static List<Budget> GetAllBudgets()
        {
            List<Budget> budgets = new();

            using (var context = new CashflowContext())
            {
                budgets = new List<Budget>(context.Budgets);
            }
            return budgets;
        }
        public static async Task UpdateBudgetAsync(Budget budget)
        {
            using var context = new CashflowContext();

            if (budget.ID == 0 || budget.ID == null) // Insert new record
            {
                await context.Budgets.AddAsync(budget); // Use AddAsync for asynchronous insert
            }
            else // Update existing record
            {
                var existingBudget = await context.Budgets.SingleOrDefaultAsync(b => b.ID == budget.ID);

                if (existingBudget != null)
                {
                    // Update the existing budget properties
                    existingBudget.Name = budget.Name;
                    existingBudget.Amount = budget.Amount;
                }
            }

            await context.SaveChangesAsync(); // Use SaveChangesAsync to save changes asynchronously
        }
        public static async Task RemoveBudgetAsync(Budget budget)
        {
            using var context = new CashflowContext();

            // Ensure the budget object is valid and has a valid ID
            if (budget != null && budget.ID != 0)
            {
                // Retrieve the budget entity from the database using the provided ID
                var existingBudget = await context.Budgets.SingleOrDefaultAsync(b => b.ID == budget.ID);

                if (existingBudget != null)
                {
                    // Remove the budget from the context
                    context.Budgets.Remove(existingBudget);

                    // Save changes to the database
                    await context.SaveChangesAsync();
                }
            }
        }
        public static List<Budget> CalculateBudgetPerMonth(DateTimeOffset month)
        {
            List<CurrencyTransaction> transactions = CurrencyTransactionService.GetTransactions(GetFirstDayOfMonth(month), GetLastDayOfMonth(month));
            List<Budget> budgets = GetAllBudgets();
            foreach (var budget in budgets) 
            {
                foreach (var transaction in transactions.Where(t => t.Budget?.ID == budget.ID))
                {
                    if(transaction.Budget == null) { 
                    continue;}
                    budget.Amount += transaction.Amount;
                }
            }
            return budgets;
        }
        public static List<Budget> CalculateIncomeExpensePerMonth(DateTimeOffset month)
        {
            List<CurrencyTransaction> transactions = CurrencyTransactionService.GetTransactions(GetFirstDayOfMonth(month), GetLastDayOfMonth(month));
            List<Budget> io = new List<Budget>
        {
            new Budget
            {
                ID = 1,
                Name = "Income",
                Description = "Incomes",
                Amount = 0
            },
            new Budget
            {
                ID = 2,
                Name = "Expense",
                Description = "Expenses",
                Amount = -0
            }
        };
            foreach(var transaction in  transactions.Where(t => t.Amount > 0))
            {
                io[0].Amount += transaction.Amount;
            }
            foreach (var transaction in transactions.Where(t => t.Amount < 0))
            {
                io[1].Amount += transaction.Amount;
            }
            io[1].Amount = io[1].Amount * -1;
            return io;
        }
        private static DateTimeOffset GetFirstDayOfMonth(DateTimeOffset month)
        {
            DateTimeOffset firstDay = month;
            while(firstDay.Month == month.Month)
            {
                firstDay = firstDay.AddDays(-1);
            }
            firstDay.AddDays(1);
            return firstDay;
        }
        private static DateTimeOffset GetLastDayOfMonth(DateTimeOffset month)
        {
            DateTimeOffset lastDay = month;
            while(lastDay.Month == month.Month)
            {
                lastDay = lastDay.AddDays(1);
            }
            lastDay.AddDays(-1);
            return lastDay;
        }

    }
}
