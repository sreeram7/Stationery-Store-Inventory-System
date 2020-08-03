using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Service;
using LogicUniversityAPI.Models;
using System.Data.SqlClient;
using LogicUniversityAPI.DataBase;

namespace LogicUniversityAPI.Service
{
    public class AdjustmentService
    {
        public List<Discrepency> GetDiscrepencies()
        {
            List<Discrepency> discrepancies = new List<Discrepency>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "  select d.DiscrepencyID,d.DiscrepancyQty,d.Reason,d.DiscrepancyStatus , s.ItemName , s.Price* d.DiscrepancyQty as Amount" +
                    "          from Discrepancy d,  Stationery s" +
                    "          where d.ItemID = s.ItemID";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Discrepency s = new Discrepency();
                    s.DiscrepencyID = (int)reader["DiscrepencyID"];
                    s.DiscrepancyQty = (int)reader["DiscrepancyQty"];
                    s.Reason = (string)reader["Reason"];
                    s.DiscrepancyStatus = (string)reader["DiscrepancyStatus"];                    
                    s.ItemName = (string)reader["ItemName"];
                    s.Amount = (int)reader["Amount"];
                    discrepancies.Add(s);

                }
                return discrepancies;

            }

        }
        public void UpdateStatus(Discrepency d)
        {
            Discrepency discrepancy = new Discrepency();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "update Discrepancy set DiscrepancyStatus='approved' where DiscrepencyID='" + d.DiscrepencyID + "'";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                cmd.ExecuteNonQuery();
            }
        }
        public List<Discrepency> GetAdjustmentDetails(int id)
        {
            List<Discrepency> adjust = new List<Discrepency>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "select d.DiscrepancyStatus,d.DiscrepencyID,s.ItemName,d.Reason,d.DiscrepancyQty,s.Price*d.DiscrepancyQty as Amount from Stationery s,Discrepancy d, DisbursementDetails dd,DisbursementList dl where dd.DisbursementID = dl.DisbursementID and dd.ItemID = s.ItemID and d.DisbursementID = dl.DisbursementID and d.DiscrepencyID ='" + id + "' group by s.ItemName,d.Reason,d.DiscrepancyQty,s.Price,d.DiscrepancyStatus,d.DiscrepencyID";
                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Discrepency s = new Discrepency();
                    s.DiscrepancyStatus = (string)reader["DiscrepancyStatus"];
                    s.DiscrepencyID = (int)reader["DiscrepencyID"];
                    s.ItemName = (string)reader["ItemName"];
                    s.Reason = (string)reader["Reason"];
                    s.DiscrepancyQty = (int)reader["DiscrepancyQty"];
                    s.Amount = (int)reader["Amount"];
                    adjust.Add(s);

                }
                return adjust;
            }
        }
    }
}

