using Avalonia.Controls.Primitives;
using CashflowBeta.Models;
using CashflowBeta.ViewModels.Templates;
using CashflowBeta.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Services.StatementProcessing;
using CashflowBeta.Services;
namespace CashflowBeta.ViewModels
{
    public partial class AccountViewModel : ViewModelBase
    {
        //List of accounts
        public ObservableCollection<Account> Accounts { get; set; }
        //Selected account in datagrid
        [ObservableProperty]
        private Account _selectedAccount;


        public AccountViewModel() 
        {
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                Accounts = new ObservableCollection<Account>(context.Accounts);
            }
        }

[RelayCommand]
        private void AddAccount()
        {
            var window = new AddAccountView();
            window.Show();
        }
        [RelayCommand]
        private void EditMapping()
        {
            CsvProcessing csvproc = new();
            csvproc.ProcessStatementFile(SelectedAccount.ID);
            //var window = new StatementMapView();
            //window.Show();
        }
    }
}
