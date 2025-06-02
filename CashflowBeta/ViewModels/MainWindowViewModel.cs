using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CashflowBeta.ViewModels.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace CashflowBeta.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;
    [ObservableProperty] private bool _dataLoaded;

    [ObservableProperty] private bool _isPaneOpen;

    [ObservableProperty] public MainMenuItemTemplate? _selectedMenuItem;
    private readonly AppStatusService _appStatusService;

    public MainWindowViewModel(AppStatusService appStatusService)
    {
        _appStatusService = appStatusService;
        var instance = Activator.CreateInstance(typeof(ApplicationLoadingViewModel));
        CurrentView = (ViewModelBase)instance;
        Task.Run(async () => LoadApplicationData());

    }

    private List<CurrencyTransaction> Transactions { get; set; }
    private List<TransactionPartner> Partners { get; set; }
    private List<Account> Accounts { get; set; }
    private List<Networth> NetworthTrends { get; set; }
    private List<Budget> Budgets { get; set; }

    public ObservableCollection<MainMenuItemTemplate> MenuItems { get; } = new()
    {
        new MainMenuItemTemplate(typeof(HomeViewModel)),
        new MainMenuItemTemplate(typeof(AccountViewModel)),
        new MainMenuItemTemplate(typeof(TransactionsViewModel)),
        new MainMenuItemTemplate(typeof(BudgetsViewModel)),
        new MainMenuItemTemplate(typeof(PartnersViewModel))
    };

    private AppStatusService AppStatus { get; }

    private async Task LoadApplicationData()
    {
        /* //Load all relevant data
        if (AppStatus.HasAnyAccount)
        {
            Accounts = AccountService.GetAllAccounts();
            if (AppStatus.HasAnyTransaction)
            { 
                Transactions = CurrencyTransactionService.GetTransactions();
                Partners = TransactionPartnerService.GetAllPartners();
            }
        }
        if (AppStatus.HasAnyNetworthtrend)
        {
            NetworthTrends = NetworthService.GetNetworthTrend();    
        }
        if (AppStatus.HasAnyBudgets)
        {
            Budgets = BudgetService.GetAllBudgets();   
        }
        //Set data loaded signal
        DataLoaded = true; */

        //Change view to home view
        var instance = Activator.CreateInstance(typeof(AccountViewModel));
        CurrentView = (ViewModelBase)instance;
    }
    
    //Change view from navigation menu
    partial void OnSelectedMenuItemChanged(MainMenuItemTemplate? value)
    {
        // if (!DataLoaded)
        // {
        //     CurrentView = App.Services.GetRequiredService<ApplicationLoadingViewModel>();
        //     SelectedMenuItem = null;
        //     return;
        // }

        if (value?.ModelType == null)
            return;

        var instance = App.Services.GetRequiredService(value.ModelType);
        CurrentView = (ViewModelBase)instance;
    }

    //Trigggers navigation menu pane
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}