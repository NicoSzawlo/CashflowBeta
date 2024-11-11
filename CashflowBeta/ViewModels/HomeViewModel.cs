using System;
using System.Collections.Generic;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CashflowBeta.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty] private DateTimeOffset _budgetMonth;

    public HomeViewModel()
    {
        NetworthService.AddNetworth();
        BudgetMonth = new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0));
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        BudgetMonth = BudgetMonth.AddMonths(-1);
        UpdateBudgetGraph(BudgetMonth);
        UpdateInOutGraph(BudgetMonth);
    }

    [RelayCommand]
    private void NextMonth()
    {
        BudgetMonth = BudgetMonth.AddMonths(1);
        UpdateBudgetGraph(BudgetMonth);
        UpdateInOutGraph(BudgetMonth);
    }

    [RelayCommand]
    private static void ReloadNetworthTrend()
    {
        NetworthService.RecalculateNetworthTrends();
        UpdateNetworthChart();
    }


    public static void RequestUpdate(DateTimeOffset month)
    {
        UpdateNetworthChart();
        UpdateBudgetGraph(month);
        UpdateInOutGraph(month);
    }

    private static void UpdateNetworthChart()
    {
        List<List<Networth>> networthTrends = [NetworthService.GetNetworthTrend()];

        var accounts = AccountService.GetAllAccounts();
        foreach (var acc in accounts) networthTrends.Add(NetworthService.GetNetworthTrend(acc));

        WeakReferenceMessenger.Default.Send(new NetworthTrendsLoadedMessage(networthTrends));
    }

    private static void UpdateBudgetGraph(DateTimeOffset month)
    {
        var budgets = BudgetService.CalculateBudgetPerMonth(month);
        WeakReferenceMessenger.Default.Send(new BudgetGraphLoadedMessage(budgets));
    }

    private static void UpdateInOutGraph(DateTimeOffset month)
    {
        var budgets = BudgetService.CalculateIncomeExpensePerMonth(month);
        WeakReferenceMessenger.Default.Send(new InOutGraphLoadedMessage(budgets));
    }
}