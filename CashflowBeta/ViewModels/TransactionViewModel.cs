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
        ObservableCollection<CurrencyTransaction> Transactions { get; }
    }
}
