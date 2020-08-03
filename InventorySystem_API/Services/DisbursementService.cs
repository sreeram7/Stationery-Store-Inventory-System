using LogicUniversityAPI.DataBase;
using LogicUniversityAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityAPI.Services
{
    public class DisbursementService
    {
        List<RequisitionDetails> rlList = new List<RequisitionDetails>();
        List<RequisitionDetails> rlItemList = new List<RequisitionDetails>();
        List<DisbursementDetails> disDetailsList = new List<DisbursementDetails>();
        List<DisbursementList> disList = new List<DisbursementList>();

        public List<DisbursementList> ShowDisbursementList()
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                string getItems = @"select  dl.DisbursementID, d.Departmentname, dl.DisbursementStatus
                    from DisbursementList dl, RequisitionList rl, Department d 
                    where rl.DeptID_FK = d.DepartmentID and d.DepartmentID = dl.DepID                          
                    group by dl.DisbursementID, d.Departmentname, dl.DisbursementStatus ";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DisbursementList dl = new DisbursementList();
                    dl.DisbursementID = (int)reader["DisbursementID"];
                    dl.Departmentname = (string)reader["Departmentname"];
                    //dl.DateofSubmission = (string)reader["DateofSubmission"];
                    dl.DisbursementStatus = (string)reader["DisbursementStatus"];
                    disList.Add(dl);
                }
            }
            return disList;
        }
        public List<RequisitionDetails> ShowDisbursementDetailUserInfo(int id)
        {
            string depID = getDepartmentByDisbID(id);
            int OTP = getOTP(id);
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select u.MobileNo, u.Username, d.Departmentname, d.CollectionPoint, u.EmailID
                        from Users u, Department d
                        where u.DeptID_FK = d.DepartmentID and 
                        u.role='DepRep' and d.DepartmentID = '" + depID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionDetails rd = new RequisitionDetails();
                    rd.MobileNo = (string)reader["MobileNo"];
                    rd.UserName = (string)reader["Username"];
                    rd.Departmentname = (string)reader["Departmentname"];
                    rd.CollectionPoint = (string)reader["CollectionPoint"];
                    rd.EmailID = (string)reader["EmailID"];
                    rd.DisbursementID = id;
                    rd.OTP = OTP;
                    rlList.Add(rd);
                }
            }
            return rlList;
        }
        public List<RequisitionDetails> ShowDisbursementDetailDataInfo(int id)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"  select dd.DeliveredQty, s.ItemName, dd.ActualQty, d.Departmentname, dd.DisbursementID, dd.ItemID
                     from DisbursementDetails dd, Department d, Stationery s
                     where d.DepartmentID= dd.DepID and dd.ItemID = s.ItemID and dd.DisbursementID= '" + id + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionDetails rd = new RequisitionDetails();
                    rd.DeliveredQty = (int)reader["DeliveredQty"];
                    rd.ActualQty = (int)reader["ActualQty"];
                    rd.ItemName = (string)reader["ItemName"];
                    rd.DisbursementID = (int)reader["DisbursementID"];
                    rd.ItemID = (string)reader["ItemID"];
                    rd.DisbursementStatus = (string)getDisbursementListStatus(rd);
                    rlItemList.Add(rd);
                }
            }
            return rlItemList;
        }

        public void createDisbursementList()
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select d.DepartmentID 
                                from Department d, RequisitionDetail rd, RequisitionList rl, Stationery s 
                                where rd.RequisitionID = rl.RequisitionID and d.DepartmentID = rl.DeptID_FK 
                                and s.ItemID = rd.ItemID and rl.statusOfRequest ='Approved'
                                group by d.DepartmentID";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // System.Guid DisbursementID = System.Guid.NewGuid();
                    RequisitionList ds = new RequisitionList();
                    ds.DeptID_FK = (string)reader["DepartmentID"];
                    addIntoDisbursementList(ds);

                }
            }

        }
        public void createDisbursementDetail(RequisitionList rl)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select Sum(rd.RequisitionQuantity) as amount,rd.ItemID, rl.DeptID_FK, dl.DisbursementID
                        from  RequisitionDetail rd, RequisitionList rl, DisbursementList dl
                        where rl.DeptID_FK= dl.DepID and rd.RequisitionID= rl.RequisitionID 
                        and dl.DisbursementStatus='pending'" +
                    "and rl.DeptID_FK='" + rl.DeptID_FK + "' and rl.statusOfRequest='approved'" +
                    "group by rd.ItemID, rl.DeptID_FK, dl.DisbursementID";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionList ds = new RequisitionList();
                    ds.amount = (int)reader["amount"];
                    ds.ItemID = (string)reader["ItemID"];
                    ds.DeptID_FK = (string)reader["DeptID_FK"];
                    //ds.RequisitionQuantity = (int)reader["RequisitionQuantity"];
                    ds.DisbursementID = (int)reader["DisbursementID"];
                    addIntoDisbursementDetail(ds);
                    //rlList.Add(ds);

                }
            }

        }

        public void addIntoDisbursementList(RequisitionList rl)
        {
            string DisbursementStatus = "pending";
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into DisbursementList( DepID, DisbursementStatus, OTP) values('" + rl.DeptID_FK + "','" + DisbursementStatus + "'," + 0 + ")";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void addIntoDisbursementDetail(RequisitionList rl)
        {
            // System.Guid DisbursementDetailID = System.Guid.NewGuid();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into DisbursementDetails(DisbursementID, DepID, ActualQty, ItemID, DeliveredQty) values('" + rl.DisbursementID + "','" + rl.DeptID_FK + "','" + rl.amount + "','" + rl.ItemID + "'," + 0 + ")";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateQty(List<RequisitionDetails> rd)
        {
            string ddStatus = "pending";
            foreach (RequisitionDetails reqDetail in rd)
            {
                using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
                {
                    connection.Open();
                    string getItems = @"UPDATE dd  
                        SET DeliveredQty= '" + reqDetail.DeliveredQty + "', DisbursementDetailsStatus='" + ddStatus + "'" +
                        " FROM DisbursementDetails dd  " +
                        " INNER JOIN DisbursementList dl ON dd.DisbursementID = dl.DisbursementID" +
                        " WHERE dd.DisbursementID = '" + reqDetail.DisbursementID + "' and dd.ItemID = '" + reqDetail.ItemID + "' and dl.DisbursementStatus = 'pending' ";
                    SqlCommand cmd = new SqlCommand(getItems, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void showAllDeptName()
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select d.DepartmentID
                                from Department d";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionList ds = new RequisitionList();
                    ds.DeptID_FK = (string)reader["DepartmentID"];
                    createDisbursementDetail(ds);
                }
            }
        }

        public void UpdateStatus(DisbursementList id)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                string DStatus = "delivered";
                connection.Open();
                string getItems = @"Update DisbursementList Set DisbursementStatus= '" + DStatus + "' WHERE DisbursementID ='" + id.DisbursementID + "'";
                List<DisbursementList> getReqIDList = UpdateReqListStatus(id);

                foreach (var single in getReqIDList)
                {
                    int getReqID = (int)single.ReqID;
                    string updateReqListStatus = @" Update RequisitionList Set statusOfRequest= 'delivered' where RequisitionID='" + getReqID + "'";
                    SqlCommand cmd1 = new SqlCommand(updateReqListStatus, connection);
                    cmd1.ExecuteNonQuery();
                }
                SqlCommand cmd = new SqlCommand(getItems, connection);
                cmd.ExecuteNonQuery();
            }
        }

        public Users GetUserInfo(int UserID)
        {
            Users user = new Users();

            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))

            {

                connection.Open();
                string cmdquery = @"Select * from Users where UserID='" + UserID + "'";
                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();
                //System.Guid a = System.Guid.NewGuid();

                //Print out each record
                while (reader.Read())
                {
                    user.UserID = (int)reader["UserId"];
                    user.Username = (string)reader["Username"];
                    user.Passcode = (string)reader["Passcode"];
                    user.EmailID = (string)reader["EmailID"];
                    user.role = (string)reader["Role"];
                    //user.SessionID = (string)reader["SessionID"];
                    user.DeptID_FK = (string)reader["DeptID_FK"];
                }

                return user;

            }
        }


        public RequisitionDetails getDataForDiscrepancy(int id, string itemID)
        {
            RequisitionDetails rDetail = new RequisitionDetails();

            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                //wrong
                string getItems = @"    select dd.DisbursementID, dd.ItemID, s.ItemName, dd.ActualQty, dd.DeliveredQty
                                        from DisbursementDetails dd, Stationery s
                                        where s.ItemID= dd.ItemID and DisbursementID='" + id + "' and dd.ItemID =  '" + itemID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rDetail.DisbursementID = (int)reader["DisbursementID"];
                    rDetail.ItemID = (string)reader["ItemID"];
                    rDetail.ItemName = (string)reader["ItemName"];
                    rDetail.ActualQty = (int)reader["ActualQty"];
                    rDetail.DeliveredQty = (int)reader["DeliveredQty"];
                }
            }
            return rDetail;
        }

        public List<DisbursementList> UpdateReqListStatus(DisbursementList dl)
        {
            DisbursementList reqID = new DisbursementList();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string selectReqID = @"	select rl.RequisitionID
	                from RequisitionList rl, DisbursementList dl, Department d
	                where rl.DeptID_FK = d.DepartmentID and d.DepartmentID = dl.DepID 
	                and rl.statusOfRequest='Approved' and dl.DisbursementID = '" + dl.DisbursementID + "'";
                SqlCommand cmd = new SqlCommand(selectReqID, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reqID.ReqID = (int)reader["RequisitionID"];
                    disList.Add(reqID);
                }

            }
            return disList;
        }

        public string getDepartmentByDisbID(int id)
        {
            string depID = null;

            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"    select d.DepartmentID
                                from Department d, DisbursementList dl
                                where dl.DepID = d.DepartmentID and dl.DisbursementID='" + id + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    depID = (string)reader["DepartmentID"];
                }
            }
            return depID;
        }

        public string getDisbursementListStatus(RequisitionDetails rd)
        {
            string disListStatus = null;
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string cmdquery = @"select dl.DisbursementStatus
                         from DisbursementList dl
                         where dl.DisbursementID ='" + rd.DisbursementID + "'";
                SqlCommand cmd = new SqlCommand(cmdquery, connection);

                //Call Execute reader to get query results
                SqlDataReader reader = cmd.ExecuteReader();
                //System.Guid a = System.Guid.NewGuid();

                //Print out each record
                while (reader.Read())
                {
                    disListStatus = (string)reader["DisbursementStatus"];
                }
                return disListStatus;
            }
        }
        public string validateOTP(RequisitionDetails reqID)
        {
            Random r = new Random();
            string OTPgenerated = (r.Next(100000, 999999)).ToString();
            addOTPToDB(OTPgenerated, reqID.DisbursementID);

            // string response = SendOTP.sendSMS(reqID.MobileNo, messageBody);
            return OTPgenerated;
        }

        public RequisitionDetails getUserDataByDisbID(int id)
        {
            string depID = getDepartmentByDisbID(id);
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select u.MobileNo, u.Username, d.Departmentname, d.CollectionPoint, u.EmailID
                        from Users u, Department d
                        where u.DeptID_FK = d.DepartmentID and 
                        u.role='DepRep' and d.DepartmentID = '" + depID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                RequisitionDetails rd = new RequisitionDetails();
                while (reader.Read())
                {
                    rd.MobileNo = (string)reader["MobileNo"];
                    rd.UserName = (string)reader["Username"];
                    rd.Departmentname = (string)reader["Departmentname"];
                    rd.CollectionPoint = (string)reader["CollectionPoint"];
                    rd.EmailID = (string)reader["EmailID"];
                    rd.DisbursementID = id;
                }
                return rd;
            }
        }

        public void addOTPToDB(string otpGenerated, int DisbID)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @" update DisbursementList  set OTP = " + otpGenerated + " where DisbursementID =" + DisbID;
                SqlCommand cmd = new SqlCommand(getItems, connection);
                cmd.ExecuteNonQuery();
            }
        }
        public int getOTP(int DisbID)
        {
            int OTP = 0;
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"  select OTP from DisbursementList where DisbursementID ='" + DisbID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                //System.Guid a = System.Guid.NewGuid();

                //Print out each record
                while (reader.Read())
                {
                    OTP = (int)reader["OTP"];
                }

            }
            return OTP;
        }
    }
}

