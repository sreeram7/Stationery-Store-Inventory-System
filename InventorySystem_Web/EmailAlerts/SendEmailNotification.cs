using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;



namespace LogicUniversityWeb.EmailAlerts
{
    public class SendEmailNotification
    {
        public void SendEmail(String To,String subject,String Body)
        {
         MailMessage mm = new MailMessage("logicuniversity.team6@gmail.com",To);

            mm.Subject = subject;
            mm.Body = Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("logicuniversity.team6@gmail.com", "Idontellyou");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
        }

        public void SendEmailHTML(String To, String subject, String Body)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("logicuniversity.team6@gmail.com", To, System.Text.Encoding.UTF8);
            mm.To.Add(To);
            mm.Subject = subject;
            mm.Body = Body;
            mm.IsBodyHtml = true;
            mm.BodyEncoding = System.Text.Encoding.UTF8;
            mm.SubjectEncoding = System.Text.Encoding.UTF8;
            mm.Priority = MailPriority.High;

            NetworkCredential nc = new NetworkCredential("logicuniversity.team6@gmail.com", "Idontellyou");
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
        }
    }
}