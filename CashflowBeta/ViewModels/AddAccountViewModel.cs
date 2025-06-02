using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.StatementProcessing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Primitives;

namespace CashflowBeta.ViewModels;

public partial class AddAccountViewModel : ViewModelBase
{
    [ObservableProperty] private bool _FilepathIsValid;

    private Account? _newAccount = new();

    [ObservableProperty] private string _newAccountIdentifier;

    [ObservableProperty] private string _newAccountName;

    [ObservableProperty] private string _newBalance;

    [ObservableProperty] private string _newBankIdentifier;

    [ObservableProperty] private string? _newFilepath;

    private readonly AccountService _accountService;
    private readonly StatementProcessingService _statementProcessing;
    public AddAccountViewModel(AccountService accountService, StatementProcessingService statementProcessing)
    {
        _accountService = accountService;
        _statementProcessing = statementProcessing;
        _newAccount = _accountService.AddNewAccount(_newAccount);

        NewAccountName = _newAccount.Name;
        NewBankIdentifier = _newAccount.BankIdentifier;
        NewAccountIdentifier = _newAccount.AccountIdentifier;
        NewBalance = _newAccount.Balance.ToString();
    }

    partial void OnNewFilepathChanged(string? value)
    {
        FilepathIsValid = CheckIfValidFilepath(value);
    }

    private bool CheckIfValidFilepath(string? filepath)
    {
        var conditionTrue = false;
        return File.Exists(NewFilepath);
    }

    [RelayCommand]
    private async Task SelectStatementFile(CancellationChangeToken token)
    {
        try
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            await file.OpenReadAsync();
            NewFilepath = file.TryGetLocalPath();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private void ConfirmInput()
    {
        try
        {
            _newAccount.Name = NewAccountName;
            _newAccount.BankIdentifier = NewBankIdentifier;
            _newAccount.AccountIdentifier = NewAccountIdentifier;
            _newAccount.Balance = decimal.Parse(NewBalance);

            _newAccount = _accountService.UpdateAccount(_newAccount);

            if (NewFilepath != null && NewFilepath != "")
                _statementProcessing.ProcessStatementFile(NewFilepath, _newAccount);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private void EditMapping()
    {
        if (FilepathIsValid)
        {
            var headers = StatementProcessingService.GetCsvHeaders(NewFilepath);
            var headerstring = string.Join("\n", headers);
            var window = new StatementMapView();
            window.DataContext = new StatementMapViewModel(_newAccount, headerstring);
            window.Show();
        }
    }

    private async Task<IStorageFile?> DoOpenFilePickerAsync()
    {
        // For learning purposes, we opted to directly get the reference
        // for StorageProvider APIs here inside the ViewModel. 

        // For your real-world apps, you should follow the MVVM principles
        // by making service classes and locating them with DI/IoC.

        // See IoCFileOps project for an example of how to accomplish this.
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Statement File",
            AllowMultiple = false
        });

        return files?.Count >= 1 ? files[0] : null;
    }
}