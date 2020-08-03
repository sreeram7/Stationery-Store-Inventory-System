using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicUniversityAPI.Models;
using LogicUniversityAPI.Services;
using LogicUniversityAPI.DataBase;
using LogicUniversityAPI.EmailAlerts;

namespace LogicUniversityAPI.Controllers
{
    public class DisbursementController : ApiController
    {
        DisbursementService ds = new DisbursementService();
       
        [HttpGet]
        [Route("api/Disbursement/DisbursementList")]
        public HttpResponseMessage DisbursementList()
        {
            List<DisbursementList> requisitionLists = ds.ShowDisbursementList();            

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK,new { model = requisitionLists });
        }
        [HttpGet]
        [Route("api/Disbursement/ViewDisbursementDetail/{id}")]
        public HttpResponseMessage ViewDisbursementDetail(int id)
        {
            List<RequisitionDetails> requisitionDetailsUserInfo = ds.ShowDisbursementDetailUserInfo(id);

            List<RequisitionDetails> requisitionDetailsDataInfo = ds.ShowDisbursementDetailDataInfo(id);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { model1= requisitionDetailsUserInfo, model2= requisitionDetailsDataInfo });
        }
        [HttpGet]
        public HttpResponseMessage UpdateStatus(DisbursementList id)
        {
            ds.UpdateStatus(id);
            List<DisbursementList> requisitionLists = ds.ShowDisbursementList();

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { model = requisitionLists });
        }

        [HttpPost]
        [Route("api/Disbursement/UpdateQuantity")]
        public HttpResponseMessage UpdateQuantity([FromBody] List<RequisitionDetails> receivedQtyUpdate)
        {
            DisbursementService ds = new DisbursementService();
            ds.UpdateQty(receivedQtyUpdate);
            
            return null;


            //ds.UpdateQty(DeliveredQty, DisbursementID);
            // List<DisbursementList> requisitionLists = ds.ShowDisbursementList();
            //return requisitionLists;
        }

        [HttpPost]
        [Route("api/Disbursement/SubmitDiscrepency")]
        public HttpResponseMessage SubmitDiscrepency([FromBody]RequisitionDetails Dp)
        {
            String ItemID = Dp.ItemID;
            string StockcardID = getstockcardID(ItemID);
            string DisStatus = "PendingForApproval";

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();
                string cmdtext = @"insert into Discrepancy (DisbursementID,StockCardID,DiscrepancyQty,Reason,DiscrepancyStatus,ItemID) values ('" + Dp.DisbursementID + "','" + StockcardID + "'," + Dp.DeliveredQty + ",'" + Dp.Reason + "','" + DisStatus + "','" + ItemID + "')";
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                cmd.ExecuteNonQuery();
            }

            //Users u = ds.GetUserInfo((int)Session["UserID"]);
            //string EmailID = u.EmailID;
            int DiscrepancyId = GetDiscrepancyID();

            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Request Submitted DiscrepancyID#" + DiscrepancyId;
            String EmailBody = "<p> Dear KyiPhyu,</p>";
            EmailBody += "<p>Your order is ready to deliver with Discrepancy ID" + DiscrepancyId + " for your reference.</p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";

            send.SendEmailHTML("winkyiphyukyaw17@gmail.com", EmailSubject, EmailBody);
            List<DisbursementList> requisitionLists = ds.ShowDisbursementList();
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { model = requisitionLists }) ;
        }
        public string getstockcardID(string ItemID)
        {
            StockCard sc = new StockCard();
            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();
                string query = @"SELECT*from StockCard where ItemID ='" + ItemID + "'";
                SqlCommand cmd = new SqlCommand(query, C);
                ; SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sc.ItemID = (string)reader["ItemID"];
                    sc.StockCardID = (string)reader["StockCardID"];
                }
                return sc.StockCardID; //pass to calling function (supplier controller)
            }
        }
        [HttpPost]
        [Route("api/Disbursement/RaiseDiscrepency")]
        public HttpResponseMessage RaiseDiscrepency(RequisitionDetails Db)
        {
            // ViewBag.DiscrepencyForm = Db;
            // RequisitionDetails rd = ds.getDataForDiscrepancy(id, itemID);
            // ViewBag.rQty = DeliveredQty;

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { model = Db});
        }

        public int GetDiscrepancyID()
        {
            Discrepency reqInfo = new Discrepency();

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();

                string cmdtext = @"  select d.DiscrepencyID
                                from DisbursementDetails dd, Discrepancy d
                                where dd.DisbursementID = d.DisbursementID and dd.ItemID = d.ItemID 
                                and d.DiscrepancyStatus='PendingForApproval'";
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    reqInfo.DiscrepencyID = (int)reader["DiscrepencyID"];
                }
            }
            int DiscrepencyID = reqInfo.DiscrepencyID;
            return DiscrepencyID;
        }

        [HttpPost]
        [Route("api/Disbursement/validateOTP")]
        public string validateOTP(RequisitionDetails rd)
        {
            string OTP = ds.validateOTP(rd);

            RequisitionDetails userData = ds.getUserDataByDisbID(rd.DisbursementID);
            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Request Submitted DisbursementID#" + userData.DisbursementID;
            String EmailBody = "<p> Dear " + userData.UserName + ",</p>";
            EmailBody += "<p>Your order is ready to deliver with Disbursement ID <b>" + userData.DisbursementID + "</b> for your reference.</p>";
            EmailBody += "<p>Collection Point will be <b>" + userData.CollectionPoint + "</b></p>";
            EmailBody += "<p>" + "OTP for loging into your account is here. <b> " + OTP + " </b>." +
                " Please tell this OTP to verify your identity. " + " </p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";
            send.SendEmailHTML(userData.EmailID, EmailSubject, EmailBody);


            int id = rd.DisbursementID;
            return OTP;
        }
    }

}