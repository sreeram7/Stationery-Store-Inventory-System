using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Models
{
    public class RequisitionDetails
    {
        public string RequisitionDetailID { get; set; }
        public int RequisitionID { get; set; }
        public string ItemID { get; set; }
        public int RequisitionQuantity { get; set; }      

        public string UserName { get; set; }
        public string DateofSubmission { get; set; }
        public string EmailID { get; set; }
        public string CollectionPoint { get; set; }
        public string Departmentname { get; set; }
        public string ItemName { get; set; }
        public int DisbursementID { get; set; }
        public int ActualQty { get; set; }
        public int DeliveredQty { get; set; }
        public string Reason { get; set; }

        public string MobileNo { get; set; }

        public string DisbursementStatus { get; set; }
        public int OTP { get; set; }
    }
}