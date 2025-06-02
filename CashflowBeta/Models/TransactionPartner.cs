using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CashflowBeta.Models;

public partial class TransactionPartner : ObservableObject
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string AccountIdentifier { get; set; }
    public string BankIdentifier { get; set; }
    public string Bankcode { get; set; }
    public virtual ICollection<CurrencyTransaction> Transactions { get; set; }

    [ObservableProperty]
    private Budget? budget;
    
    // Navigation property for the parent
    public TransactionPartner? ParentPartner { get; set; }

    // Navigation property for child partners
    public virtual ICollection<TransactionPartner>? ChildPartners { get; set; }
}