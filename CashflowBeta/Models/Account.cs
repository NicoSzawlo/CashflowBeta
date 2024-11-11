using System.Collections.Generic;

namespace CashflowBeta.Models;

public class Account
{
    public Account()
    {
        Name = "New Account";
        AccountIdentifier = "IBANXXXX####XXXX";
        BankIdentifier = "AB000111XXX";
        Balance = 0;
    }

    public int ID { get; set; }
    public string Name { get; set; }
    public string AccountIdentifier { get; set; }
    public string BankIdentifier { get; set; }
    public decimal Balance { get; set; }
    public virtual ICollection<CurrencyTransaction> Transactions { get; set; }
    public virtual ICollection<Asset> Assets { get; set; }
}