using CashflowBeta.Models;
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
        public StatementMapViewModel()
        {
            // Request the currently selected account on opening the window
            SelectedAccount = WeakReferenceMessenger.Default.Send<Services.Messages.SelectedAccountRequestMessage>();
            CsvMap = CurrencyTransactionCsvMap.LoadMapForAccount(SelectedAccount.ID);
        }
        [RelayCommand]
        private void SaveMap()
        {
            CurrencyTransactionCsvMap.SaveMapForAccount(SelectedAccount.ID, CsvMap);
        }
    }
}
