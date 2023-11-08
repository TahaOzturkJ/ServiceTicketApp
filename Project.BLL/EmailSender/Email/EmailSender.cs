using Project.BLL.EmailSender.IEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Project.ENTITY.Models;

namespace Project.BLL.EmailSender.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("teknik@bbsyazilim.com", "2019Ecrin1982*")
            };

            var mailMessage = new MailMessage(from: "teknik@bbsyazilim.com", to: email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            return client.SendMailAsync(mailMessage);
        }

    }
}
