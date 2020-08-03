using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.DataBase
{
    public class Data_CollectionPoint:DataLink
    {
        public Department FindUserDepartmentByUserId(int Id)
        {
            Department dept = new Department();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                String query = @"
                                SELECT d.*
                                FROM [Users] u
                                Inner join Department d on u.DeptID_FK = d.DepartmentID 
                                WHERE u.UserID  = " + Id + ";";

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