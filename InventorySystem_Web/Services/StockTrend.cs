using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.Models;
namespace LogicUniversityWeb.Services
{
    public class StockTrend
    {
        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "select * from Category";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Category cat = new Category();
                    cat.CategoryID = (string)reader["CategoryID"];
                    cat.CategoryName = (string)reader["CategoryName"];

                    categories.Add(cat);

                }
                return categories;

            }

        }
        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {
                connection.Open();

                string cmdquery = "select DepartmentID,Departmentname  from Department";

                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();

                //Print out each record
                while (reader.Read())
                {
                    Department cat = new Department();
                    cat.DepartmentID = (string)reader["DepartmentID"];
                    cat.Departmentname = (string)reader["Departmentname"];

                    departments.Add(cat);

                }
                return departments;

            }
        }
    }
}