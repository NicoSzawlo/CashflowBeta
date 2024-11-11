using System.Linq;
using Avalonia;
using Avalonia.Controls;
using CashflowBeta.Models;
using CashflowBeta.ViewModels;

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
        var viewModel = DataContext as BudgetsViewModel;

        if (viewModel != null) viewModel.SelectedPartners = dataGrid.SelectedItems.Cast<TransactionPartner>().ToList();
    }

    private void Binding(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
    }
}