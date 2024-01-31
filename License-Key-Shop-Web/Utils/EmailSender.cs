using License_Key_Shop_Web.MyInterface;
using System.Net;
using System.Net.Mail;

namespace License_Key_Shop_Web.Utils
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml)
        {
            var senderMail = "hieptvhe173252@gmail.com";
            var senderPassword = "xrub dxja ykrc nwuk";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderMail, senderPassword)
            };

            var senderAddress = new MailAddress(senderMail, "License Shop");
            var mailMessage = new MailMessage
            {
                From = senderAddress,
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}
