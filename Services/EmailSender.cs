using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace Site.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Support Mon Site","adresse@mail.com"));
            Message.To.Add(new MailboxAddress(email, email));
            Message.Subject = subject;
            var body = new BodyBuilder();
            body.HtmlBody = htmlMessage;
            Message.Body=body.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.monsmtp.fr", 000, false);
                smtpClient.Authenticate("adresse@mail.com", "monMDP");
                await smtpClient.SendAsync(Message);
                smtpClient.Disconnect(true);
            }
        }
    }
}