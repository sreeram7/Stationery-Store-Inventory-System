using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityAPI.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Passcode { get; set; }
        public string EmailID { get; set; }
        public string role { get; set; }
        public string SessionID { get; set; }
        public string DeptID_FK { get; set; }

        public string URL { get; set; }
    }
}