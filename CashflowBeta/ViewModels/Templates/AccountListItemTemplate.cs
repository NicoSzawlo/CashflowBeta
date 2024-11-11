using CashflowBeta.Models;

namespace CashflowBeta.ViewModels.Templates;

public class AccountListItemTemplate
{
    public AccountListItemTemplate(Account account)
    {
        this.account = account;
    }

    public Account account { get; }
}