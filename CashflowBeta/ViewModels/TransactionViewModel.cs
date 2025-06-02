using System.Collections.ObjectModel;
using System.Linq;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace CashflowBeta.ViewModels;

public partial class TransactionsViewModel : ViewModelBase
{
    //Selected transcation in main datagrid of view
    [ObservableProperty] private CurrencyTransaction _selectedTransaction;
    private readonly AppDataStore _appDataStore;
    public TransactionsViewModel(AppDataStore appDataStore)
    {
        _appDataStore = appDataStore;
        Transactions = _appDataStore.CurrencyTransactions;
    }

    //Content of main datagrid
    public ObservableCollection<CurrencyTransaction> Transactions { get; }

    //Content of small datagrid showing all transactions with currently selected partner
    public ObservableCollection<CurrencyTransaction> TransactionsWithPartner { get; } = new();

    //Reload TransactionsWithPartner when new transaction is selected
    partial void OnSelectedTransactionChanged(CurrencyTransaction? oldValue, CurrencyTransaction newValue)
    {
        TransactionsWithPartner.Clear();
        if (SelectedTransaction != null)
        {
            //Get partner and account of selected transaction
            var partner = newValue.TransactionPartner;
            //Select all transactions with same partner and account as selected in main datagrid
            var transactionsWithPartner = Transactions
                .Where(t => t.TransactionPartner.ID == partner.ID)
                .ToList();
            //Fill collection
            foreach (var transaction in transactionsWithPartner) TransactionsWithPartner.Add(transaction);
        }
    }
}