using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;

namespace LogicUniversityAPI.DataBase
{
    public class Data_RequisitionForm : DataLink
    {
        public List<WishList> wishList(int ID)
        {
            List<WishList> Lt_wishlist = new List<WishList>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"SELECT * FROM WishList where UserID="+ID;
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                  while (sdr.Read())
                  {
                     WishList Wt = new WishList();
                     Wt.UserID = (int)sdr["UserID"];
                     Wt.ItemID = (string)sdr["ItemID"];
                     Wt.ItemName = (string)sdr["ItemName"];
                     Wt.RequiredQuantity = (int)sdr["RequiredQuantity"];
                     Wt.UOM = (string)sdr["UOM"];

                    Lt_wishlist.Add(Wt);
                  }
                  return Lt_wishlist;
            }

        }
    }

}