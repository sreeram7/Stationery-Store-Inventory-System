using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace LogicUniversityWeb.OTP
{
    public class SendOTP
    {
        public static string sendSMS(string receiverNumber, string messageBody)
        {
            String result;
            string apiKey = "TR8c2Wxi8Mc-FWvP9alR3G9Ji7M0Bp10eUsOqVfFtx"; //API Key Generated
            string numbers = receiverNumber;               //give Sender Mobile number  // in a comma seperated list
            string sender = "Team6";


            //sender name to whom we need to send 
            string message = messageBody;                              //"Dear" + name + "! Your OTP for is " + randomNumber;

            String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            //refer to parameters to complete correct url string

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
                string response = "Unable to send because!" + e.Message;

                return response;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }
    }
}