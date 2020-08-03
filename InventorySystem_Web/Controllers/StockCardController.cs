using LogicUniversityWeb.EmailAlerts;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityWeb.Controllers
{
    [Authorize]
    public class StockCardController : Controller
    {
        StockCardService sc = new StockCardService();
        DisbursementService ds = new DisbursementService();
        // GET: StockCard
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult StockCardList()
        {
            List<StockCard> stockCard = sc.createStockCardList();
            ViewBag.stockCardList = stockCard;
            return View();
        }
        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult StockCardDetail(string stockCardID, string id)
        {
            StockCradDetails scDetails = sc.createStockCardDetail(id);

            sc.createDisbursementTransaction(stockCardID, id);
          //  List<StockCradDetails> scDisTran = sc.showDisbursementTransaction(stockCardID, id);

            sc.createSupplierTransaction(stockCardID, id);
            List<IncomingCode> incomingTran = sc.showSupplierTransaction(stockCardID);
            int balance = sc.getLatestBalance(stockCardID, id);
            int reorderLevel = sc.getReorderLevel(id);
            if (balance <= reorderLevel)
            {
                Users u = ds.GetUserInfo((int)Session["UserID"]);
                string EmailID = u.EmailID;
                SendEmailNotification send = new SendEmailNotification();

                String EmailSubject = "Reorder level low at ItemID#" + id;
                String EmailBody = "<p> Dear SuSu,</p>";
                EmailBody += "<p>Your reorder level is low in ItemID" + id + ". For your reference.</p>";
                EmailBody += "<p>Thank you<br/>Logic University Staionery Store</p>";
                EmailBody += "<p> Please do not reply to this email it is auto-generated.</p>";
                send.SendEmailHTML("khaingsumyatnoe.nusiss@gmail.com", EmailSubject, EmailBody);
            }
            ViewBag.stockCardDetail = scDetails;
           // ViewBag.scDisbursementTran = scDisTran;
            ViewBag.incomingTran = incomingTran;
            return View();
        }
    }
}