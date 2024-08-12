using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CashflowBeta.ViewModels
{
    public partial class TransactionViewModel : ViewModelBase
    {
        //Content of main datagrid
        public ObservableCollection<CurrencyTransaction> Transactions { get; } = new ObservableCollection<CurrencyTransaction>();
        //Content of small datagrid showing all transactions with currently selected partner
        public ObservableCollection<CurrencyTransaction> TransactionsWithPartner { get; } = new ObservableCollection<CurrencyTransaction>();
        //Selected transcation in main datagrid of view
        [ObservableProperty]
        private CurrencyTransaction _selectedTransaction;

        //Reload TransactionsWithPartner when new transaction is selected
        partial void OnSelectedTransactionChanged(CurrencyTransaction? oldValue, CurrencyTransaction newValue)
        {
            TransactionsWithPartner.Clear();
            if (SelectedTransaction != null)
            {
                var partner = newValue.TransactionPartner;
                //Select all transactions with same partner as selected in main datagrid
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

        public TransactionViewModel()
        {
            //Implement loading from DatabaseContext here
            AddDemoData();
        }

        //Write Mockdata into model-instances
        private void AddDemoData()
        {
            Account Demoaccount = new Account()
            {
                ID = 1,
                Name = "Alice Smith",
                AccountIdentifier = "ACC123456789",
                BankIdentifier = "BANK001",
                Balance = 1000.50m
            };
            TransactionPartner Demopartner = new TransactionPartner()
            {
                ID = 2,
                Name = "Eva White",
                AccountIdentifier = "TPB987654321",
                BankIdentifier = "TPBANK002",
                Bankcode = "TPCODE002"
            };
            CurrencyTransaction Demotransaction = new CurrencyTransaction()
            {
                ID = 1,
                DateTime = new DateTime(2023, 6, 1, 14, 30, 0),
                Amount = 250.00m,
                Currency = "USD",
                Info = "Payment for services",
                Reference = "REF001",
                Account = Demoaccount,
                TransactionPartner = Demopartner
            };
            CurrencyTransaction Demotransaction2 = new CurrencyTransaction()
            {
                ID = 1,
                DateTime = new DateTime(2023, 6, 1, 14, 30, 0),
                Amount = 20.00m,
                Currency = "EUR",
                Info = "Payment for cash oida",
                Reference = "REF991",
                Account = Demoaccount,
                TransactionPartner = Demopartner
            };
            // New Partner
            TransactionPartner Newpartner = new TransactionPartner()
            {
                ID = 3,
                Name = "John Doe",
                AccountIdentifier = "TPB123456789",
                BankIdentifier = "TPBANK003",
                Bankcode = "TPCODE003"
            };

            // New Transaction with Newpartner
            CurrencyTransaction Newtransaction = new CurrencyTransaction()
            {
                ID = 3,
                DateTime = new DateTime(2023, 6, 5, 10, 0, 0),
                Amount = 500.00m,
                Currency = "GBP",
                Info = "Payment for goods",
                Reference = "REF003",
                Account = Demoaccount,
                TransactionPartner = Newpartner
            };

            // Another Partner
            TransactionPartner Anotherpartner = new TransactionPartner()
            {
                ID = 4,
                Name = "Jane Smith",
                AccountIdentifier = "TPB901234567",
                BankIdentifier = "TPBANK004",
                Bankcode = "TPCODE004"
            };

            // Another Transaction with Anotherpartner
            CurrencyTransaction Anothertransaction = new CurrencyTransaction()
            {
                ID = 4,
                DateTime = new DateTime(2023, 6, 10, 12, 0, 0),
                Amount = 100.00m,
                Currency = "USD",
                Info = "Payment for services",
                Reference = "REF004",
                Account = Demoaccount,
                TransactionPartner = Anotherpartner
            };

            // Yet Another Partner
            TransactionPartner Yetanotherpartner = new TransactionPartner()
            {
                ID = 5,
                Name = "Bob Johnson",
                AccountIdentifier = "TPB111111111",
                BankIdentifier = "TPBANK005",
                Bankcode = "TPCODE005"
            };

            // Yet Another Transaction with Yetanotherpartner
            CurrencyTransaction Yetanothertransaction = new CurrencyTransaction()
            {
                ID = 5,
                DateTime = new DateTime(2023, 6, 15, 14, 0, 0),
                Amount = 300.00m,
                Currency = "EUR",
                Info = "Payment for goods",
                Reference = "REF005",
                Account = Demoaccount,
                TransactionPartner = Yetanotherpartner
            };
            Transactions.Add(Demotransaction);
            Transactions.Add(Demotransaction2);
            Transactions.Add(Newtransaction);
            Transactions.Add(Anothertransaction);
            Transactions.Add(Yetanothertransaction);
        }
    }
}
