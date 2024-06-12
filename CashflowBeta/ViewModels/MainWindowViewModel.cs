using CashflowBeta.Services;
using ReactiveUI;
using System.Transactions;
namespace CashflowBeta.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;
        public TransactionViewModel TransactionsVm { get; }
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public MainWindowViewModel()
        {
            TransactionsVm = new TransactionViewModel();
            _contentViewModel = TransactionsVm;
        }
        
    }
}
