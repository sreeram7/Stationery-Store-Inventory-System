using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.DataBase
{
    public class Data_DisbursementList:DataLink
    {
        public List<DisbursementList> FindDisbursementListByDepartmentID(string Id)
        {
            List<DisbursementList> dlist = new List<DisbursementList>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // String query = @"select dlist.DisbursementID,dlist.DisbursementStatus from DisbursementList dlist where DepID='"+Id+"'";
                String query = @"select dlist.DisbursementID,dlist.DisbursementStatus,d.CollectionPoint from DisbursementList dlist,Department d where dlist.DepID=d.DepartmentID and dlist.DepID='" + Id + "'";

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DisbursementList list = new DisbursementList();
                    list.DisbursementID = (int)reader[0];
                    list.DisbursementStatus = (string)reader[1];
                    list.CollectionPoint = (string)reader[2];
                    dlist.Add(list);
                }
                return dlist;


            }
        }
    }
}