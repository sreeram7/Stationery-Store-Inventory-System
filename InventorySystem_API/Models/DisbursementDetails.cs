using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class DisbursementDetails
    {
        public string DisbursementDetailsID { get; set; }
        public string DisbursementID { get; set; }
        public string DepID { get; set; }
        public Nullable<int> ActualQty { get; set; }
        public Nullable<int> DeliveredQty { get; set; }

        public string ItemID { get; set; }

    }
}