using CashflowBeta.Models;
using CashflowBeta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.ViewModels
{
    public partial class BudgetsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private TransactionPartner _selectedPartner;

        [ObservableProperty]
        private Budget _selectedBudget;

        [ObservableProperty]
        private decimal _selectedBudgetAmount;
        public ObservableCollection<TransactionPartner> TransactionPartners { get; } = new ObservableCollection<TransactionPartner>();
        public List<TransactionPartner> SelectedPartners { get; set; } = new();
        public ObservableCollection<Budget> Budgets { get; } = new ObservableCollection<Budget>();

        public BudgetsViewModel()
        {
            TransactionPartners = new ObservableCollection<TransactionPartner>(TransactionPartnerService.GetAllPartners());
            Budgets = new ObservableCollection<Budget>(BudgetService.GetAllBudgets());
            if (Budgets.Count > 0)
            {
                SelectedBudget = Budgets.First();
            }

        }

        [RelayCommand]
        private void AddNewBudget()
        {
            Budgets.Add(new Budget() { Name = "New Budget", Amount = 0, Description = "This is a new Budget" });
        }
        [RelayCommand]
        private async Task SaveChanges()
        {
            foreach (var budget in Budgets) 
            {
                await BudgetService.UpdateBudgetAsync(budget);
            }
        }
        [RelayCommand]
        private async Task ApplyBudgetToPartner()
        {
            foreach(var partner in SelectedPartners)
            {
                partner.Budget = SelectedBudget;
            }
            await TransactionPartnerService.ApplyBudgetToPartnersAsync(SelectedBudget, SelectedPartners);
        }
        [RelayCommand]
        private async Task DeleteBudget() 
        {
            await BudgetService.RemoveBudgetAsync(SelectedBudget);
            Budgets.Remove(SelectedBudget);
        }
    }
}
