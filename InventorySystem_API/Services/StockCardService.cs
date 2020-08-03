
using LogicUniversityAPI.DataBase;
using LogicUniversityAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Services
{
    public class StockCardService
    {
        List<StockCard> scList = new List<StockCard>();
        List<StockCradDetails> scDetailList = new List<StockCradDetails>();
        List<IncomingCode> incomingList = new List<IncomingCode>();
        public List<StockCard> createStockCardList()
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                string getItems = @"select s.ItemName, s.ItemID
                                from Stationery s";
                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockCard sc = new StockCard();
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
                                     where s.CategoryID = c.CategoryID and s.ItemID='"+ ItemID+"'"+
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

        public void createDisbursementTransaction(string ItemID)
        {
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select  dl.DisbursementID, scDetails.Balance- dd.DeliveredQty as Balance, d.Departmentname
                    from DisbursementDetails dd, DisbursementList dl, Department d, StockCardDetails scDetails
                    where dl.DisbursementID = dd.DisbursementID and  d.DepartmentID= dd.DepID and
                    dl.DisbursementStatus = 'delivered' and dd.DisbursementDetailsStatus ='pending'and dd.ItemID = '" + ItemID + "' and " +
                    "scDetails.Balance in(select scDetails.Balance from StockCardDetails scDetails, StockCard sc " +
                    "where sc.StockCardID = scDetails.StockCardID and sc.ItemID = '"+ ItemID +"')";

                SqlCommand cmd = new SqlCommand(getItems, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    StockCradDetails sc = new StockCradDetails();
                    sc.DisbursementID = (int)reader["DisbursementID"];
                    sc.Balance = (int)reader["Balance"];
                    sc.Departmentname = (string)reader["Departmentname"];
                    addDisTranIntoStockCard(sc);
                }
                
            }
        }

        public void createSupplierTransaction(string ItemID)
        {            
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {

                connection.Open();
                //Update into database
                string getItems = @"select s.SupplierName, iss.IncomingQty+scDetail.Balance as Balance, sc.StockCardID
                        from Supplier s, IncomingStock iss, StockCard sc, PurchaseOrder po, StockCardDetails scDetail
                        where sc.StockCardID = iss.StockCardID and s.SupplierID = iss.SupplierID and po.SupplierID = s.SupplierID 
                        and po.PurchaseOrderStatus= 1  and iss.IncomingStockStatus='pending' and po.ItemID='" + ItemID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    IncomingCode ic = new IncomingCode();
                    ic.SupplierName = (string)reader["SupplierName"];
                    ic.Balance = (int)reader["Balance"];
                    ic.StockCardID = (string)reader["StockCardID"];

                    addIntoStockCard(ic);
                }
            }
        }

        public List<StockCradDetails> showDisbursementTransaction(string ItemID)
        {            
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();

                string getItems = @"select d.Departmentname, scDetails.Balance, dd.DeliveredQty
                        from StockCardDetails scDetails, DisbursementDetails dd, DisbursementList dl, Department d
                        where scDetails.DisbursementID = dd.DisbursementID and dl.DisbursementID= dd.DisbursementID
                        and dl.DepID = d.DepartmentID and dd.ItemID='" + ItemID + "'";
              
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
       
        public List<IncomingCode> showSupplierTransaction(string ItemID)
        {            
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                //only for display
                string displayItems = @"select s.SupplierName, iss.IncomingQty, scDetails.Balance, sc.StockCardID
                        from Supplier s, IncomingStock iss, StockCardDetails scDetails, StockCard sc
                        where  sc.StockCardID = scDetails.StockCardID and iss.StockCardID = sc.StockCardID and iss.SupplierID = s.SupplierID
                            and sc.ItemID='" + ItemID+"'";              
               
                SqlCommand cmd1 = new SqlCommand(displayItems, connection);
                SqlDataReader reader = cmd1.ExecuteReader();                
                while (reader.Read())
                {
                    IncomingCode ic = new IncomingCode();
                    ic.SupplierName = (string)reader["SupplierName"];
                    ic.IncomingQty = (int)reader["IncomingQty"];
                    ic.Balance = (int)reader["Balance"];
                    ic.StockCardID = (string)reader["StockCardID"];
                    
                    incomingList.Add(ic);
                }   
            }
            return incomingList;
        }

        public void addDisTranIntoStockCard(StockCradDetails sc)
        {
            System.Guid scDetailsID = System.Guid.NewGuid();
            string isStatus = "delivered";
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into StockCardDetails(StockCardDetailsID, DisbursementID, Balance) values('" + scDetailsID + "','" + sc.DisbursementID + "'," + sc.Balance + ")";
                string updateItems = @"Update DisbursementDetails Set DisbursementDetailsStatus= '" + isStatus + "' WHERE DisbursementID ='" + sc.DisbursementID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlCommand cmd1 = new SqlCommand(updateItems, connection);
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
            }
        }
       
        public void addIntoStockCard(IncomingCode ic)
        {
            System.Guid scDetailsID = System.Guid.NewGuid();
            string isStatus = "delivered";
            using (SqlConnection connection = new SqlConnection(DataLink.connectionString))
            {
                connection.Open();
                string getItems = @"insert into StockCardDetails(StockCardDetailsID, StockCardID, Balance) values('" + scDetailsID+"','"+ ic.StockCardID + "'," + ic.Balance + ")";
                string updateItems = @"Update IncomingStock Set IncomingStockStatus= '" + isStatus + "' WHERE StockCardID ='" + ic.StockCardID + "'";
                SqlCommand cmd = new SqlCommand(getItems, connection);
                SqlCommand cmd1 = new SqlCommand(updateItems, connection);
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
            }
        }

        }
}