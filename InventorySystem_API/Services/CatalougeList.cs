using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using LogicUniversityAPI.DataBase;
using LogicUniversityAPI.Models;

namespace LogicUniversityAPI.Service
{
    public class CatalougeList : DataLink
    {
        public List<Stationary> CatByID()
        {
            List<Stationary> cat = new List<Stationary>();

            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "Select * from Category c,Stationery s where s.CategoryID =c.CategoryID";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Stationary s = new Stationary();

                    s.ItemID = (string)reader["ItemID"];
                    s.CategoryName = (string)reader["CategoryName"];
                    s.ItemName = (string)reader["ItemName"];
                    s.UOM = (string)reader["UOM"];
                    s.ReorderLevel = (int)reader["ReorderLevel"];
                    s.ReorderQuantity = (int)reader["ReorderQuantity"];
                    s.PrioritySupplier1 = (string)reader["PrioritySupplier1"];
                    s.PrioritySupplier2 = (string)reader["PrioritySupplier2"];
                    s.PrioritySupplier3 = (string)reader["PrioritySupplier3"];


                    cat.Add(s);

                }
                return cat;


            }

        }
        public void AddStationery(Stationary s)
        {
            List<Stationary> cat = new List<Stationary>();

            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "Insert into Stationary (ItemID,CategoryID,ItemName,RequiredQuantity,UOM,ReorderLevel,ReorderQuantity,PrioritySupplier1,PrioritySupplier2,PrioritySupplier3)" +
                    "values('" + s.ItemID + "','" + s.CategoryID + "','" + s.ItemName + "','" + s.RequiredQuantity + "','" + s.UOM + "','" + s.ReorderLevel + "','" + s.ReorderQuantity + "','" + s.PrioritySupplier1 + "','" +
                    s.PrioritySupplier2 + "','" + s.PrioritySupplier3 + "')";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Stationary sat = new Stationary();
                    {
                        sat.ItemID = (string)reader["ItemID"];

                        sat.CategoryName = (string)reader["CategoryName"];







                        sat.ItemName = (string)reader["ItemName"];
                        sat.UOM = (string)reader["UOM"];
                        sat.ReorderLevel = (int)reader["ReorderLevel"];
                        sat.ReorderQuantity = (int)reader["ReorderQuantity"];
                        sat.PrioritySupplier1 = (string)reader["PrioritySupplier1"];
                        sat.PrioritySupplier2 = (string)reader["PrioritySupplier2"];
                        sat.PrioritySupplier3 = (string)reader["PrioritySupplier3"];




                    }
                    cat.Add(s);

                }


            }

        }
    }
}