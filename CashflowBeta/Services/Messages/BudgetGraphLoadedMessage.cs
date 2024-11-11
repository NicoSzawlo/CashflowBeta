using System.Collections.Generic;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CashflowBeta.Services.Messages;

public sealed class BudgetGraphLoadedMessage : ValueChangedMessage<List<Budget>>
{
    public BudgetGraphLoadedMessage(List<Budget> data) : base(data)
    {
    }
}