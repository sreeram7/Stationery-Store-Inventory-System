using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Services;
using LogicUniversityWeb.DataBase;

namespace LogicUniversityWeb.Services
{
    public class RequisitionService
    {
        public List<RequisitionList> GetRequisitionList()
        {
            List<RequisitionList> requisitions = new List<RequisitionList>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = @" select rl.DateofSubmission,rl.RequisitionID,d.Departmentname,rl.DeptID_FK from RequisitionList rl,Department d where rl.DeptID_FK=d.DepartmentID group by d.Departmentname,rl.DeptID_FK,rl.RequisitionID,rl.DateofSubmission";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    RequisitionList s = new RequisitionList();
                    s.RequisitionID = (int)reader["RequisitionID"];
                    s.Departmentname = (string)reader["Departmentname"];
                    s.DeptID_FK = (string)reader["DeptID_FK"];
                    s.DateofSubmission = (DateTime)reader["DateofSubmission"];


                    requisitions.Add(s);

                }
                return requisitions;

            }
        }
        public List<RequisitionDetails> GetRequisitionDetails(int RequisitionID)
        {
            List<RequisitionDetails> requisitions = new List<RequisitionDetails>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = @"select s.ItemID, s.ItemName,rd.RequisitionQuantity
                    from RequisitionDetail rd,RequisitionList rl,Stationery s
                    where rd.RequisitionID=rl.RequisitionID
                    and rd.ItemID=s.ItemID and rd.RequisitionID=" + RequisitionID;

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    RequisitionDetails s = new RequisitionDetails();
                    s.ItemID = (string)reader["ItemID"];
                    s.ItemName = (string)reader["ItemName"];
                    s.RequisitionQuantity = (int)reader["RequisitionQuantity"];


                    requisitions.Add(s);

                }
                return requisitions;
            }
        }


    }
}