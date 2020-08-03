using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.DB;
using LogicUniversityWeb.Models;

namespace LogicUniversityWeb.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {

        public ActionResult Home()
        {
            /* Users users = Data_Users.GetInfoByUserID((int)Session["UserID"]);
             ViewData["Userinfo"] = users;*/
            return View();
        }

        // GET: Supplier
        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult Stationery()
        {
            List<Stationary> stationerylist = DataSuppliers.GetStationeryInfo();

            //  ViewData["stationerylist"] = stationerylist;

            ViewBag.StatList = stationerylist;

            return View();

        }

        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult ViewSupplier(Stationary s)
        {
            Stationary st = DataSuppliers.iteminfo(s.ItemID);

            List<Suppliers> SupplierList = new List<Suppliers>();

            Suppliers s1 = DataSuppliers.supplierDetails(st.PrioritySupplier1);

            SupplierList.Add(s1);

            Suppliers s2 = DataSuppliers.supplierDetails(st.PrioritySupplier2);

            SupplierList.Add(s2);

            Suppliers s3 = DataSuppliers.supplierDetails(st.PrioritySupplier3);

            SupplierList.Add(s3);

            ViewBag.supplierInfo = SupplierList;

            return View();
        }

        [Authorize(Roles = "SClerk")]
        public ActionResult ViewPurchaseForm()
        {
            PurchaseOrders po = new PurchaseOrders();

            return View();

        }

        public string getstockcardID(PurchaseOrders s)
        {
            StockCard sc = new StockCard();
            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();
                string query = @"SELECT ItemID,StockCardID from StockCard where ItemID ='" + s.ItemID + "'";
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
        public ActionResult CreatePurchaseOrder(PurchaseOrders s)
        {

            PurchaseOrders po = new PurchaseOrders();

            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"Insert into PurchaseOrder(DescriptionPurchase, SupplierID, ItemID, PurchaseOrderStatus,OrderQuantity)" +
      "values('" + s.DescriptionPurchase + "','" + s.SupplierID + "','" + s.ItemID + "','" + s.PurchaseOrderStatus + "'," + s.OrderQuantity + ")";

                SqlCommand cmd = new SqlCommand(query, C);
                cmd.ExecuteNonQuery();
            }
            updateIncomingStock(s);


            return RedirectToAction("PurchaseOrdersList");

        }

        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult PurchaseOrdersList()
        {
            List<PurchaseOrders> purchaseOrders = getPurchaseOrdersList();
            ViewBag.orders = purchaseOrders;
            return View();

        }

        public void updateIncomingStock(PurchaseOrders s)
        {
            string stockcardID = getstockcardID(s);
            string incomingSS = "pending";
            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();

                string query = @"INSERT INTO IncomingStock(IncomingStockStatus, StockCardID, SupplierID, IncomingQty, IncomingDate) values('" + incomingSS + "','" + stockcardID + "','" + s.SupplierID + "'," + s.OrderQuantity + ",'" + DateTime.Now.ToString() + "')";
                SqlCommand cmd = new SqlCommand(query, C);
                cmd.ExecuteNonQuery();
            }
        }


        public List<PurchaseOrders> getPurchaseOrdersList()
        {
            List<PurchaseOrders> orders = new List<PurchaseOrders>();
            using (SqlConnection C = new SqlConnection(DataLink.connectionString))
            {
                C.Open();
                string query = @"select ss.ItemID, s.SupplierName,ss.ItemName,p.PurchaseOrderID,p.OrderQuantity from Supplier s,Stationery ss,PurchaseOrder p where s.SupplierID=p.SupplierID and ss.ItemID=p.ItemID";
                SqlCommand cmd = new SqlCommand(query, C);
                ; SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PurchaseOrders p = new PurchaseOrders();
                    p.ItemID = (string)reader["ItemID"];
                    p.SupplierName = (string)reader["SupplierName"];
                    p.ItemName = (string)reader["ItemName"];
                    p.PurchaseOrderID = (int)reader["PurchaseOrderID"];

                    p.OrderQuantity = (int)reader["OrderQuantity"];
                    orders.Add(p);
                }

                return orders;
            }
        }
    }

}