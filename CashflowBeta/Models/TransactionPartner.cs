using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class TransactionPartner
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AccountIdentifier { get; set; }
        public string BankIdentifier { get; set; }
        public string Bankcode { get; set; }
        public virtual ICollection<CurrencyTransaction> Transactions { get; set; }
        public virtual Budget? Budget { get; set; }
    }
}
