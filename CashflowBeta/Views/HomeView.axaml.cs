using System;
using Avalonia.Controls;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CashflowBeta.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using ScottPlot.Avalonia;

namespace CashflowBeta.Views;

public partial class HomeView : UserControl
{
    private AvaPlot budgetPlot;
    private AvaPlot inOutPlot;
    private AvaPlot networthPlot;

    public HomeView()
    {
        InitializeComponent();

        networthPlot = this.Find<AvaPlot>("NetworthPlot");
        budgetPlot = this.Find<AvaPlot>("BudgetPlot");
        inOutPlot = this.Find<AvaPlot>("InOutPlot");

        WeakReferenceMessenger.Default.Register<NetworthTrendsLoadedMessage>(this, async (r, m) =>
        {
            //Setup networth chart with chartservice
            networthPlot = ChartService.UpdateNetworthPlot(networthPlot, m.Value);
            //Refresh plot
            networthPlot.Refresh();
        });
        WeakReferenceMessenger.Default.Register<BudgetGraphLoadedMessage>(this, async (r, m) =>
        {
            budgetPlot.Plot.Clear();
            budgetPlot = ChartService.UpdateBudgetPlot(budgetPlot, m.Value);

            budgetPlot.Refresh();
        });
        WeakReferenceMessenger.Default.Register<InOutGraphLoadedMessage>(this, async (r, m) =>
        {
            inOutPlot.Plot.Clear();
            inOutPlot = ChartService.UpdateInOutPlot(inOutPlot, m.Value);

            inOutPlot.Refresh();
        });
        DataContextChanged += HomeView_DataContextChanged;
    }

    private void HomeView_DataContextChanged(object? sender, EventArgs e)
    {
        // Access the ViewModel from DataContext after it's set
        //if (DataContext is HomeViewModel vm)
            // Now you can safely call the static method
            //HomeViewModel.RequestUpdate(vm.BudgetMonth);
    }
}