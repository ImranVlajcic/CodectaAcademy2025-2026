using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.WalletContracts
{
    public class WalletCon
    {
        public int userID { get; set; }
        public int currencyID { get; set; }
        public decimal balance { get; set; }
        public string purpose { get; set; }
    }
}
