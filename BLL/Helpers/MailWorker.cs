using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using Microsoft.Extensions.Options;

namespace BLL.Helpers
{
    public class MailWorker : IMailWorker
    {
        public string Code { get; set; }
        private readonly AppSettings _appSettings;
        private readonly ICurrentUser _currentUser;

        public MailWorker(IOptions<AppSettings> appSettings, ICurrentUser currentUser)
        {
            this._currentUser = currentUser;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<bool> SendCodeByEmailAsync(string emailTo)
        {
            try
            {
                var random = new Random();
                Code = random.Next(100000, 999999).ToString(CultureInfo.InvariantCulture);
                var toMailAddress = new MailAddress(emailTo);
                var subject = "AIM APP | Confirmation code";
                var body =
                    $"<h4>Confirmation code to enter the application:</h4><br><center><code><b>{Code}</b></center></code>";

                await SendMailMessageAsync(toMailAddress, subject, body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendInvitationEmailAsync(Room room, string url, string emailTo)
        {
            try
            {
                var random = new Random();
                Code = random.Next(100000, 999999).ToString(CultureInfo.InvariantCulture);
                var toMailAddress = new MailAddress(emailTo);
                var subject = "AIM APP | Invitation To Room";
                var body = $"Invitation to join {room.RoomName} from {_currentUser.User.UserName}: {url}";

                await SendMailMessageAsync(toMailAddress, subject, body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        private async Task<bool> SendMailMessageAsync(MailAddress mailAddressTo, string subject, string body)
        {
            try
            {
                var fromMailAddress = new MailAddress(_appSettings.Email, _appSettings.EmailDisplayName);

                var message = new MailMessage(fromMailAddress, mailAddressTo);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient(_appSettings.SmtpSettings.Host, _appSettings.SmtpSettings.Port);
                smtpClient.Credentials =
                    new NetworkCredential(fromMailAddress.Address, _appSettings.SmtpSettings.Password);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CompareCodes(string codeFromUser)
        {
            if (Code == null)
            {
                return false;
            }

            return codeFromUser == Code;
        }
    }
}
