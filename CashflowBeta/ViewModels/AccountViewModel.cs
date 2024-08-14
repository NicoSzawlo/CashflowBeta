using Avalonia.Controls.Primitives;
using CashflowBeta.Models;
using CashflowBeta.ViewModels.Templates;
using CashflowBeta.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Services;
namespace CashflowBeta.ViewModels
{
    public partial class AccountViewModel : ViewModelBase
    {
        [RelayCommand]
        private void AddAccount()
        {
            var window = new AddAccountView();
            window.Show();
        }
        [RelayCommand]
        private void ProcessStatement()
        {
            var test = new CsvProcessing();
            test.ProcessStatementFile();
        }
    }
}
