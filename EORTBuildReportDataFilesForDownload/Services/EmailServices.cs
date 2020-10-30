using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class EmailServices
    {
        public static void BuildSuccessEmail(int runID, string toEmailAddress)
        {
            MailMessage message = null;
            try
            {
                string outputURL = string.Empty;
                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    message = new MailMessage();
                    string body = "Hello,<br/><br/>" +
                        "Your Requested Raw Data Reports for Run ID: " + runID + " are ready for download. The report will be available for the next 24 hours and will be deleted after that.<br/>" +
                        "To download the Raw Data login to the Tool, go to the 'Completed Runs' Page, find Run ID: " + runID + " and click the download icon in the 'Raw Data' column.<br/><br/>" + 
                        "Thanks!<br/><br/>" +
                        "Executive Overview Reporting Tool";

                    message.To.Add(new MailAddress(toEmailAddress));
                    message.From = new MailAddress(AppSettingServices.GetAppSetting("fromEmailAddress"));
                    message.Subject = "Requested Raw Data for Run ID: " + runID + " ready for download!";
                    message.IsBodyHtml = true;
                    message.Body = body;
                    SendEmail(message);
                }
                else
                {
                    throw new Exception("User's Email Address could not be found so the email could not be sent.");
                }
            }
            catch (Exception ex)
            {
                ErrorServices.LogServicesError(ex);
            }
        }

        public static void BuildFailEmail(int runID, string toEmailAddress, Exception exceptionDetails)
        {
            MailMessage message = null;
            try
            {
                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    message = new MailMessage();
                    string body = "Hello,<br/><br/>" +
                            "Your Requested Raw Data Reports for Run ID: " + runID + " has failed. The reason is listed below. Please try again later.<br/><br/>" +
                            "Error Message: <br/>" +
                            exceptionDetails.Message + "<br/><br/>" +
                            "Sorry!<br/><br/>" +
                            "Executive Overview Reporting Tool";

                    message.To.Add(new MailAddress(toEmailAddress));
                    message.From = new MailAddress(AppSettingServices.GetAppSetting("fromEmailAddress"));
                    message.Bcc.Add(new MailAddress(AppSettingServices.GetAppSetting("developerEmailAddress")));
                    message.Subject = "Requested Raw Data Reports Not Created!";
                    message.IsBodyHtml = true;
                    message.Body = body;
                    SendEmail(message);
                }
                else
                {
                    throw new Exception("User's Email Address could not be found so the email could not be sent.");
                }
            }
            catch (Exception ex)
            {
                ErrorServices.LogServicesError(ex);
            }
        }

        public static void SendEmail(MailMessage message)
        {
            if (message != null)
            {
                try
                {
                    SmtpClient client = new SmtpClient()
                    {
                        Host = "nasmail.skillsoft.com",
                        Port = 25,
                        EnableSsl = true,
                        Timeout = 300000,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                    };
                    client.Send(message);
                }
                catch (SmtpException smtpEx)
                {
                    throw smtpEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("The email could not be created and sent. Please try again later.");
            }
        }
    }
}
