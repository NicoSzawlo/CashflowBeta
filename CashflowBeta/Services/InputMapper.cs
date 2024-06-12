using CashflowBeta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CashflowBeta.Services
{
    public class InputMapper
    {
        public Account AddAccount()
        {
            Account demoAccount = new Account { Name = "DemoAcc" };

        return demoAccount; 
        }
    }
}
