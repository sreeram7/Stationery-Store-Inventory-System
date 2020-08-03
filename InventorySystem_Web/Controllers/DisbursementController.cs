
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.EmailAlerts;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityWeb.Controllers
{
    [Authorize]
    public class DisbursementController : Controller
    {
        DisbursementService ds = new DisbursementService();
      
        // GET: Disbursement
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SClerk")]

        public ActionResult createDisb()
        {
            ds.createDisbursementList();
            ds.showAllDeptName();
            return RedirectToAction("DisbursementList");
        }
        public ActionResult createDisbursement()
        {
            return View();
        }


        [Authorize(Roles = "SClerk")]
        public ActionResult DisbursementList()
        {
            List<DisbursementList> requisitionLists = ds.ShowDisbursementList();
            ViewBag.disbursementList = requisitionLists;
            return View();
        }

        [Authorize(Roles = "SSupervisor,SManager")]
        public ActionResult DisbursementListMgrSupr()
        {
            List<DisbursementList> requisitionLists = ds.ShowDisbursementList();
            ViewBag.dList = requisitionLists;
            return View();
        }

        [Authorize(Roles = "SClerk")]
        public ActionResult ViewDisbursementDetail(int id)
        {            
            List<RequisitionDetails> requisitionDetailsUserInfo = ds.ShowDisbursementDetailUserInfo(id);
            ViewBag.requisitionDetail = requisitionDetailsUserInfo;
            List<RequisitionDetails> requisitionDetailsDataInfo = ds.ShowDisbursementDetailDataInfo(id);
            ViewBag.dataInfo = requisitionDetailsDataInfo;
           
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "SClerk")]
        public ActionResult UpdateStatus(DisbursementList id)
        {
            ds.UpdateStatus(id);
            return RedirectToAction("DisbursementList");
        }

        [Authorize(Roles = "SClerk")]
        public ActionResult UpdateQuantity(List<RequisitionDetails> receivedQtyUpdate)
        {
            ds.UpdateQty(receivedQtyUpdate);
            //ds.UpdateQty(DeliveredQty, DisbursementID);
            return RedirectToAction("DisbursementList");
        }

        public int GetDisbursementID()
        {
            DisbursementList reqInfo = new DisbursementList();

            using (SqlConnection conn = new SqlConnection(DataLink.connectionString))
            {
                conn.Open();

                string cmdtext = @"select top(1) DisbursementID from DisbursementList dl,RequisitionList rl where dl.DepID=rl.DeptID_FK and rl.UserID_FK='" + (int)Session["UserID"] + "' and dl.DisbursementStatus='pending'" + " ORDER BY DisbursementID DESC";
                SqlCommand cmd = new SqlCommand(cmdtext, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    reqInfo.DisbursementID = (int)reader["DisbursementID"];
                }
            }
            int DisbursementId = reqInfo.DisbursementID;
            return DisbursementId;
        }

        [Authorize(Roles = "SClerk")]
        public ActionResult RaiseDiscrepency(RequisitionDetails Db)
        {
            // ViewBag.DiscrepencyForm = Db;
            // RequisitionDetails rd = ds.getDataForDiscrepancy(id, itemID);
            // ViewBag.rQty = DeliveredQty;
            ViewData["Db"] = Db;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SClerk")]
        public ActionResult SubmitDiscrepency(RequisitionDetails Dp)
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

            Users u = ds.GetUserInfo((int)Session["UserID"]);
            string EmailID = u.EmailID;
            int DiscrepancyId = GetDiscrepancyID();

            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Request Submitted DiscrepancyID#" + DiscrepancyId;
            String EmailBody = "<p> Dear KyiPhyu,</p>";
            EmailBody += "<p>Your order is ready to deliver with Discrepancy ID" + DiscrepancyId + " for your reference.</p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";

            send.SendEmailHTML("winkyiphyukyaw17@gmail.com", EmailSubject, EmailBody);

            return RedirectToAction("DisbursementList");
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

        public ActionResult validateOTP(RequisitionDetails rd)
        {
            string OTP = ds.validateOTP(rd);
          
            RequisitionDetails userData = ds.getUserDataByDisbID(rd.DisbursementID);
            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Request Submitted DisbursementID#" + userData.DisbursementID;
            String EmailBody = "<p> Dear " + userData.UserName + ",</p>";
            EmailBody += "<p>Your order is ready to deliver with Disbursement ID <b>" + userData.DisbursementID + "</b> for your reference.</p>";
            EmailBody += "<p>Collection Point will be <b>" + userData.CollectionPoint + "</b></p>";
            EmailBody += "<p>" + "OTP for loging into your account is here. <b> " + OTP + " </b>." +
                " Please tell this OTP to verify your identity. "+ " </p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";
            send.SendEmailHTML(userData.EmailID, EmailSubject, EmailBody);

          
            int id = rd.DisbursementID;
            return RedirectToAction("ViewDisbursementDetail/"+ id);
        }
    }
}