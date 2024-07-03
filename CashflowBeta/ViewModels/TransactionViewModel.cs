using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;

namespace CashflowBeta.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {

        public ObservableCollection<CurrencyTransaction> Transactions { get; } = new ObservableCollection<CurrencyTransaction>();

        public TransactionViewModel()
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
                Info = "Payment for services",
                Reference = "REF001",
                Account = Demoaccount,
                TransactionPartner = Demopartner
            };
            Transactions.Add(Demotransaction);
            Transactions.Add(Demotransaction2);
        }
    }
}
