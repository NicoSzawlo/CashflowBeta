using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class AssetTransaction
    {
        public int ID { get; set; }
        public virtual Asset Asset { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal TransactionDuties { get; set; }
        public virtual Account Account { get; set; }
                
    }
}
