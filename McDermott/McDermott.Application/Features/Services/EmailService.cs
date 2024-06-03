using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body, byte[] attachment = null, string attachmentName = null)
        {
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Your Name", _smtpUser));
            //message.To.Add(new MailboxAddress(to));
            //message.Subject = subject;

            //var bodyBuilder = new BodyBuilder { HtmlBody = body };

            if (attachment != null && attachmentName != null)
            {
                //bodyBuilder.Attachments.Add(attachmentName, attachment);
            }

            //message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                //client.Connect(_smtpServer, _smtpPort, false);
                //client.Authenticate(_smtpUser, _smtpPass);
                //await client.SendAsync(message);
                //client.Disconnect(true);
            }
        }
    }
}