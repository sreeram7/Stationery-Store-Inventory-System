using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicUniversityAPI.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LogicUniversityAPI.DataBase

{
   public class Data_Users : DataLink
    {
        public static Users GetUserInfo(string username)
        {
            Users u_info = new Users();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"SELECT * from Users WHERE Username = '" + username + "'";
                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    u_info = new Users();
                    u_info.UserID = (int)reader["UserId"];
                    u_info.Username = (string)reader["Username"];
                    u_info.Passcode = (string)reader["Passcode"];
                    u_info.EmailID = (string)reader["EmailID"];
                    u_info.role = (string)reader["Role"];
                   // u_info.SessionID = (string)reader["SessionID"];
                    u_info.DeptID_FK = (string)reader["DeptID_FK"];
                    u_info.URL = (string)reader["Images"];

                }
            }
            return u_info;   //pass to calling function (Login controller)
        }

        public static Users GetInfoByUserID(int UsreID)
        {
            Users u_info = new Users();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select * from  Users where UserID = '" + UsreID + "'";
                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    u_info = new Users();
                    u_info.UserID = (int)reader["UserId"];
                    u_info.Username = (string)reader["Username"];
                    u_info.Passcode = (string)reader["Passcode"];
                    u_info.EmailID = (string)reader["EmailID"];
                    u_info.role = (string)reader["Role"];
                    // u_info.SessionID = (string)reader["SessionID"];
                    u_info.DeptID_FK = (string)reader["DeptID_FK"];
                    u_info.URL = (string)reader["Images"];

                }
            }
            return u_info;   //pass to calling function (Login controller)
        }

        public static List<Users> GetAllEmpByDeptID(string DeptID)
        {
            List<Users> Lt_Users = new List<Users>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select * from  Users where DeptID_FK = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                   
                     Users u_info = new Users();
                    u_info.UserID = (int)reader["UserId"];
                    u_info.Username = (string)reader["Username"];
                    u_info.Passcode = (string)reader["Passcode"];
                    u_info.EmailID = (string)reader["EmailID"];
                    u_info.role = (string)reader["Role"];
                    // u_info.SessionID = (string)reader["SessionID"];
                    u_info.DeptID_FK = (string)reader["DeptID_FK"];
                    u_info.URL = (string)reader["Images"];

                    Lt_Users.Add(u_info);
                }


            }
            return Lt_Users;   //pass to calling function (Login controller)
        }

        public static Users GetDepRepInfo(string DeptID)
        {
            Users u_info = new Users();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select * from  Users where role = 'DepRep' and  DeptID_FK = '" + DeptID + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    u_info = new Users();
                    u_info.UserID = (int)reader["UserId"];
                    u_info.Username = (string)reader["Username"];
                    u_info.Passcode = (string)reader["Passcode"];
                    u_info.EmailID = (string)reader["EmailID"];
                    u_info.role = (string)reader["Role"];
                    // u_info.SessionID = (string)reader["SessionID"];
                    u_info.DeptID_FK = (string)reader["DeptID_FK"];
                    u_info.URL = (string)reader["Images"];

                }
            }
            return u_info;   //pass to calling function (Login controller)
        }

        public static List<Users> GetAllDeptStaffByDeptID(string DeptID)
        {
            List<Users> Lt_Users = new List<Users>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select * from  Users where role = 'DepStaff' and  DeptID_FK = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    Users u_info = new Users();
                    u_info.UserID = (int)reader["UserId"];
                    u_info.Username = (string)reader["Username"];
                    u_info.Passcode = (string)reader["Passcode"];
                    u_info.EmailID = (string)reader["EmailID"];
                    u_info.role = (string)reader["Role"];
                    // u_info.SessionID = (string)reader["SessionID"];
                    u_info.DeptID_FK = (string)reader["DeptID_FK"];
                    u_info.URL = (string)reader["Images"];

                    Lt_Users.Add(u_info);
                }

            }
            return Lt_Users;   //pass to calling function (Login controller)
        }

        public static int AssignRepresentative(int UserID)
        {
            int status;

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();
                string cmdtext = @"UPDATE Users SET role ='DepRep' where UserID = '" + UserID + "'";            // Update status of request to cancel with request number
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                  status = cmd.ExecuteNonQuery();

                return status;
            }
            
        }

        public static int RemoveRepresentative(int UserID)
        {
            int status;

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();
                string cmdtext = @"UPDATE Users SET role ='DepStaff' where UserID = '" + UserID + "'";            // Update status of request to cancel with request number
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                status = cmd.ExecuteNonQuery();

                return status;
            }

        }

        public static List<Users> GetAllDepStaffNameByID(string DeptID)
        {
            List<Users> UsernameList = new List<Users>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select Username from  Users where role = 'DepStaff' and  DeptID_FK = '" + DeptID + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    Users u_info = new Users();
                    u_info.Username = (string)reader["UserName"];
                    UsernameList.Add(u_info);

                }
            }
            return UsernameList;   //pass to calling function (Login controller)
        }

        public static String GetDepRep(int UserID)
        {
            Users u_info = new Users();

            String DepID;

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select DeptID_FK from  Users where role = 'DepStaff' and  UserID = '" + UserID + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    u_info = new Users();
                    u_info.DeptID_FK = (string)reader[0];

                }
                DepID = u_info.DeptID_FK;

            }
            return DepID;   //pass to calling function (Login controller)
        }


        public static String GetDepRepbyName(string username)
        {
            Users u_info = new Users();

            String DepID;

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"select DeptID_FK from  Users where role = 'DepStaff' and  Username = '" + username + "'";

                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    u_info = new Users();
                    u_info.DeptID_FK = (string)reader[0];

                }
                DepID = u_info.DeptID_FK;

            }
            return DepID;   //pass to calling function (Login controller)
        }
    }
}
