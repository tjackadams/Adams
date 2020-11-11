using System.Threading.Tasks;
using Adams.Services.Identity.Api.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Adams.Services.Identity.Api.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptionsMonitor<EmailSenderOptions> _options;

        public SendGridEmailSender(IOptionsMonitor<EmailSenderOptions> options)
        {
            _options = options;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_options.CurrentValue.Key);
            var message = new SendGridMessage
            {
                From = new EmailAddress("noreply@itadams.co.uk", "Adams Identity"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };

            message.AddTo(new EmailAddress(email));

            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }
    }
}