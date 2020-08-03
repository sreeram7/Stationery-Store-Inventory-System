using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;


namespace LogicUniversityWeb.DataBase
{
    public class Data_Delegation:DataLink
    {
        public List<Delegations> Delegation_List(String DeptID)
        {
            List<Delegations> Lt_Delegation = new List<Delegations>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"SELECT * FROM Delegation where DelegationStatus = 'Allocated' and  DeptID = '" + DeptID +"'";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Delegations Ds = new Delegations();
                    Ds.DelegationID = (int)sdr["DelegationID"];
                    Ds.DeptID = (string)sdr["DeptID"];
                    Ds.UserID = (int)sdr["UserID"];
                    Ds.StartDate = (string)sdr["StartDate"];
                    Ds.EndDate = (string)sdr["EndDate"];
                    Ds.Username = (string)sdr["Username"];

                    Lt_Delegation.Add(Ds);
                }
                return Lt_Delegation;
            }

        }

    }
}