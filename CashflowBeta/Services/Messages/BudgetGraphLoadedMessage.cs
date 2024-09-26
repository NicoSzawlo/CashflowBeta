using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ScottPlot;
using ScottPlot.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services.Messages
{
    public sealed class BudgetGraphLoadedMessage : ValueChangedMessage<List<Budget>>
    {
        public BudgetGraphLoadedMessage(List<Budget> data) : base(data) 
        {
        }
    }
}
