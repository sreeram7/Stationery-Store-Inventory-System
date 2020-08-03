using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.Models
{
    public class Stationary
    {
        public string ItemID { get; set; }
        public string CategoryID { get; set; }
        public string ItemName { get; set; }
        public int RequiredQuantity { get; set; }
        public string UOM { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public string PrioritySupplier1 { get; set; }
        public string PrioritySupplier2 { get; set; }
        public string PrioritySupplier3 { get; set; }

        public List<WishList> wishListofusers  { get; set; }

        public string CategoryName { get; set; }
    }
}