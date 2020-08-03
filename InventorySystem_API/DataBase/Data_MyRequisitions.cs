using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;

namespace LogicUniversityAPI.DataBase
{
    public class Data_MyRequisitions: DataLink
    {
        public List<RequisitionList> getRequisitionList(int UserID)
        {
            List<RequisitionList> Lt_Requisitions = new List<RequisitionList>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"SELECT * from RequisitionList WHERE UserID_FK = '"+ UserID + "' order by RequisitionID desc";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    RequisitionList St = new RequisitionList();
                    St.RequisitionID = (int)sdr[0];
                    St.statusOfRequest = (string)sdr[1];
                    St.DateofSubmission = (string)sdr[2];
                    St.DeptID_FK = (string)sdr[4];
                    St.UserID_FK = (int)sdr[5];

                    Lt_Requisitions.Add(St);
                }
                return Lt_Requisitions;
            }

        }

    }
}