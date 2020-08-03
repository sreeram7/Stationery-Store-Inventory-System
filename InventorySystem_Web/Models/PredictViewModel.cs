using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Models
{
    public class PredictViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string CategoryName { get; set; }
        public string departmentId { get; set; }
        public string supplierId { get; set; }

        public string Departmentname { get; set; }
    }
}