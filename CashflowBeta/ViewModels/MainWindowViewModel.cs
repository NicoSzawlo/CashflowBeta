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

namespace CashflowBeta.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;
    [ObservableProperty] private bool _dataLoaded;

    [ObservableProperty] private bool _isPaneOpen;

    [ObservableProperty] public MainMenuItemTemplate? _selectedMenuItem;

    public MainWindowViewModel()
    {
        DataLoaded = false;
        var instance = Activator.CreateInstance(typeof(ApplicationLoadingViewModel));
        CurrentView = (ViewModelBase)instance;
        Task.Run(async () => LoadApplicationData());
        RegisterMessages();
        
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

    private async Task LoadApplicationData()
    {
        //Load all relevant data
        Transactions = CurrencyTransactionService.GetTransactions();
        Partners = TransactionPartnerService.GetAllPartners();
        Accounts = AccountService.GetAllAccounts();
        NetworthTrends = NetworthService.GetNetworthTrend();
        Budgets = BudgetService.GetAllBudgets();
        //Set data loaded signal
        DataLoaded = true;

        //Change view to home view
        var instance = Activator.CreateInstance(typeof(HomeViewModel));
        CurrentView = (ViewModelBase)instance;
    }

    private void RegisterMessages()
    {
        //Register transaction request message
        WeakReferenceMessenger.Default.Register<MainWindowViewModel, CurrencyTransactionsRequestMessage>(this, (r, m) =>
        {
            m.Reply(r.Transactions);
        });
    }

    //Change view from navigation menu
    partial void OnSelectedMenuItemChanged(MainMenuItemTemplate? value)
    {
        if (DataLoaded)
        {
            if (value == null) return;
            var instance = Activator.CreateInstance(value.ModelType);
            if (instance == null) return;
            CurrentView = (ViewModelBase)instance;
        }
        else
        {
            var instance = Activator.CreateInstance(typeof(ApplicationLoadingViewModel));
            CurrentView = (ViewModelBase)instance;
            SelectedMenuItem = null;
        }
    }

    //Trigggers navigation menu pane
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}