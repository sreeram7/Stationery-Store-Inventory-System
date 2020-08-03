using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.DataBase;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace LogicUniversityWeb.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(Users s, string ReturnUrl)
        {
            if (s.Username == null || s.Passcode == null)
                return View();                                            //display home screen
            else
            {
                string Hash_Password = GetMD5Hash(s.Passcode);

                Users userinfo = Data_Users.GetUserInfo(s.Username);

                if (userinfo == null || userinfo.Passcode != Hash_Password)
                {
                    Debug.WriteLine("I am lost here!");
                    return View();                                             //display home screen
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(userinfo.Username, false);
                    Session["UserID"] = userinfo.UserID;
                    Session["DeptID"] = userinfo.DeptID_FK;
                    Session["user"] = userinfo;
                }
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                if (userinfo.role == "DepRep" || userinfo.role == "DepStaff" || userinfo.role == "DepHead" || userinfo.role == "InterimHead")
                {
                    return RedirectToAction("Home", "DepartmentRep", User);
                }

                else
                {
                    return RedirectToAction("Home", "Supplier", User);

                }
                
            }
        }
        public static string GetMD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));            //Compute hash from the bytes of text
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));                    //Each byte is changed into 2-hexadecimal digits  
            }
            return strBuilder.ToString();
        }

        [HttpPost]
        public JsonResult IsChecked(string Username)
        {
            if (Regex.IsMatch(Username.ToString(), "^[a-zA-Z0-9]+$"))
                return Json(true);
            return Json(false);
        }

        public ActionResult Logout()
        {
            //Data_Sessions.DeleteSession(sessionId);
            Session.Clear();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}
