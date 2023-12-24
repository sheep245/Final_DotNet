using MimeKit;
using System.Net;
using System.Net.Mail;

namespace Point_Of_Sales.Helpers
{
    public class Mailer
    {
        private readonly IConfiguration _configuration;
        public Mailer(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string content)
        {
            var host = _configuration["MailSettings:Server"];
            var username = _configuration["MailSettings:Username"];
            var pwd = _configuration["MailSettings:Password"];
            var senderName = _configuration["MailSettings:MailSender"];

            var smtp = new SmtpClient()
            {
                Host = host,
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(username, pwd),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage()
            {
                From = new MailAddress(username, senderName),
                Subject = subject,
                Body = content
            };

            message.To.Add(to);
            smtp.Send(message);
        }
    }
}
