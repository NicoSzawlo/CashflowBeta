using System.Collections.ObjectModel;
using CashflowBeta.Models;

namespace CashflowBeta.Services
{
    public class AppDataStore
    {
        public ObservableCollection<Account> Accounts { get; } = new();
        public ObservableCollection<CurrencyTransaction> CurrencyTransactions { get; } = new();
        public ObservableCollection<Budget> Budgets { get; } = new();
        public ObservableCollection<AssetTransaction> AssetTransactions { get; } = new();
        public ObservableCollection<Asset> Assets { get; } = new();
        public ObservableCollection<Networth> Networths { get; } = new();
        public ObservableCollection<TransactionPartner> TransactionPartners { get; } = new();

        // Clears all collections
        public void ClearAll()
        {
            Accounts.Clear();
            CurrencyTransactions.Clear();
            Budgets.Clear();
            AssetTransactions.Clear();
            Assets.Clear();
            Networths.Clear();
            TransactionPartners.Clear();
        }
    }
}