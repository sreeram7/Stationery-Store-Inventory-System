using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.DataBase;
using LogicUniversityAPI.Models;

namespace LogicUniversityAPI.DB
{
    public class DataSuppliers : DataLink
    {
        public static List<Stationary> GetStationeryInfo()
        {
            List<Stationary> stationerylist = new List<Stationary>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"SELECT * from Stationery";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Stationary stationery = new Stationary();
                    stationery.ItemID = (string)reader["ItemID"];
                    stationery.CategoryID = (string)reader["CategoryID"];
                    stationery.ItemName = (string)reader["ItemName"];
                    stationery.UOM = (string)reader["UOM"];
                    stationery.ReorderLevel = (int)reader["ReorderLevel"];
                    stationery.ReorderQuantity = (int)reader["ReorderQuantity"];
                    stationery.PrioritySupplier1 = (string)reader["PrioritySupplier1"];
                    stationery.PrioritySupplier2 = (string)reader["PrioritySupplier2"];
                    stationery.PrioritySupplier3 = (string)reader["PrioritySupplier3"];

                    stationerylist.Add(stationery);
                }
                return stationerylist; //pass to calling function (supplier controller)
            }
        }

        public static Stationary iteminfo(String ItemID)
        {
            Stationary stationery = new Stationary();
            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"SELECT * from Stationery where ItemID = '" + ItemID + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    stationery = new Stationary();
                    stationery.ItemID = (string)reader["ItemID"];
                    stationery.CategoryID = (string)reader["CategoryID"];
                    stationery.ItemName = (string)reader["ItemName"];
                    stationery.UOM = (string)reader["UOM"];
                    stationery.ReorderLevel = (int)reader["ReorderLevel"];
                    stationery.ReorderQuantity = (int)reader["ReorderQuantity"];
                    stationery.PrioritySupplier1 = (string)reader["PrioritySupplier1"];
                    stationery.PrioritySupplier2 = (string)reader["PrioritySupplier2"];
                    stationery.PrioritySupplier3 = (string)reader["PrioritySupplier3"];

                }
                return stationery; //pass to calling function (supplier controller)
            }
        }

        public static Suppliers supplierDetails(String SupplierID)
        {
            Suppliers suppliers = new Suppliers();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"Select * from Supplier where SupplierID =  '" + SupplierID + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    suppliers = new Suppliers();
                    suppliers.SupplierID = (string)reader["SupplierID"];
                    suppliers.SupplierName = (string)reader["SupplierName"];
                }
                return suppliers; //pass to calling function (supplier controller)
            }
        }
    }


}
