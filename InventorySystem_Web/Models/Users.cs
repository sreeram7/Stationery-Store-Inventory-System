using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityWeb.Models
{
    public class Users
    {
        public int UserID { get; set; }

        [Remote("IsChecked", "Login", HttpMethod = "POST", ErrorMessage = " special characters not allowed")]
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username:")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Passcode { get; set; }
        public string EmailID { get; set; }
        public string role { get; set; }
        public string MobileNo { get; set; }
        public string DeptID_FK { get; set; }

        public string URL { get; set; }
    }
}