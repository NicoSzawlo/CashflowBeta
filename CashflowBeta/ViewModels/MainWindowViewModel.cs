using System;
using System.Collections.ObjectModel;
using CashflowBeta.ViewModels.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CashflowBeta.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;

    [ObservableProperty] private bool _isPaneOpen;

    [ObservableProperty] public MainMenuItemTemplate? _selectedMenuItem;

    public ObservableCollection<MainMenuItemTemplate> MenuItems { get; } = new()
    {
        new MainMenuItemTemplate(typeof(HomeViewModel)),
        new MainMenuItemTemplate(typeof(AccountViewModel)),
        new MainMenuItemTemplate(typeof(TransactionsViewModel)),
        new MainMenuItemTemplate(typeof(BudgetsViewModel))
    };

    //Change view from navigation menu
    partial void OnSelectedMenuItemChanged(MainMenuItemTemplate? value)
    {
        if (value == null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance == null) return;
        CurrentView = (ViewModelBase)instance;
    }

    //Trigggers navigation menu pane
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}