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
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
namespace CashflowBeta.ViewModels
{
    public partial class AccountViewModel : ViewModelBase
    {
        public AccountViewModel() 
        {
            //Load Accounts from database
            using (var context = new CashflowContext())
            {
                Accounts = new ObservableCollection<Account>(context.Accounts);
            }
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


        [RelayCommand]
        private void AddAccount()
        {
            var window = new AddAccountView();
            window.Show();
        }
        [RelayCommand]
        private void EditMapping()
        {
            var window = new StatementMapView();
            window.DataContext = new StatementMapViewModel();
            window.Show();
        }
    }
}
