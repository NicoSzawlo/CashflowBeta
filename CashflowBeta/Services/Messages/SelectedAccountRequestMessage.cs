using CashflowBeta.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Services.Messages
{
    public sealed class SelectedAccountRequestMessage : RequestMessage<Account>
    {
    }
}
