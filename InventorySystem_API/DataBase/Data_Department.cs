using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;
using System.Diagnostics;


namespace LogicUniversityAPI.DataBase
{
    public class Data_Department : DataLink
    {
        public static Department GetDepartmentInfoByID(string DeptID)
        {
            Department Dept_info = new Department();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"Select * from Department where DepartmentID = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(query, C);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("Hello");
                    Dept_info = new Department();
                    Dept_info.DepartmentID = (string)reader["DepartmentID"];
                    Dept_info.Departmentname = (string)reader["Departmentname"];
                    Dept_info.DepartmentHead = (string)reader["DepartmentHead"];
                    Dept_info.CollectionPoint = (string)reader["CollectionPoint"];
                    Dept_info.ContactName = (string)reader["ContactName"];
                    Dept_info.ContactNumber = (string)reader["ContactNumber"];
                    Dept_info.FaxNo = (string)reader["FaxNo"];


                }
            }
            return Dept_info;   //pass to calling function (Login controller)

        }

        public static bool UpdateCollectionPoint(string id, string point)
        {
            Department dept = new Department();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                String query = @" UPDATE Department set CollectionPoint = '" + point +"' WHERE DepartmentID = '" + id +"'";

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                int count = cmd.ExecuteNonQuery();
                con.Close();
                if (count > 0)
                    return true;
                else
                    return false;
            }
        }

        public static Department FindUserDepartmentByUserId(string Id)
        {
            Department dept = new Department();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                String query = @"Select * from Department where DepartmentID = '" + Id + "'";                                

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dept.DepartmentID = reader[0] != DBNull.Value ? (String)reader[0] : throw new Exception("No department found");
                    dept.Departmentname = reader[1] != DBNull.Value ? (String)reader[1] : "";
                    dept.DepartmentHead = reader[2] != DBNull.Value ? (String)reader[2] : "";
                    dept.CollectionPoint = reader[3] != DBNull.Value ? (String)reader[3] : "";
                    dept.ContactName = reader[4] != DBNull.Value ? (String)reader[4] : "";
                    dept.ContactNumber = reader[5] != DBNull.Value ? (String)reader[5] : "";
                    dept.FaxNo = reader[6] != DBNull.Value ? (String)reader[6] : "";
                }

            }
            return dept;
        }
    }
}