using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class Asset
    {

        public int ID { get; set; }
        public decimal Amount { get; set; }
        public string AssetIdentifier { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<AssetTransaction> Transactions { get; set; }

    }
}
