using Avalonia.Controls.Shapes;
using CashflowBeta.Models;
using CashflowBeta.Services;
using CommunityToolkit.Mvvm.ComponentModel;
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

        public static Coordinates[] GetOverallNetworthCoordinates()
        {
            List<Networth> networth = NetworthService.CalculateNetworth();
            Coordinates[] coordinates = new Coordinates[networth.Count];

            for(int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new Coordinates(Convert.ToDouble(networth[i].DateTime.ToOADate()), Convert.ToDouble(networth[i].Capital));
            }

            return coordinates;
        }
    }
}
