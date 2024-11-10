using Avalonia.Platform.Storage;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CashflowBeta.Services;
using CashflowBeta.Services.StatementProcessing;

namespace CashflowBeta.ViewModels
{
    public partial class AddAccountViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _newAccountName;

        [ObservableProperty]
        private string _newFilepath;
        partial void OnNewFilepathChanged(string value)
        {
            FilepathIsValid = CheckIfValidFilepath(value);
        }
        private bool CheckIfValidFilepath(string filepath)
        {
            bool conditionTrue = false;
            return File.Exists(NewFilepath);
        }
        [ObservableProperty] private bool _FilepathIsValid = false;
        
        [ObservableProperty]
        private string _newBankIdentifier;

        [ObservableProperty]
        private string _newAccountIdentifier;

        [ObservableProperty]
        private string _newBalance;
    
        private Account _newAccount = new();

        public AddAccountViewModel()
        {
            _newAccount = AccountService.AddNewAccount(_newAccount);

            NewAccountName = _newAccount.Name;
            NewBankIdentifier = _newAccount.BankIdentifier;
            NewAccountIdentifier = _newAccount.AccountIdentifier;
            NewBalance = _newAccount.Balance.ToString();
            
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

                _newAccount = AccountService.UpdateAccount(_newAccount);

                if (NewFilepath != null && NewFilepath != "")
                {
                    StatementProcessing.ProcessStatementFile(NewFilepath, _newAccount);
                }
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
                List<string> headers = StatementProcessing.GetCsvHeaders(NewFilepath);
                string headerstring = string.Join("\n", headers);
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

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Statement File",
                AllowMultiple = false
            });

            return files?.Count >= 1 ? files[0] : null;
        }
    }
}
