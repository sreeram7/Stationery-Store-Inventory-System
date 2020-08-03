using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class Delegations
    {
        public int DelegationID { get; set; }
        public string DeptID { get; set; }
        public int UserID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public  string DelegationStatus { get; set; } 
        public string Username { get; set; }
    }
}