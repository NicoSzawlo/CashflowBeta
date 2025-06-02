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

    private readonly AppDataStore _appDataStore;
    private readonly BudgetService _budgetService;
    public HomeViewModel(AppDataStore appDataStore, BudgetService budgetService)
    {
        _appDataStore = appDataStore;
        _budgetService = budgetService;
        //NetworthService.AddNetworth();
        //BudgetMonth = new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0));
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


    public void RequestUpdate(DateTimeOffset month)
    {
        UpdateNetworthChart();
        UpdateBudgetGraph(month);
        UpdateInOutGraph(month);
    }

    private static void UpdateNetworthChart()
    {
        List<List<Networth>> networthTrends = [NetworthService.GetNetworthTrend()];

        //var accounts = AccountService.GetAllAccounts();
        //foreach (var acc in accounts) networthTrends.Add(NetworthService.GetNetworthTrend(acc));

        WeakReferenceMessenger.Default.Send(new NetworthTrendsLoadedMessage(networthTrends));
    }

    private void UpdateBudgetGraph(DateTimeOffset month)
    {
        var budgets = _budgetService.CalculateBudgetPerMonth(month);
        WeakReferenceMessenger.Default.Send(new BudgetGraphLoadedMessage(budgets));
    }

    private void UpdateInOutGraph(DateTimeOffset month)
    {
        var budgets = _budgetService.CalculateIncomeExpensePerMonth(month);
        WeakReferenceMessenger.Default.Send(new InOutGraphLoadedMessage(budgets));
    }
}