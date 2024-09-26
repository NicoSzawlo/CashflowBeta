using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashflowBeta.Models
{
    public class Networth
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Capital { get; set; }
        public virtual Account? Account {  get; set; }
    }
}
