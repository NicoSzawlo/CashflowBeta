using Avalonia.Controls.Primitives;
using CashflowBeta.Models;
using CashflowBeta.ViewModels.Templates;
using CashflowBeta.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Services.StatementProcessing;
using CashflowBeta.Services;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
namespace CashflowBeta.ViewModels
{
    public partial class AccountViewModel : ViewModelBase
    {

        public AccountViewModel() 
        {
            //Load Accounts from database
            Accounts = new ObservableCollection<Account>(AccountService.GetAllAccounts());
            //Register request message for selected account
            WeakReferenceMessenger.Default.Register<AccountViewModel, Services.Messages.SelectedAccountRequestMessage>(this, (r, m) =>
            {
                //Reply with currently selected account
                m.Reply(r.SelectedAccount);
            });
        }

        //List of accounts
        public ObservableCollection<Account> Accounts { get; set; }
        //Selected account in datagrid
        [ObservableProperty]
        private Account _selectedAccount;

        partial void OnSelectedAccountChanged(Account value)
        {
            AccountSelected = (value != null);
        }

        [ObservableProperty] private bool _accountSelected = false;
        
        [ObservableProperty]
        private string _newFilepath;
        partial void OnNewFilepathChanged(string value)
        {
            FilepathIsValid = CheckIfValidFilepath(value);
        }
        private bool CheckIfValidFilepath(string filepath)
        {
            if (File.Exists(filepath) && SelectedAccount != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [ObservableProperty] private bool _filepathIsValid = false;

        [RelayCommand]
        private void AddAccount()
        {
            AddAccountViewModel addAccountViewModel = new();
            var window = new AddAccountView();
            window.DataContext = addAccountViewModel;
            window.Show();
        }
        [RelayCommand]
        private void EditMapping()
        {
            string headerstring = "";
            
            if (FilepathIsValid)
            {
                List<string> headers = StatementProcessing.GetCsvHeaders(NewFilepath);
                headerstring = string.Join("\n", headers);
            }
            
            var window = new StatementMapView();
            window.DataContext = new StatementMapViewModel(SelectedAccount, headerstring);
            window.Show();
        }
        [RelayCommand]
        private void AddStatement()
        {
            StatementProcessing.ProcessStatementFile(NewFilepath,SelectedAccount);
        }

        [RelayCommand]
        private async Task DeleteAccount()
        {
            Account selected = new();
            selected = SelectedAccount;
            Accounts.Remove(selected);
            await AccountService.DeleteAccount(selected);
            
        }
    }
}
