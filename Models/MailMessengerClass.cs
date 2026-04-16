using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Description;
using System.Configuration;

namespace MajorProject_HRMS_APP25.Models
{
    public class MailMessengerClass
    {
        public int SendMail(string reciever_email_var, string subject_var, string body_var)
        {
            try
            {
                string senderEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string senderPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];

                // Compose the mail using inbuilt class called MailMessage
                MailMessage MailMessageObj = new MailMessage("example@email.com", reciever_email_var, subject_var, body_var);// first should be subject then body regardless of parameter order of the method/function.
                MailMessageObj.IsBodyHtml = true; // To check if body is text or not. If text then it is false.

                // send the composed mail over the internet.
                SmtpClient smtpClientObj = new SmtpClient();
                smtpClientObj.Host = smtpHost;
                smtpClientObj.Port = 587;
                smtpClientObj.EnableSsl = true;
                smtpClientObj.Credentials = new NetworkCredential(senderEmail, senderPassword);

                // send the mail via smtp
                smtpClientObj.Send(MailMessageObj);

                return 1;
            }
            catch
            {
                return 0;

            }
        }
    }
}