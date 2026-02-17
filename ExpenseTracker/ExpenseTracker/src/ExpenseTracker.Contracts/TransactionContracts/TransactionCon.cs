using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.TransactionContracts
{
    public class TransactionCon
    {
        public int walletID { get; set; }
        public int categoryID { get; set; }
        public int currencyID { get; set; }
        public TimeOnly transactionTime { get; set; }
        public DateOnly transactionDate { get; set; }
        public string transactionType { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
    }
}
