using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CashflowBeta.ViewModels
{
    public partial class TransactionsViewModel : ViewModelBase
    {
        //Content of main datagrid
        public ObservableCollection<CurrencyTransaction> Transactions { get; } = new ObservableCollection<CurrencyTransaction>();
        //Content of small datagrid showing all transactions with currently selected partner
        public ObservableCollection<CurrencyTransaction> TransactionsWithPartner { get; } = new ObservableCollection<CurrencyTransaction>();
        //Selected transcation in main datagrid of view
        [ObservableProperty]
        private CurrencyTransaction _selectedTransaction;

        public TransactionsViewModel()
        {
            //Load transactions from database
            Transactions = new ObservableCollection<CurrencyTransaction>(
                CurrencyTransactionService.GetTransactions()
                .OrderByDescending(item => item.DateTime).ToList());
        }

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
                foreach (var transaction in transactionsWithPartner)
                {
                    TransactionsWithPartner.Add(transaction);
                }
            }
        }

        
    }
}
