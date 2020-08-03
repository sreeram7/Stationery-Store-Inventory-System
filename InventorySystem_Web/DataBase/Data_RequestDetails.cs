using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.DataBase
{
    public class Data_RequestDetails : DataLink
    {
        public List<WishList> RequestDetails(int RequestID)
        {
            List<WishList> RequestDetails = new List<WishList>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"Select RD.ItemID,RD.RequisitionQuantity, ST.ItemName, ST.UOM from RequisitionDetail RD INNER JOIN Stationery ST ON RD.ItemID = ST.ItemID where RD.RequisitionID ='" + RequestID + "'";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    WishList wt = new WishList();
                    wt.ItemID = (string)sdr["ItemID"];
                    wt.ItemName = (string)sdr["ItemName"];
                    wt.RequiredQuantity = (int)sdr["RequisitionQuantity"];
                    wt.UOM = (string)sdr["UOM"];

                    RequestDetails.Add(wt);
                }
                return RequestDetails;
            }

        }



    }
   
}