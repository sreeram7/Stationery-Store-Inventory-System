using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Models
{
    public class WishList
    {
        public int UserID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public int RequiredQuantity { get; set; }
        public string UOM { get; set; }

        

    }
}