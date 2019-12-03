using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SendGrid;
using SendGrid.Helpers.Mail;

using System.Globalization;

namespace PollsAPI.Services
{
    public class MessageService : IMessageService
    {
        public void SendActivationMail(string email, string name, Guid guid)
        {
            var client = new SendGridClient(Config.sendGridApiKey);
            var from = new EmailAddress("noreply@angularpolls.com", "AngularPolls");
            var to = new EmailAddress(email, name);
            var plainTextContent = "";
            var htmlContent = "<p>Hi " + name + "!<p><p>In order to start using AngularPolls, please confirm your email address by clicking the activation link below.</p><p><a href=\"" + Config.appFrontEndUrl + "/activate/" + guid.ToString() + "\">Confirm email address</a></p><p>Kind regards</p><p>AngularPolls</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, "Confirm your email address", plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }

        public void SendInvitationMail(string email, string inviterName, Guid guid)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            var client = new SendGridClient(Config.sendGridApiKey);
            var from = new EmailAddress("noreply@angularpolls.com", "AngularPolls");
            var to = new EmailAddress(email);
            var plainTextContent = "";
            var htmlContent = "<p>Hi!<p><p>You have been invited by " + inviterName + " to join AngularPolls! To create your account please click the link below.</p><p><a href=\"" + Config.appFrontEndUrl + "/invitation/" + guid.ToString() + "\">Create my account</a></p><p>Have fun using AngularPolls!</p><p>Kind regards</p><p>AngularPolls</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, myTI.ToTitleCase(inviterName) + " invited you to join AngularPolls!", plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }
    }
}
