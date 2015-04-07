using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace FootballOracle.Foundation.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            var credentialUserName = "administrator@footballoracle.co.uk";
            var sentFrom = "administrator@footballoracle.co.uk";
            var pwd = "S3pt197!";

            // Configure the client:
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("mail.netcetera.co.uk");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}
