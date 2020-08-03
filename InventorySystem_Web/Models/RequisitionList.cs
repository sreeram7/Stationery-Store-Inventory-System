using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.Models
{
    public class RequisitionList
    {
        public int RequisitionID { get; set; }
        public string statusOfRequest { get; set; }
        public DateTime DateofSubmission { get; set; }
        public string Comments { get; set; }
        public string DeptID_FK { get; set; }
        public Nullable<int> UserID_FK { get; set; }      

        public string Departmentname { get; set; }
        public string ItemID { get; set; }
        public string Username { get; set; }
        public string EmailID { get; set; }
        public string CollectionPoint { get; set; }

        public string ItemName { get; set; }
        public int RequisitionQuantity { get; set; }
        public int DisbursementID { get; set; }
        public int amount { get; set; }
    }
}