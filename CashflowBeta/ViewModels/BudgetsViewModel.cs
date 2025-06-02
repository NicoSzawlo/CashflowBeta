using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CashflowBeta.ViewModels;

public partial class BudgetsViewModel : ViewModelBase
{
    [ObservableProperty] private Budget _selectedBudget;

    [ObservableProperty] private decimal _selectedBudgetAmount;

    [ObservableProperty] private TransactionPartner _selectedPartner;

    private readonly AppDataStore _appDataStore;
    private readonly BudgetService _budgetService;
    private readonly CurrencyTransactionService _currencyTransactionService;
    private readonly TransactionPartnerService _transactionPartnerService;

    public BudgetsViewModel(
        AppDataStore appDataStore,
        BudgetService budgetService,
        CurrencyTransactionService currencyTransactionService,
        TransactionPartnerService transactionPartnerService)
    {
        //Inject dependencies
        _appDataStore = appDataStore;
        _budgetService = budgetService;
        _currencyTransactionService = currencyTransactionService;
        _transactionPartnerService = transactionPartnerService;

        TransactionPartners = _appDataStore.TransactionPartners;
        Budgets = _appDataStore.Budgets;
        if (Budgets.Count > 0) SelectedBudget = Budgets.First();
    }

    public ObservableCollection<TransactionPartner> TransactionPartners { get; set; }
    public List<TransactionPartner> SelectedPartners { get; set; } = new();
    public ObservableCollection<Budget> Budgets { get; } = new();

    [RelayCommand]
    private async Task AddNewBudget()
    {
        Budget budget = new() { Name = "New Budget", Amount = 0, Description = "This is a new Budget" };
        await _budgetService.UpdateBudgetAsync(budget);
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        foreach (var budget in Budgets) await _budgetService.UpdateBudgetAsync(budget);
    }

    [RelayCommand]
    private async Task ApplyBudgetToPartner()
    {
        await _transactionPartnerService.ApplyBudgetToPartnersAsync(SelectedBudget, SelectedPartners);
    }

    [RelayCommand]
    private async Task DeleteBudget()
    {
        await _budgetService.RemoveBudgetAsync(SelectedBudget);
        Budgets.Remove(SelectedBudget);
    }
}