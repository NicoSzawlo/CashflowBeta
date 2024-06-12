using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AccountIdentifier { get; set; }
        public string BankIdentifier { get; set; }
        public decimal Balance { get; set; }
        public virtual ICollection<CurrencyTransaction> Transactions { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
}
