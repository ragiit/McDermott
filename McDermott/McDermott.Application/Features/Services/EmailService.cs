using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "srv42.niagahoster.com";
        private readonly int _smtpPort = 465;
        private readonly string _smtpUser = "nuralimajid@matrica.co.id";
        private readonly string _smtpPass = "nuralimajid";

        public async Task SendEmailAsync(string to, string subject, string body, byte[] attachment = null, string attachmentName = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Nur Ali Majid", _smtpUser));
            //message.To.Add(new MailboxAddress.);
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };

            if (attachment != null && attachmentName != null)
            {
                bodyBuilder.Attachments.Add(attachmentName, attachment);
            }

            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
        }
    }
}