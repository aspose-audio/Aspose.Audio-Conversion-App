using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AudioVedio.Apis.Services
{
    public class EmailService
    {
        public static bool SendFile(string url, string email,ILogger logger) {

            if (!IsValidEmail(email)) {
                throw new AppException("Email address is invalid");
            }
            if (!IsValidUrl(url))
            {
                throw new AppException("Url is invalid");
            }
            var body=PopulateBody("Free Online Visio Apps", url, "Your files have been processed successfully");
            return SendEmail(email, AppConfig.FromEmailAddress, "Download File", body, logger);
        }


        internal static bool SendFeedback(string subject, string body, ILogger logger)
        {
            return SendEmail(AppConfig.FeedbackEmail,AppConfig.FromEmailAddress, subject,body, logger);
        }

        private static bool SendEmail(string toEmailAddress, string fromEmailAddress, string subject, string body, ILogger logger)
        {
            Task.Factory.StartNew(() =>
            {
                SmtpClient smtp = new SmtpClient();
                MailMessage message = new MailMessage();
                try
                {
                    message.To.Add(toEmailAddress);
                    message.From = new MailAddress(fromEmailAddress);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = AppConfig.MailServer;
                    smtp.Port = AppConfig.MailServerPort;
                    smtp.Timeout = AppConfig.MailServerTimeOut;

                    smtp.EnableSsl = true;

                    smtp.Credentials = new NetworkCredential(AppConfig.MailServerUserId, AppConfig.MailServerUserPassword);
                    if (message.To.Count > 0)
                    {
                        smtp.Send(message);
                    }

                }
                catch (Exception e) {
                    logger.LogError(e, e.ToMessage("no session"));
                    throw e;
                }
                finally
                {
                    message.Dispose();
                }

            });
           
            return true;
        }

        private static string PopulateBody(string featureTitle, string url, string successMessage)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(AppContext.BaseDirectory, "App_Data/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FeatureTitle}", featureTitle);
            body = body.Replace("{Url}", HttpUtility.UrlDecode(url));
            body = body.Replace("{SuccessMessage}", successMessage);
            return body;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidUrl(string url)
        {
            var downloadUrl = new Uri(url);
            return downloadUrl.Host.EndsWith(".amazonaws.com");
        }
    }
}
