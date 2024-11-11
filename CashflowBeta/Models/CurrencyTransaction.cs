using System;

namespace CashflowBeta.Models;

public class CurrencyTransaction
{
    public int ID { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Info { get; set; }
    public string Reference { get; set; }
    public virtual TransactionPartner TransactionPartner { get; set; }
    public virtual Account? Account { get; set; }
    public virtual Budget? Budget { get; set; }
}