using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Info { get; set; }
        public string Reference { get; set; }
        public virtual TransactionPartner TransactionPartner { get; set; }
        public virtual Account Account { get; set; } 
        public virtual Budget? Budget { get; set; }
    }
}
