using Avalonia.Controls.Shapes;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CashflowBeta.Services.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        public HomeViewModel() 
        {
            
        }

        public static void RequestUpdate()
        {
            UpdateNetworthChart();
        }

        private static void UpdateNetworthChart()
        {
            List<List<Networth>> networthTrends = new();
            networthTrends.Add(NetworthService.GetNetworthTrend());
            
            List<Account> accounts = AccountService.GetAllAccounts();
            foreach(var acc in accounts)
            {
                networthTrends.Add(NetworthService.GetNetworthTrend(acc));
            }

            WeakReferenceMessenger.Default.Send(new NetworthTrendsLoadedMessage(networthTrends));
        }
    }
}
