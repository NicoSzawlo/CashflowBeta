using System.Collections.Generic;

namespace CashflowBeta.Models;

public class Asset
{
    public int ID { get; set; }
    public decimal Amount { get; set; }
    public string AssetIdentifier { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<AssetTransaction> AssetTransactions { get; set; }
}