using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CashflowBeta.Services.Messages;

public sealed class SelectedAccountRequestMessage : RequestMessage<Account>
{
}