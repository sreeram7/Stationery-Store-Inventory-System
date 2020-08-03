using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using LogicUniversityWeb.DataBase;
using LogicUniversityWeb.EmailAlerts;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Services;

namespace LogicUniversityWeb.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult StockTrendAnalysis()
        {
            StockTrend stockTrend = new StockTrend();

            List<Category> categories = stockTrend.GetCategories();
            List<Department> departments = stockTrend.GetDepartments();
            ViewBag.categories = categories;
            ViewBag.departments = departments;
            return View();
        }
        [HttpPost]
        public ActionResult GenerateStockTrendAnalysis(string selectedcategory,string selecteddepartment)
        {
            //string departmentName = DepartmentData.GetDepartmentByDepartmentId(selectdepartment).Name;
            List<string> datelist_X = new List<string>();
            List<int> quantitylist_Y = new List<int>();

           
            
                string dateFrom = $"{DateTime.Now.AddMonths(1).Month}/{DateTime.Now.AddMonths(1).Year}";
               string  dateTo = $"{DateTime.Now.AddMonths(3).Month}/{DateTime.Now.AddMonths(3).Year}";
                datelist_X = GetDateList(dateFrom, dateTo);

                for (int i = 1; i <= 3; i++)
                {
                    int month = DateTime.Today.AddMonths(i).Month;
                    int year = DateTime.Today.AddMonths(i).Year;
                string y = GetPredictedStockQuantity(selectedcategory, selecteddepartment, month, year);

                    int Y = int.Parse(y);
                    quantitylist_Y.Add(Y);
                }
           

            Report reportForm = new Report()
            {
                Axis_X = datelist_X,
                Axis_Y = quantitylist_Y,
                Category = selectedcategory,
                Department = selecteddepartment
            };
            ViewBag.ReportForm = reportForm;
            return View();
        }
        public static List<string> GetDateList(string dateFrom, string dateTo)
        {
            string[] s1 = dateFrom.Split('/');
            int monthfrom = int.Parse(s1[0]);
            int yearfrom = int.Parse(s1[1]);

            string[] s2 = dateTo.Split('/');
            int monthto = int.Parse(s2[0]);
            int yearto = int.Parse(s2[1]);

            List<string> datelist = new List<string>();
            while ((yearfrom != yearto) || (monthfrom != monthto))
            {
                string s = monthfrom + "/" + yearfrom;
                if (monthfrom < 10)
                    s = "0" + s;

                datelist.Add(s);
                if (monthfrom != 12)
                {
                    monthfrom = monthfrom + 1;
                }
                else
                {
                    monthfrom = 1;
                    yearfrom = yearfrom + 1;
                }
            }
            string last_s = monthto + "/" + yearto;
            if (monthto < 10)
                last_s = "0" + last_s;
            datelist.Add(last_s);
            return datelist;
        }
        public string GetPredictedStockQuantity(string selectcategory, string selectdepartment, int month, int year)
        {
            Task<string> task = Task.Run(async () => await PredictStockQuantity(selectcategory, selectdepartment, month, year));
            return task.Result;
        }
        public async Task<string> PredictStockQuantity(string selectcategory, string selectdepartment, int month, int year)
        {
            using (var client = new HttpClient())
            {
                PredictViewModel predModel = new PredictViewModel();
                predModel.CategoryName = selectcategory;
                predModel.Departmentname = selectdepartment;
                predModel.Month = month;
                predModel.Year = year;

                HttpResponseMessage res = await client.PostAsJsonAsync("http://127.0.0.1:5000/", predModel);

                if (res.IsSuccessStatusCode)
                {
                    string predicted = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine("PREDICTION : " + predicted);

                    return predicted;
                }
                else
                {
                    Debug.WriteLine("ERROR");
                    return "error";
                }
            }
        }


    }
}