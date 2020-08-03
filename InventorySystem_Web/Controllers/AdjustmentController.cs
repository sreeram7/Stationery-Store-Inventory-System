using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.EmailAlerts;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Service;
using LogicUniversityWeb.Services;

namespace LogicUniversityWeb.Controllers
{
    [Authorize]
    public class AdjustmentController : Controller
    {
        DisbursementService ds = new DisbursementService();
        // GET: Adjustment only for clerk
        [Authorize(Roles = "SClerk")]
        public ActionResult AdjustmentList()
        {
            AdjustmentService adjust = new AdjustmentService();
            List<Discrepency> adjustList = adjust.GetDiscrepencies();
            ViewBag.adjustList = adjustList;

            return View();
        }

        //for supervisor
        [Authorize(Roles = "SSupervisor")]
        public ActionResult UpdateAdjustmentStatus()
        {
            AdjustmentService adjust = new AdjustmentService();
            List<Discrepency> adjustDetails = adjust.GetDiscrepencies();
            ViewBag.adjustDetails = adjustDetails;
            return View();

        }
        [HttpPost]
        public ActionResult UpdateStatus(Discrepency d)
        {
            int id = d.DiscrepencyID;
            int qty = d.DiscrepancyQty;
            AdjustmentService adjust = new AdjustmentService();
            adjust.UpdateStatus(d);
            return RedirectToAction("UpdateAdjustmentStatus", "Adjustment");
        }

        //send Manger to notify when Discrepancy> 250 
        //by supervisor
        public ActionResult sendMail()
        {
            Users u = ds.GetUserInfo((int)Session["UserID"]);
            string EmailID = u.EmailID;
            int discrepancyID = GetDiscrepancyID();

            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Request Submitted DiscrepancyID#" + discrepancyID;
            String EmailBody = "<p> Dear KyiPhyu,</p>";
            EmailBody += "<p>Your item has discrepancy in Discrepancy ID" + discrepancyID + " for your reference.</p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";

            send.SendEmailHTML("winkyiphyukyaw17@gmail.com", EmailSubject, EmailBody);
            return RedirectToAction("UpdateAdjustmentStatus", "Adjustment");
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

        //adjustment for manager
        [Authorize(Roles = "SManager")]
        public ActionResult UpdateAdjustmentStatusMgr()
        {
            AdjustmentService adjust = new AdjustmentService();
            List<Discrepency> adjustDetails = adjust.GetDiscrepencies();
            ViewBag.adjustDetails = adjustDetails;
            return View();

        }
        //by manager
        public ActionResult sendRejectMail()
        {
            Users u = ds.GetUserInfo((int)Session["UserID"]);
            string EmailID = u.EmailID;
            int discrepancyID = GetDiscrepancyID();

            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Rejected DiscrepancyID#" + discrepancyID;
            String EmailBody = "<p> Dear SuSu,</p>";
            EmailBody += "<p>Your Discrepancy ID" + discrepancyID + " has been rejected by manager.</p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";

            send.SendEmailHTML("khaingsumyatnoe.nusiss@gmail.com", EmailSubject, EmailBody);
            return RedirectToAction("UpdateAdjustmentStatusMgr", "Adjustment");
        }

        public ActionResult sendRejectMailSup()
        {
            Users u = ds.GetUserInfo((int)Session["UserID"]);
            string EmailID = u.EmailID;
            int discrepancyID = GetDiscrepancyID();

            SendEmailNotification send = new SendEmailNotification();

            String EmailSubject = "Rejected DiscrepancyID#" + discrepancyID;
            String EmailBody = "<p> Dear SuSu,</p>";
            EmailBody += "<p>Your Discrepancy ID" + discrepancyID + " has been rejected by supervisor.</p>";
            EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
            EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";

            send.SendEmailHTML("khaingsumyatnoe.nusiss@gmail.com", EmailSubject, EmailBody);
            return RedirectToAction("UpdateAdjustmentStatus", "Adjustment");
        }
    }
}
