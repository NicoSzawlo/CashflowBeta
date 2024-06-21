using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        ObservableCollection<Account> Accounts { get; }
    }
}
