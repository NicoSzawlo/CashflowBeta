using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CashflowBeta.Services.StatementProcessing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace CashflowBeta.ViewModels;

public partial class StatementMapViewModel : ViewModelBase
{
    [ObservableProperty] private CurrencyTransactionCsvMap _csvMap;

    [ObservableProperty] private string _headers;
    [ObservableProperty] private bool _headersAvailable;

    [ObservableProperty] private Account _selectedAccount;

    public StatementMapViewModel(Account? account, string headers)
    {
        if (headers != null)
        {
            Headers = headers;
            HeadersAvailable = true;
        }

        if (account != null)
            SelectedAccount = account;
        else
            // Request the currently selected account on opening the window
            SelectedAccount =
                WeakReferenceMessenger.Default.Send<SelectedAccountRequestMessage>();

        CsvMap = FileService.LoadMapForAccount(SelectedAccount.ID);
    }

    [RelayCommand]
    private void SaveMap()
    {
        FileService.SaveMapForAccount(SelectedAccount.ID, CsvMap);
    }
}