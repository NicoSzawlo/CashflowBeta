﻿using Avalonia.Controls.Primitives;
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
            if (Accounts.Count > 0)
            {
                SelectedAccount = Accounts.First();
            }
            //NetworthService.AddNetworth();
            //NetworthService.AddNetworth(Accounts[0]);
            //NetworthService.AddNetworth(Accounts[1]);
        }

        //List of accounts
        public ObservableCollection<Account> Accounts { get; set; }
        //Selected account in datagrid
        [ObservableProperty]
        private Account _selectedAccount;
        [ObservableProperty]
        private string _statementPath;

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
            var window = new StatementMapView();
            window.DataContext = new StatementMapViewModel(SelectedAccount);
            window.Show();
        }
        [RelayCommand]
        private void AddStatement()
        {
            StatementProcessing.ProcessStatementFile(StatementPath,SelectedAccount);
        }
    }
}
