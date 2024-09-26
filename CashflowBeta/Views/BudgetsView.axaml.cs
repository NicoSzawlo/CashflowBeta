using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CashflowBeta.Models;
using CashflowBeta.ViewModels;
using System.Linq;

namespace CashflowBeta.Views;

public partial class BudgetsView : UserControl
{
    public BudgetsView()
    {
        InitializeComponent();
        
    }

    private void TransactionPartnersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        var viewModel = this.DataContext as BudgetsViewModel;

        if (viewModel != null)
        {
            viewModel.SelectedPartners = dataGrid.SelectedItems.Cast<TransactionPartner>().ToList();
        }
    }

    private void Binding(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
    {
    }
}