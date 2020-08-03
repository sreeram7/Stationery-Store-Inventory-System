using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class StockCradDetails
    {
        public string StockCardDetailsID { get; set; }
        public int DisbursementID { get; set; }
        public string StockCardID { get; set; }
        public int Balance { get; set; }

        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string CategoryName { get; set; }
        public string Departmentname { get; set; }
        public int DeliveredQty { get; set; }
    }
}