using CashflowBeta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System;
using System.Transactions;
using CashflowBeta.ViewModels.Templates;
namespace CashflowBeta.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private ViewModelBase _currentView;

        public ObservableCollection<MainMenuItemTemplate> MenuItems { get; } = new()
        {
            new MainMenuItemTemplate(typeof(HomeViewModel)),
            new MainMenuItemTemplate(typeof(AccountViewModel)),
            new MainMenuItemTemplate(typeof(TransactionViewModel))
        };
        [ObservableProperty]
        public MainMenuItemTemplate? _selectedMenuItem;

        public MainWindowViewModel()
        {
            //Initialising application to homeview
            CurrentView = new HomeViewModel();
            SelectedMenuItem = MenuItems[0];
        }

        //Change view from navigation menu
        partial void OnSelectedMenuItemChanged(MainMenuItemTemplate? value)
        {
            if (value == null)
            {
                return;
            }
            var instance = Activator.CreateInstance(value.ModelType);
            if (instance == null)
            {
                return;
            }
            CurrentView = (ViewModelBase)instance;
        }
        //Trigggers navigation menu pane
        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }
}