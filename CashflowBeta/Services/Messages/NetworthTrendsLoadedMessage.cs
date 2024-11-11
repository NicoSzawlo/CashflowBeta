using System.Collections.Generic;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CashflowBeta.Services.Messages;

public sealed class NetworthTrendsLoadedMessage : ValueChangedMessage<List<List<Networth>>>
{
    public NetworthTrendsLoadedMessage(List<List<Networth>> data) : base(data)
    {
    }
}