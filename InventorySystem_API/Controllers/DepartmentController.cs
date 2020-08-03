using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicUniversityAPI.Models;
using LogicUniversityAPI.DataBase;
using System.Data.SqlClient;


namespace LogicUniversityAPI.Controllers
{
    public class DepartmentController : ApiController
    {

        [HttpGet]
        [Route("api/Department/ViewDelegations")]
        public HttpResponseMessage ViewDelegations(string DeptID)
        {
            Data_Delegation data_Delegation = new Data_Delegation();
            Delegations delegations = data_Delegation.GetDelegationInfoByID(DeptID);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, delegations);
        }


        [HttpGet]
        [Route("api/Department/RemoveDelegation/{DelegationId}")]
        public IHttpActionResult RemoveDelegation(int DelegationId)                     //this method triggers when the "ADD" button is pressed for the item.
        {
            int IsUnAllocated;

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();
                string cmdtext = @"UPDATE Delegation SET DelegationStatus ='UnAllocated' where DelegationID = '" + DelegationId + "'";
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                IsUnAllocated = cmd.ExecuteNonQuery();

            }

            Delegations Ds = Data_Delegation.GetUserInfoByDelegationID(DelegationId);

            if (IsUnAllocated != 0)
            {
                using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
                {
                    conn.Open();
                    Data_Delegation data = new Data_Delegation();

                    string cmdtext = @"UPDATE Users SET role ='DepStaff' where Username = '" + Ds.Username + "'";
                    SqlCommand cmd = new SqlCommand(cmdtext, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("api/Department/DelegateEmployee/{DeptID}")]
        public HttpResponseMessage DelegateEmployee(string DeptID)
        {
            List<Users> DeptEmployees = Data_Users.GetAllDeptStaffByDeptID(DeptID);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { DepStaff = DeptEmployees });
        }

        [HttpPost]
        [Route("api/Department/SaveDelegation")]
        public IHttpActionResult SaveDelegation(Delegations d)
        {
            int IsAllocated;
            Users user = Data_Users.GetUserInfo(d.Username);
            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();
                string cmdtext = @"insert into Delegation (DeptID,UserID,StartDate,EndDate,Username,DelegationStatus) values ('" + user.DeptID_FK + "','" + user.UserID + "','" + d.StartDate + "','" + d.EndDate + "','" + user.Username + "','Allocated')";
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                IsAllocated = cmd.ExecuteNonQuery();
            }

            if (IsAllocated != 0)
            {
                using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
                {
                    conn.Open();
                    string cmdtext = @"UPDATE Users SET role ='InterimHead' where UserID = '" + user.UserID + "'";
                    SqlCommand cmd = new SqlCommand(cmdtext, conn);
                    IsAllocated = cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("api/Department/Assginrepresentative/{DeptID}")]
        public HttpResponseMessage Assginrepresentative(string DeptID)
        {
            Users DepRepInfo = Data_Users.GetDepRepInfo(DeptID);
            var model1 = DepRepInfo.Username;

            List<Users> DeptStaff = Data_Users.GetAllDeptStaffByDeptID(DeptID);

            var model2 = DeptStaff;

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, (model1, model2));
        }

        [HttpGet]
        [Route("api/Department/SaveRepresentative/{username}")]
        public HttpResponseMessage SaveRepresentative(string username)
        {
            String DepID = Data_Users.GetDepRepbyName(username);                                //getting DEP id of the Department

            Users u = Data_Users.GetUserInfo(username);


            Users DepRepInfo = Data_Users.GetDepRepInfo(DepID);

            int prev_DepRep = DepRepInfo.UserID;                                               //Getting Previous Dep Rep ID

            int Confirm = Data_Users.AssignRepresentative(u.UserID);                          //Setting New Representative to Department

            if (Confirm != 0)
            {
                Data_Users.RemoveRepresentative(prev_DepRep);                                //Removing the Previous Representative of the Department              
            }
            var message = Request.CreateResponse(HttpStatusCode.Created, "Successfully updated!");

            return (message);
        }

        [HttpGet]
        [Route("api/Department/ApproveRequisition/{DeptID}")]
        public HttpResponseMessage ApproveRequisition(string DeptID)
        {
            Data_ApproveRequisition data_ApproveRequisition = new Data_ApproveRequisition();
            List<RequisitionList> Lt_Requcicsitions = data_ApproveRequisition.getRequisitionListForApproval(DeptID);
            var model1 = Lt_Requcicsitions;

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, (model1));
        }

        [HttpGet]
        [Route("api/Department/ApproveRequisitionDetails/{ReqID}")]
        public HttpResponseMessage ApproveRequistionDetails(int ReqID)                      //RequisitionList r
        {
            Data_RequestDetails ReqDetails = new Data_RequestDetails();

            List<WishList> DetailsOfRequest = ReqDetails.RequestDetails(ReqID);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, DetailsOfRequest);
        }

        [HttpGet]
        [Route("api/Department/RejectRequest/{ID}")]
        public HttpResponseMessage RejectRequest(int ID)                                    //string Comments
        {
            Data_CancelRequest DC = new Data_CancelRequest();

            DC.RejectRequest(ID);

            bool a = true;

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, a);
        }

        [HttpGet]
        [Route("api/Department/ApproveRequest/{ID}")]
        public HttpResponseMessage ApproveRequest(int ID)
        {
            Data_CancelRequest DC = new Data_CancelRequest();
            DC.ApproveRequest(ID);

            bool a = true;
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, a);

        }
    }
}
