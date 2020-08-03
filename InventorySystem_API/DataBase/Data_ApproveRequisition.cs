using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityAPI.Models;


namespace LogicUniversityAPI.DataBase
{
    public class Data_ApproveRequisition:DataLink
    {
        public List<RequisitionList> getRequisitionListForApproval(string DeptID)
        {
            List<RequisitionList> Lt_Requisitions = new List<RequisitionList>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"select RL.RequisitionID,u.Username,RL.statusOfRequest,RL.DateofSubmission,RL.UserID_FK,Rl.DeptID_FK from  RequisitionList RL inner join 
                 Users u on RL.UserID_FK = u.UserID  where RL.statusOfRequest = 'PendingforApproval' and Rl.DeptID_FK = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    RequisitionList St = new RequisitionList();
                    St.RequisitionID = (int)sdr[0];
                    St.Comments = sdr[1] != DBNull.Value ? (String)sdr[1] : "";
                    St.statusOfRequest = (string)sdr[2];
                    St.DateofSubmission = (string)sdr[3];
                    St.UserID_FK = (int)sdr[4];
                    St.DeptID_FK = (string)sdr[5];
                  
                    Lt_Requisitions.Add(St);
                }
                return Lt_Requisitions;
            }

        }


    }
}