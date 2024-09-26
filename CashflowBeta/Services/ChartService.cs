using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mysqlx.Crud;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators;

namespace CashflowBeta.Services
{
    public class ChartService
    {
        public static AvaPlot UpdateNetworthPlot(AvaPlot plot, List<List<Networth>> networthTrends)
        {
            //Setup datetime X-Axis
            plot = SetupDateTimeAxisFormat(plot);

            //Add total networth trendline to plot, always first in list
            Scatter totalNetworth = plot.Plot.Add.ScatterLine(GenerateCoordinates(networthTrends[0]));
            totalNetworth.LineWidth = 3;
            totalNetworth.Color = Colors.Blue;
            totalNetworth.LegendText = "Total Networth";

            if (networthTrends.Count > 2) 
            {
                //Add individual account trendlines to plot

                foreach (var trend in networthTrends)
                {
                    //Except first item in networthtrends as its not acc specific
                    if (trend != networthTrends[0])
                    {
                        var accLine = plot.Plot.Add.ScatterLine(GenerateCoordinates(trend));
                        accLine.LineWidth = 2;
                        accLine.LegendText = trend[0].Account.Name;
                    }
                }
            }
            
            plot.Plot.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            return plot;
        }
        public static async Task<AvaPlot> UpdateNetworthPlotAsync(AvaPlot plot, List<List<Networth>> networthTrends)
        {
            //Setup datetime X-Axis
            plot = SetupDateTimeAxisFormat(plot);

            //Add total networth trendline to plot, always first in list
            Scatter totalNetworth = plot.Plot.Add.ScatterLine(GenerateCoordinates(networthTrends[0]));
            totalNetworth.LineWidth = 3;
            totalNetworth.Color = Colors.Blue;
            totalNetworth.LegendText = "Total Networth";

            //Add individual account trendlines to plot
            foreach (var trend in networthTrends)
            {
                //Except first item in networthtrends as its not acc specific
                if (trend != networthTrends[0])
                {
                    var accLine = plot.Plot.Add.ScatterLine(GenerateCoordinates(trend));
                    accLine.LineWidth = 2;
                    accLine.LegendText = trend[0].Account.Name;
                }
            }
            return plot;
        }
        //Generate Coordinate-Array for XY-Scatter Graph
        private static Coordinates[] GenerateCoordinates(List<Networth> networthTrend)
        {
            Coordinates[] coordinates = new Coordinates[networthTrend.Count];

            for (int i = 0; i < coordinates.Length; i++)
            {
                //Convert values to double for Coordinate
                coordinates[i] = new Coordinates(
                    Convert.ToDouble(networthTrend[i].DateTime.ToOADate()), 
                    Convert.ToDouble(networthTrend[i].Capital));
            }
            return coordinates;
        }
        //Setup X-Axis to be DateTime Format
        private static AvaPlot SetupDateTimeAxisFormat(AvaPlot plot)
        {
            // setup the bottom axis to use DateTime ticks
            var axis = plot.Plot.Axes.DateTimeTicksBottom();

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

            return plot;
        }
        public static AvaPlot UpdateBudgetPlot(AvaPlot plot, List<Budget> budgets) 
        {
            int poscounter = 0;
            foreach (var budget in budgets)
            {
                var bar = plot.Plot.Add.Bar(new Bar()
                {
                    Label = budget.Name,
                    Value = Convert.ToDouble(budget.Amount),
                    Position = poscounter,
                    FillColor = Color.RandomHue()
                });

                poscounter++;
            }
            plot.Plot.Axes.Margins(bottom: 0);
            return plot;
        }
        public static AvaPlot UpdateInOutPlot(AvaPlot plot, List<Budget> budgets)
        {
            var bar1 = plot.Plot.Add.Bar(new Bar()
            {
                Label = budgets[0].Name,
                Value = Convert.ToDouble(budgets[0].Amount),
                Position = budgets[0].ID,
                FillColor = Colors.Green
            });
            var bar2 = plot.Plot.Add.Bar(new Bar()
            {
                Label = budgets[1].Name,
                Value = Convert.ToDouble(budgets[1].Amount),
                Position = budgets[1].ID,
                FillColor = Colors.Red
            });
            plot.Plot.Axes.Margins(bottom: 0);
            return plot;
        }
    }
}
