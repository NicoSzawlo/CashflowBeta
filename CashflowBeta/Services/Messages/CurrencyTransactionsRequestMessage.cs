using System.Collections.Generic;
using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CashflowBeta.Services.Messages;

public class CurrencyTransactionsRequestMessage : RequestMessage<List<CurrencyTransaction>>
{
    
}