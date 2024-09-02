using Avalonia.Controls;
using CashflowBeta.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.TickGenerators;
using System;

namespace CashflowBeta.Views
{
    public partial class HomeView : UserControl
    {
        AvaPlot networthPlot;
        public HomeView()
        {
            InitializeComponent();

            //Get handle of networthplo
            networthPlot = this.Find<AvaPlot>("NetworthPlot");

            //Settings for display of plot
            //############################################################

            // setup the bottom axis to use DateTime ticks
            var axis = networthPlot.Plot.Axes.DateTimeTicksBottom();

            // create a custom formatter to return a string with
            // date only when zoomed out and time only when zoomed in
            static string CustomFormatter(DateTime dt)
            {
                bool isMidnight = dt is { Hour: 0, Minute: 0, Second: 0 };
                return isMidnight
                    ? DateOnly.FromDateTime(dt).ToString()
                    : TimeOnly.FromDateTime(dt).ToString();
            }
            // apply the custom tick formatter
            DateTimeAutomatic tickGen = (DateTimeAutomatic)axis.TickGenerator;
            tickGen.LabelFormatter = CustomFormatter;

            //Request data from viewmodel an refresh plot
            networthPlot.Plot.Add.ScatterLine(HomeViewModel.GetOverallNetworthCoordinates());
            networthPlot.Refresh();
        }

    }
}