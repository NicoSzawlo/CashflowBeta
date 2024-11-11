using System.Collections.Generic;

namespace CashflowBeta.Models;

public class Budget
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public virtual ICollection<CurrencyTransaction> Transactions { get; set; }
}