using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;
using System.Threading.Tasks;

namespace InternetShop.Data.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Подтверждение почты", "electroshopf@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("electroshopf@gmail.com", "qsxzebhcgtnyizbo");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
