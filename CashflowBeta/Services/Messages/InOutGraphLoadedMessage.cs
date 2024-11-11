using System.Collections.Generic;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CashflowBeta.Services.Messages;

public sealed class InOutGraphLoadedMessage : ValueChangedMessage<List<Budget>>
{
    public InOutGraphLoadedMessage(List<Budget> data) : base(data)
    {
    }
}