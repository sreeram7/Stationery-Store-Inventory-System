using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class IncomingCode
    {
        public string StockCardID { get; set; }
        public string SupplierID { get; set; }
        public int IncomingQty { get; set; }
        public Nullable<System.DateTime> IncomingDate { get; set; }



        public string SupplierName { get; set; }

        public int Balance { get; set; }
    }
}