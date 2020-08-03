
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LogicUniversityWeb.Services
{
    public class StockCardService
    {
        List<StockCard> scList = new List<StockCard>();
        List<StockCradDetails> scDetailList = new List<StockCradDetails>();
        List<IncomingCode> incomingList = new List<IncomingCode>();
        List<IncomingCode> iList = new List<IncomingCode>();
        public List<StockCard> createStockCardList()
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                string getItems = @"select s.ItemName, sc.StockCardID, s.ItemID
                                from Stationery s, StockCard sc
								where s.ItemID= sc.ItemID";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockCard sc = new StockCard();
                    sc.StockCardID = (string)reader["StockCardID"];
                    sc.ItemName = (string)reader["ItemName"];
                    sc.ItemID = (string)reader["ItemID"];
                    scList.Add(sc);
                }
            }
            return scList;
        }

        public StockCradDetails createStockCardDetail(string ItemID)
        {
            StockCradDetails sc = new StockCradDetails();
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                string getItems = @"select s.ItemID, s.ItemName, s.UOM, c.CategoryName
                                     from Stationery s, Category c
                                     where s.CategoryID = c.CategoryID and s.ItemID='" + ItemID + "'" +
                                     "group by s.ItemID, s.ItemName, s.UOM, c.CategoryName";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    sc.ItemID = (string)reader["ItemID"];
                    sc.ItemName = (string)reader["ItemName"];
                    sc.UOM = (string)reader["UOM"];
                    sc.CategoryName = (string)reader["CategoryName"];
                    //scDetailList.Add(sc);
                }
            }
            return sc;
        }

        public void createDisbursementTransaction(string stockCardID, string ItemID)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select dd.DisbursementDetailsID, scDetails.StockCardID,dd.DisbursementID,  scDetails.Balance-dd.DeliveredQty as Balance, d.DepartmentName, dd.DeliveredQty
                    from DisbursementDetails dd, StockCardDetails scDetails, Department d
                    where dd.DisbursementDetailsStatus ='pending'and dd.ItemID = '" + ItemID + "' and scDetails.StockCardID ='" + stockCardID + "'" +
                    "and d.DepartmentID = dd.DepID	and  scDetails.Balance in(select top 1 scD.Balance" +
                    " from StockCardDetails scD" +
                    " where scD.StockCardID='" + stockCardID + "' order by scD.StockCardDetailsID desc)";

                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    StockCradDetails sc = new StockCradDetails();
                    sc.DisbursementDetailsID = (int)reader["DisbursementDetailsID"];
                    sc.StockCardID = (string)reader["StockCardID"];
                    sc.DisbursementID = (int)reader["DisbursementID"];
                    sc.Balance = (int)reader["Balance"];
                    sc.Departmentname = (string)reader["Departmentname"];
                    sc.DeliveredQty = (int)reader["DeliveredQty"];
                    addDisTranIntoStockCard(sc);
                }

            }
        }

        public void createSupplierTransaction(string stockCardID, string id)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                //Update into database
                string getItems = @" select iss.StockCardID, iss.IncomingQty+scDetails.Balance as Balance, iss.IncomingQty, s.SupplierName
                                    from IncomingStock iss, StockCard sc, StockCardDetails scDetails, Supplier s
                                    where iss.StockCardID=" + stockCardID + " and sc.ItemID='" + id + "' and iss.StockCardID=sc.StockCardID and" +
                                    " s.SupplierID = iss.SupplierID and " +
                                    " sc.StockCardID = scDetails.StockCardID and iss.IncomingStockStatus='pending' and scDetails.Balance in" +
                                    " (select top 1 scD.Balance" +
                                    " from StockCardDetails scD" +
                                    " where scD.StockCardID=" + stockCardID + "" +
                                    " order by scD.StockCardDetailsID desc)";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IncomingCode ic = new IncomingCode();
                    ic.StockCardID = (string)reader["StockCardID"];
                    ic.Balance = (int)reader["Balance"];
                    ic.IncomingQty = (int)reader["IncomingQty"];
                    ic.SupplierName = (string)reader["SupplierName"];
                    addIntoStockCard(ic);
                }
            }

        }

        public List<StockCradDetails> showDisbursementTransaction(string stockCardID, string ItemID)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select d.Departmentname, scDetails.Balance, dd.DeliveredQty
                        from StockCardDetails scDetails, DisbursementDetails dd, DisbursementList dl, Department d
                        where scDetails.DisbursementID = dd.DisbursementID and dl.DisbursementID= dd.DisbursementID
                        and dl.DepID = d.DepartmentID and dd.ItemID='" + ItemID + "'  and scDetails.StockCardID='" + stockCardID + "'";

                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockCradDetails sc = new StockCradDetails();
                    sc.Departmentname = (string)reader["Departmentname"];
                    sc.DeliveredQty = (int)reader["DeliveredQty"];
                    sc.Balance = (int)reader["Balance"];
                    scDetailList.Add(sc);
                }
            }
            return scDetailList;
        }

        public List<IncomingCode> showSupplierTransaction(string id)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                //only for display
                string displayItems = @" select scD.Balance, scD.IncomingQty, scD.SupplierName 
	                                 from StockCardDetails scD
	                                 where scD.StockCardID='" + id + "'";

                SqlCommand cmd1 = new SqlCommand(displayItems, connection);
                SqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    IncomingCode ic = new IncomingCode();
                    ic.Balance = (int)reader["Balance"];
                    ic.IncomingQty = (int)reader["IncomingQty"];
                    ic.SupplierName = (string)reader["SupplierName"];
                    incomingList.Add(ic);
                }
            }
            return incomingList;
        }

        public void addDisTranIntoStockCard(StockCradDetails sc)
        {
            string isStatus = "delivered";
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into StockCardDetails( StockCardID, DisbursementID, Balance, IncomingQty, SupplierName) values('" + sc.StockCardID + "','" + sc.DisbursementID + "'," + sc.Balance + "," + sc.DeliveredQty + ",'" + sc.Departmentname + "')";
                string updateItems = @"Update DisbursementDetails Set DisbursementDetailsStatus= '" + isStatus + "' WHERE DisbursementDetailsID ='" + sc.DisbursementDetailsID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlCommand cmd1 = new SqlCommand(updateItems, connection);
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
            }
        }

        public void addIntoStockCard(IncomingCode ic)
        {
            string isStatus = "delivered";
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into StockCardDetails( StockCardID, Balance, IncomingQty, SupplierName) values('" + ic.StockCardID + "'," + ic.Balance + "," + ic.IncomingQty + ",'" + ic.SupplierName + "')";
                string updateItems = @"Update IncomingStock Set IncomingStockStatus= '" + isStatus + "' WHERE StockCardID ='" + ic.StockCardID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlCommand cmd1 = new SqlCommand(updateItems, connection);
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
            }

        }

        public int getLatestBalance(string stockCardID, string id)
        {
            int balance = 0;
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select top 1 scD.Balance
                                     from StockCardDetails scD
                                    where scD.StockCardID=" + stockCardID +
                                    "order by scD.StockCardDetailsID desc";

                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    balance = (int)reader["Balance"];

                }

            }
            return balance;
        }

        public int getReorderLevel(string id)
        {
            int reorderLevel = 0;
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"select ReorderLevel from Stationery
                                    where ItemID = '" + id + "'";

                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reorderLevel = (int)reader["ReorderLevel"];
                }
            }
            return reorderLevel;
        }

    }
}