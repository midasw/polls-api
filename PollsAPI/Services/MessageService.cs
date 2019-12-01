using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace PollsAPI.Services
{
    public class MessageService : IMessageService
    {
        public void Send(string email, string subject, string message)
        {
            var client = new SendGridClient(Config.sendGridApiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var to = new EmailAddress(email, "Example User");
            var plainTextContent = message;
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }

        public void SendActivationMail(string email, string name, Guid guid)
        {
            var client = new SendGridClient(Config.sendGridApiKey);
            var from = new EmailAddress("noreply@angularpolls.com", "AngularPolls");
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<p>Hi " + name + "!<p><p>In order to start using AngularPolls, please confirm your email address by clicking the activation link below.</p><p><a href=\"http://localhost:4200/activate/" + guid.ToString() + "\">Confirm email address</a></p><p>Kind regards</p><p>AngularPolls</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "Confirm your email address", plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }
    }
}
