using Project.BLL.EmailSender.IEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.EmailSender.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("tahozturk@stu.okan.edu.tr", "Tahaozturk41!")
            };

            return client.SendMailAsync(
                new MailMessage(from: "tahozturk@stu.okan.edu.tr",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
