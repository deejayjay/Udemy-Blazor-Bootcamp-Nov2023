using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TangyWeb_API.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public string Username { get; set; }

        public string Password { get; set; }

        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration config)
        {
            _config = config;

            Username = _config["SmtpSettings:Username"];
            Password = _config["SmtpSettings:Password"];
            // SendGridSecret = _config["SendGrid:SecretKey"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // NOTE: Commented out SendGrid code because the sender identity need to be verified to send emails.
                //var client = new SendGridClient(SendGridSecret);
                //var from = new EmailAddress("hello@dotnetmastery.com", "Tangy");
                //var to = new EmailAddress(email);
                //var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

                //await client.SendEmailAsync(msg);

                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("hello@dotnetmastery.com"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlMessage
                };

                // Send email
                using var emailClient = new SmtpClient();
                await emailClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await emailClient.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
                await emailClient.SendAsync(emailToSend);
                await emailClient.DisconnectAsync(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
