using LogicUniversityAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.DataBase
{
    public class Data_DisbursementDetails :DataLink
    {
        public List<Stationary> DisbursementDetails(int ID)
        {
            List<Stationary> DisbursementDetailsList = new List<Stationary>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();
                string cmdtext = @"select s.ItemName, s.UOM ,d.DisbursementID,d.ActualQty,d.DeliveredQty from DisbursementDetails d  INNER JOIN Stationery s ON d.ItemID = s.ItemID  where d.DisbursementID='" + ID + "'";

                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {

                    Stationary details = new Stationary();
                    details.ItemName = sdr[0] != DBNull.Value ? (string)sdr[0] : "";
                    details.UOM = sdr[1] != DBNull.Value ? (string)sdr[1] : "";
                    details.RequiredQuantity = sdr[3] != DBNull.Value ? (int)sdr[3] : 0;
                    details.ReorderQuantity = sdr[4] != DBNull.Value ? (int)sdr[4] : 0;
                    DisbursementDetailsList.Add(details);

                    /* DisbursmentDetails.ItemName = (string)sdr["ItemName"];
                     DisbursmentDetails.UOM = (string)sdr["UOM"];
                     DisbursmentDetails.RequiredQuantity = (int)sdr["ActualQty"];
                     DisbursmentDetails.ReorderQuantity = (int)sdr["DeliveredQty"];
                     DisbursementDetails.Add(DisbursmentDetails);*/


                    /*
                                         dept.DepartmentID = reader[0] != DBNull.Value ? (String)reader[0] : throw new Exception("No department found");
                                        dept.Departmentname = reader[1] != DBNull.Value ? (String)reader[1] : "";
                                        dept.DepartmentHead = reader[2] != DBNull.Value ? (String)reader[2] : "";
                                        dept.CollectionPoint = reader[3] != DBNull.Value ? (String)reader[3] : "";
                                        dept.ContactName = reader[4] != DBNull.Value ? (String)reader[4] : "";
                                        dept.ContactNumber = reader[5] != DBNull.Value ? (String)reader[5] : "";
                                        dept.FaxNo = reader[6] != DBNull.Value ? (String)reader[*/
                }
                return DisbursementDetailsList;
            }

        }

    }
}