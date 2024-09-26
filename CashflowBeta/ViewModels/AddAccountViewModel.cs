using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CashflowBeta.ViewModels
{
    public partial class AddAccountViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _newAccountName;

        [ObservableProperty]
        private string _newFilepath;

        [ObservableProperty]
        private string _newBankIdentifier;

        [ObservableProperty]
        private string _newAccountIdentifier;

        [ObservableProperty]
        private string _newBalance;

        private Account newAccount = new();



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
                newAccount.Name = NewAccountName;
                newAccount.BankIdentifier = NewBankIdentifier;
                newAccount.AccountIdentifier = NewAccountIdentifier;
                newAccount.Balance = decimal.Parse(NewBalance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
