using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashflowBeta.Models;

namespace CashflowBeta.ViewModels.Templates
{
    public class AccountListItemTemplate
    {
        public Account account { get; }
        public AccountListItemTemplate(Account account)
        {
            this.account = account;
        }
    }
}
