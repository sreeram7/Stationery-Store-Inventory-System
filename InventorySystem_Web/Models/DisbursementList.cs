using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Models
{
    public class DisbursementList
    {
        public int DisbursementID { get; set; }
        public Nullable<int> ReqID { get; set; }
        public string DepID { get; set; }
        public string DisbursementStatus { get; set; }
        public Nullable<int> OTP { get; set; }

        public string CollectionPoint { get; set; }
       
        public string Departmentname { get; set; }
        public string DateofSubmission { get; set; }
       
    }
}