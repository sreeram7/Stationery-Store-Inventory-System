using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;


namespace LogicUniversityAPI.DataBase
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
                    Ds.DelegationStatus = (string)sdr["DelegationStatus"];

                    Lt_Delegation.Add(Ds);
                }
                return Lt_Delegation;
            }

        }

        public Delegations GetDelegationInfoByID(String DeptID)
        {
            Delegations Ds = new Delegations();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"SELECT * FROM Delegation where DelegationStatus = 'Allocated' and  DeptID = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Ds.DelegationID = (int)sdr["DelegationID"];
                    Ds.DeptID = (string)sdr["DeptID"];
                    Ds.UserID = (int)sdr["UserID"];
                    Ds.StartDate = (string)sdr["StartDate"];
                    Ds.EndDate = (string)sdr["EndDate"];
                    Ds.Username = (string)sdr["Username"];
                    Ds.DelegationStatus = (string)sdr["DelegationStatus"];
                }
                return Ds;
            }

        }

        public static Delegations GetUserInfoByDelegationID(int DelegationId)
        {
            Delegations Ds = new Delegations();
            //String username;

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();
                string query = @"Select Username from Delegation where DelegationID='" + DelegationId + "'";
                SqlCommand cmd1 = new SqlCommand(query, C);

                SqlDataReader sdr = cmd1.ExecuteReader();

                if (sdr.Read())
                {

                    Ds.Username = (String)sdr["Username"];
                }

            }

            return Ds;

        }

    }

   

}
