using AnglicanGeek.MarkdownMailer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace iFrames.Classes
{
    public class MessageService
    {
        public bool SendEmail(MailAddress toAddress, string body)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.Subject = String.Format(CultureInfo.CurrentCulture, "[{0}] Please verify your account.", "MutualFundIndia");
                mailMessage.Body = body;
                mailMessage.From = new MailAddress("support@icraonline.com", "MutualFundIndia");

                mailMessage.To.Add(toAddress);
                return SendMessage(mailMessage);
            }
        }

        private bool SendMessage(MailMessage mailMessage)
        {
            try
            {
                var _mailSender = MailConfiguration.getConfig();

                _mailSender.Send(mailMessage);
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
                // Log but swallow the exception
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


    }

    public static class MailConfiguration
    {
        public static MailSender getConfig()
        {
            var configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            var mailSenderConfiguration = new MailSenderConfiguration
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = mailSettings.Smtp.Network.Host,
                Port = mailSettings.Smtp.Network.Port,
                EnableSsl = mailSettings.Smtp.Network.EnableSsl
            };
            mailSenderConfiguration.UseDefaultCredentials = false;
            mailSenderConfiguration.Credentials = new NetworkCredential(
                mailSettings.Smtp.Network.UserName,
                mailSettings.Smtp.Network.Password);

            return new MailSender(mailSenderConfiguration);

        }
    }
}