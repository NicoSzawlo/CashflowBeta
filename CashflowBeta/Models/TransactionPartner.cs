using System.Collections.Generic;

namespace CashflowBeta.Models;

public class TransactionPartner
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string AccountIdentifier { get; set; }
    public string BankIdentifier { get; set; }
    public string Bankcode { get; set; }
    public virtual ICollection<CurrencyTransaction> Transactions { get; set; }

    public virtual Budget? Budget { get; set; }

    // Navigation property for the parent
    public TransactionPartner? ParentPartner { get; set; }

    // Navigation property for child partners
    public virtual ICollection<TransactionPartner>? ChildPartners { get; set; }
}