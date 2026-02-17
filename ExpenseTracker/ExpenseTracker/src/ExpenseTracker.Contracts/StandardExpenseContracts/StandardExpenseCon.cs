using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.StandardExpenseContracts
{
    public class StandardExpenseCon
    {
        public int walletID { get; set; }
        public string reason { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public string frrquency { get; set; }
        public DateOnly nextDate { get; set; }
    }
}
