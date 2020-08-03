using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LogicUniversityWeb.Models;


namespace LogicUniversityWeb.DataBase
{
    public class Data_ApproveRequisition:DataLink
    {
        public List<RequisitionList> getRequisitionListForApproval(string DeptID)
        {
            List<RequisitionList> Lt_Requisitions = new List<RequisitionList>();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string cmdtext = @"select u.Username, RL.RequisitionID,Rl.Comments,RL.statusOfRequest,RL.DateofSubmission,RL.UserID_FK,Rl.DeptID_FK from  RequisitionList RL inner join 
                 Users u on RL.UserID_FK = u.UserID  where RL.statusOfRequest = 'PendingforApproval' and Rl.DeptID_FK = '" + DeptID + "'";
                SqlCommand cmd = new SqlCommand(cmdtext, C);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    RequisitionList St = new RequisitionList();
                    St.Username = (string)sdr["Username"];
                    St.RequisitionID = (int)sdr["RequisitionID"];
                    St.Comments = sdr["Comments"] != DBNull.Value ? (String)sdr["Comments"] : " ";
                    St.statusOfRequest = (string)sdr["statusOfRequest"];
                    St.DateofSubmission = (DateTime)sdr["DateofSubmission"];
                    St.UserID_FK = (int)sdr["UserID_FK"];
                    St.DeptID_FK = (string)sdr["DeptID_FK"];
                  
                    Lt_Requisitions.Add(St);
                }
                return Lt_Requisitions;
            }

        }


    }
}