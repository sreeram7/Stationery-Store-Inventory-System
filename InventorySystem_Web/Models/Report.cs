using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Models
{
    public class Report
    {
        private string department;
        private string supplier;
        private string category;
        private List<string> axis_X;
        private List<int> axis_Y;

        public string Department
        {
            set { department = value; }
            get { return department; }
        }
        public string Supplier
        {
            set { supplier = value; }
            get { return supplier; }
        }
        public string Category
        {
            set { category = value; }
            get { return category; }
        }
        public List<string> Axis_X
        {
            set { axis_X = value; }
            get { return axis_X; }
        }
        public List<int> Axis_Y
        {
            set { axis_Y = value; }
            get { return axis_Y; }
        }
    }
}