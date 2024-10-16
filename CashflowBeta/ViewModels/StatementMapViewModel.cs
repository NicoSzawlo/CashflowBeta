﻿using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.StatementProcessing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.ViewModels
{
    public partial class StatementMapViewModel : ViewModelBase
    {
        [ObservableProperty]
        private CurrencyTransactionCsvMap _csvMap;

        [ObservableProperty]
        private Account _selectedAccount;
        public StatementMapViewModel(Account account)
        {
            if(account != null)
            {
                SelectedAccount = account;
            }
            else
            {
                // Request the currently selected account on opening the window
                SelectedAccount = WeakReferenceMessenger.Default.Send<Services.Messages.SelectedAccountRequestMessage>();
            }

            CsvMap = FileService.LoadMapForAccount(SelectedAccount.ID);
        }
        [RelayCommand]
        private void SaveMap()
        {
            FileService.SaveMapForAccount(SelectedAccount.ID, CsvMap);
        }
    }
}
