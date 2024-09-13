using Avalonia.Controls;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CashflowBeta.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.TickGenerators;
using System;
using System.Linq;

namespace CashflowBeta.Views
{
    public partial class HomeView : UserControl
    {
        AvaPlot networthPlot;
        public HomeView()
        {
            InitializeComponent();

            networthPlot = this.Find<AvaPlot>("NetworthPlot");


            WeakReferenceMessenger.Default.Register<NetworthTrendsLoadedMessage>(this, (r, m) =>
            {
                //Setup networth chart with chartservice
                networthPlot = ChartService.UpdateNetworthPlot(networthPlot, m.Value);
                //Refresh plot
                networthPlot.Refresh();
            });
            HomeViewModel.RequestUpdate();
        }

    }
}