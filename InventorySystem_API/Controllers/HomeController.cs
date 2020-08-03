using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LogicUniversityAPI.Models;
using LogicUniversityAPI.DataBase;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace LogicUniversityAPI.Controllers
{
   public class HomeController : ApiController
   {
        [HttpGet]
       // [Route("Home/Login")]
        public HttpResponseMessage Login(string username,string passcode)
        {
            if (username == null || passcode == null)
            {
                var message = Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,"Please input Credentials");

                return message;
            }                                                    
            else
                    {
                string Hash_Password = GetMD5Hash(passcode);

                Users userinfo = Data_Users.GetUserInfo(username);

                if (userinfo == null || userinfo.Passcode != Hash_Password)
                {
                    var message = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Please input Valid Credentials");                                        //display home screen

                    return message;
                }
                else
                {
                    var message = Request.CreateResponse(HttpStatusCode.Created, userinfo);

                    return message;
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

       
    }
}
