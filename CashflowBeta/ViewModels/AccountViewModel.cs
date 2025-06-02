using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CashflowBeta.Services.StatementProcessing;
using CashflowBeta.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CashflowBeta.ViewModels;

public partial class AccountViewModel : ViewModelBase
{
    [ObservableProperty] private bool _accountSelected;
    [ObservableProperty] private bool _filepathIsValid;

    [ObservableProperty] private string? _newFilepath;

    //Selected account in datagrid
    [ObservableProperty] private Account? _selectedAccount;
    private readonly AppDataStore _appDataStore;
    private readonly AccountService _accountService;
    private readonly StatementProcessingService _statementProcessing;
    public AccountViewModel(AppDataStore appDataStore, AccountService accountService, StatementProcessingService statementProcessing)
    {
        _appDataStore = appDataStore;
        _accountService = accountService;
        _statementProcessing = statementProcessing;
        //Load Accounts from database
        Accounts = appDataStore.Accounts;
        //Register request message for selected account
        WeakReferenceMessenger.Default.Register<AccountViewModel, SelectedAccountRequestMessage>(this, (r, m) =>
        {
            //Reply with currently selected account
            m.Reply(r.SelectedAccount);
        });
    }

    //List of accounts
    public ObservableCollection<Account?> Accounts { get; set; }

    partial void OnSelectedAccountChanged(Account? value)
    {
        AccountSelected = value != null;
    }

    partial void OnNewFilepathChanged(string? value)
    {
        FilepathIsValid = CheckIfValidFilepath(value);
    }

    private bool CheckIfValidFilepath(string? filepath)
    {
        if (File.Exists(filepath) && SelectedAccount != null)
            return true;
        return false;
    }

    [RelayCommand]
    private void AddAccount()
    {
        AddAccountViewModel addAccountViewModel = new AddAccountViewModel(_accountService, _statementProcessing);
        var window = new AddAccountView();
        window.DataContext = addAccountViewModel;
        window.Show();
    }

    [RelayCommand]
    private void EditMapping()
    {
        var headerstring = "";

        if (FilepathIsValid)
        {
            var headers = StatementProcessingService.GetCsvHeaders(NewFilepath);
            headerstring = string.Join("\n", headers);
        }

        var window = new StatementMapView();
        window.DataContext = new StatementMapViewModel(SelectedAccount, headerstring);
        window.Show();
    }

    [RelayCommand]
    private void AddStatement()
    {
        _statementProcessing.ProcessStatementFile(NewFilepath, SelectedAccount);
    }

    [RelayCommand]
    private async Task DeleteAccount()
    {
        Account? selected = new();
        selected = SelectedAccount;
        Accounts.Remove(selected);
        await _accountService.DeleteAccount(selected);
    }
}