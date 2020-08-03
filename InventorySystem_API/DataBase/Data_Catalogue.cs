using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;

namespace LogicUniversityAPI.DataBase
{
    public class Data_Catalogue:DataLink
    {
        public List<Stationary> Stationery_List()
        {
            List<Stationary> Lt_Stationary = new List<Stationary>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"SELECT * FROM Stationery";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Stationary St = new Stationary();
                    St.ItemID = (string)sdr["ItemID"];
                    St.CategoryID = (string)sdr["CategoryID"];
                    St.ItemName = (string)sdr["ItemName"];
                    St.UOM = (string)sdr["UOM"];

                    Lt_Stationary.Add(St);
                }
                return Lt_Stationary;
            }

        }


    }
}