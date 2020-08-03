using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class Discrepency
    {
        public int DiscrepencyID { get; set; }
        public string StockCardID { get; set; }
        public int DiscrepancyQty { get; set; }
        public string Reason { get; set; }
        public string DisbursementID { get; set; }
        public string ItemID { get; set; }

        public string DiscrepancyStatus { get; set; }

        public string ItemName { get; set; }

        public int Amount { get; set; }
        
    }
}