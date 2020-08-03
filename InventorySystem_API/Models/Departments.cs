using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class Department
    {
        public string DepartmentID { get; set; }
        public string Departmentname { get; set; }
        public string DepartmentHead { get; set; }
        public string CollectionPoint { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNo { get; set; }
    }
}